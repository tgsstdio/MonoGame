// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.IO;
using System.Runtime.InteropServices;
using Magnesium;
using System.Collections.Generic;
using System.Diagnostics;

/*
* Vulkan Example - Texture loading (and display) example (including mip maps)
* Code translated to 
* Copyright (C) 2016 by Sascha Willems - www.saschawillems.de
*
* This code is licensed under the MIT license (MIT) (http://opensource.org/licenses/MIT)
*/

namespace MonoGame.Core
{
	public partial class Texture2DPlatform : ITexture2DPlatform
    {		

		// Load all available mip levels into linear textures
		// and copy to optimal tiling target
		private class MipLevel 
		{
			public MgImage image;
			public MgDeviceMemory memory;
		};

		// Contains all Vulkan objects that are required to store and use a texture
		// Note that this repository contains a texture loader (vulkantextureloader.h)
		// that encapsulates texture loading functionality in a class that is used
		// in subsequent demos
		struct Texture {
			public MgSampler sampler;
			public MgImage image;
			public MgImageLayout imageLayout;
			public MgDeviceMemory deviceMemory;
			public MgImageView view;
			public UInt32 width, height;
			public UInt32 mipLevels;
		};

		private MgPhysicalDeviceMemoryProperties mMemoryProperties;
		private bool getMemoryType(UInt32 typeBits, MgMemoryPropertyFlagBits requirementsMask, out UInt32 typeIndex)
		{
			// Search memtypes to find first index with those properties
			for (UInt32 i = 0U; i < mMemoryProperties.MemoryTypes.Length; i++)
			{
				if ((typeBits & 1) == 1)
				{
					// Type is available, does it match user properties?
					if ((mMemoryProperties.MemoryTypes[i].PropertyFlags & requirementsMask) == requirementsMask)
					{
						typeIndex = i;
						return true;
					}
				}
				typeBits >>= 1;
			}
			// No memory types matched, return failure
			typeIndex = 0U;
			return false;
		}

			// Create an image memory barrier for changing the layout of
		// an image and put it into an active command buffer
		static void setImageLayout(IMgCommandBuffer buffer, MgImage image, MgImageAspectFlagBits aspectMask, MgImageLayout oldImageLayout, MgImageLayout newImageLayout, UInt32 baseMipLevel, UInt32 levelCount)
		{
			// Create an image barrier object
			var imageMemoryBarrier = new MgImageMemoryBarrier
			{
				OldLayout = oldImageLayout,
				NewLayout = newImageLayout,
				Image = image,
				SubresourceRange = new MgImageSubresourceRange{
					AspectMask = aspectMask,
					BaseMipLevel = baseMipLevel,
					LevelCount = levelCount,
					LayerCount = 1,
				},
			};

			// Only sets masks for layouts used in this example
			// For a more complete version that can be used with
			// other layouts see vkTools::setImageLayout

			// Source layouts (new)

			if (oldImageLayout == MgImageLayout.PREINITIALIZED)
			{
				imageMemoryBarrier.SrcAccessMask =  MgAccessFlagBits.HOST_WRITE_BIT | MgAccessFlagBits.TRANSFER_WRITE_BIT;
			}

			// Target layouts (new)

			// New layout is transfer destination (copy, blit)
			// Make sure any reads from and writes to the image have been finished
			if (newImageLayout == MgImageLayout.TRANSFER_DST_OPTIMAL)
			{
				imageMemoryBarrier.DstAccessMask =  MgAccessFlagBits.TRANSFER_READ_BIT | MgAccessFlagBits.HOST_WRITE_BIT | MgAccessFlagBits.TRANSFER_WRITE_BIT;
			}

			// New layout is shader read (sampler, input attachment)
			// Make sure any writes to the image have been finished
			if (newImageLayout == MgImageLayout.SHADER_READ_ONLY_OPTIMAL)
			{
				imageMemoryBarrier.SrcAccessMask = MgAccessFlagBits.HOST_WRITE_BIT | MgAccessFlagBits.TRANSFER_WRITE_BIT;
				imageMemoryBarrier.DstAccessMask = MgAccessFlagBits.SHADER_READ_BIT;
			}

			// New layout is transfer source (copy, blit)
			// Make sure any reads from and writes to the image have been finished
			if (newImageLayout == MgImageLayout.TRANSFER_SRC_OPTIMAL)
			{
				imageMemoryBarrier.DstAccessMask = MgAccessFlagBits.TRANSFER_READ_BIT | MgAccessFlagBits.HOST_WRITE_BIT | MgAccessFlagBits.TRANSFER_WRITE_BIT;
			}

			// Put barrier inside setup command buffer
			buffer.CmdPipelineBarrier(
				// Put barrier on top
				MgPipelineStageFlagBits.TOP_OF_PIPE_BIT, 
				MgPipelineStageFlagBits.TOP_OF_PIPE_BIT, 
				0, 
				null,
				null,
				new [] {imageMemoryBarrier});
		}

		private IMgPhysicalDevice mPhysicalDevice;
		private IMgDevice mDevice;
		private IMgCommandBuffer mSetupCmdBuffer;
		private MgAllocationCallbacks mCallbacks;
		public Texture2DPlatform (
			IMgPhysicalDevice physicalDevice,
			IMgDevice device, 
			IMgCommandBuffer setupCmdBuffer,
			MgAllocationCallbacks callbacks,
			MgPhysicalDeviceMemoryProperties memoryProperties)
		{
			mPhysicalDevice = physicalDevice;
			mDevice = device;
			mSetupCmdBuffer = setupCmdBuffer;
			mCallbacks = callbacks;
			mMemoryProperties = memoryProperties;
		}

		public void Construct(int width, int height, bool mipmap, MgFormat format, SurfaceType type, bool shared)
        {
			MgFormatProperties formatProperties;

			bool forceLinearTiling = false;
			Result err;

			mPhysicalDevice.GetPhysicalDeviceFormatProperties (format, out formatProperties);

          //  Threading.BlockOnUIThread(() =>
            {
				// Only use linear tiling if requested (and supported by the device)
				// Support for linear tiling is mostly limited, so prefer to use
				// optimal tiling instead
				// On most implementations linear tiling will only support a very
				// limited amount of formats and features (mip maps, cubemaps, arrays, etc.)
				bool useStaging = true;

				// Only use linear tiling if forced
				if (forceLinearTiling)
				{
					// Don't use linear if format is not supported for (linear) shader sampling
					useStaging = (formatProperties.LinearTilingFeatures & MgFormatFeatureFlagBits.SAMPLED_IMAGE_BIT) > 0;
				}

				var imageCreateInfo = new MgImageCreateInfo 
				{
					ImageType = MgImageType.TYPE_2D,
					Format = format,
					MipLevels = 1,
					ArrayLayers = 1,
					Samples = MgSampleCountFlagBits.COUNT_1_BIT,
					Tiling = MgImageTiling.LINEAR, 
					Usage = (useStaging) ? MgImageUsageFlagBits.TRANSFER_SRC_BIT : MgImageUsageFlagBits.SAMPLED_BIT,
					SharingMode = MgSharingMode.EXCLUSIVE,
					InitialLayout = MgImageLayout.PREINITIALIZED,
					Flags = 0,
					Extent = new MgExtent3D{
						Width = (UInt32) width,
						Height = (UInt32) height,
						Depth = 1},
				};

				var memAllocInfo = new MgMemoryAllocateInfo();
				MgMemoryRequirements memReqs;

				// copy data 
				if (useStaging)
				{

					var mipLevels = new List<MipLevel>(texture.mipLevels);

					// Load mip levels into linear textures that are used to copy from
					for (int level = 0; level < texture.mipLevels; level++)
					{
						var extent = new MgExtent3D {
							Width = tex2D[level].dimensions().x,
							Height = tex2D[level].dimensions().y,
							Depth = 1,
						};

						err = mDevice.CreateImage(imageCreateInfo, mCallbacks, out mipLevels[level].image);
						Debug.Assert(err == Result.SUCCESS);

						mDevice.GetImageMemoryRequirements(mipLevels[level].image, out memReqs);
						memAllocInfo.AllocationSize = memReqs.Size;

						{
							UInt32 typeIndex; 
							if (getMemoryType (memReqs.MemoryTypeBits, MgMemoryPropertyFlagBits.HOST_VISIBLE_BIT, out typeIndex))
							{
								memAllocInfo.MemoryTypeIndex = typeIndex;
							}
							else
							{
								throw new NotSupportedException ();
							}
						}

						err = mDevice.AllocateMemory(memAllocInfo, mCallbacks, out mipLevels[level].memory);
						Debug.Assert(err == Result.SUCCESS);
						err = mDevice.BindImageMemory(mipLevels[level].image, mipLevels[level].memory, 0);
						Debug.Assert(err == Result.SUCCESS);

						MgImageSubresource subRes = new MgImageSubresource{};
						subRes.AspectMask =  MgImageAspectFlagBits.COLOR_BIT;

						MgSubresourceLayout subResLayout;
						IntPtr data;

						mDevice.GetImageSubresourceLayout(mipLevels[level].image, subRes, out subResLayout);
						Debug.Assert(err == Result.SUCCESS);
						err = mDevice.MapMemory(mipLevels[level].memory, 0, memReqs.Size, 0, out data);
						Debug.Assert(err == Result.SUCCESS);
						/// Copy data here
						//memcpy(data, tex2D[level].data(), tex2D[level].size());
						mDevice.UnmapMemory(mipLevels[level].memory);

						// Image barrier for linear image (base)
						// Linear image will be used as a source for the copy
						setImageLayout(
							mSetupCmdBuffer,
							mipLevels[level].image,
							MgImageAspectFlagBits.COLOR_BIT,
							MgImageLayout.PREINITIALIZED,
							MgImageLayout.TRANSFER_SRC_OPTIMAL,
							0,
							1);
					}

					// Setup texture as blit target with optimal tiling
					imageCreateInfo.Tiling = MgImageTiling.OPTIMAL;
					imageCreateInfo.Usage = MgImageUsageFlagBits.TRANSFER_DST_BIT | MgImageUsageFlagBits.SAMPLED_BIT;
					imageCreateInfo.MipLevels = texture.mipLevels;
					imageCreateInfo.Extent = new MgExtent3D{ Width= texture.width, Height =texture.height, Depth= 1 };

					err = mDevice.CreateImage(imageCreateInfo, mCallbacks, out texture.image);
					Debug.Assert(err == Result.SUCCESS);

					mDevice.GetImageMemoryRequirements(texture.image, out memReqs);

					memAllocInfo.AllocationSize = memReqs.Size;

					{
						UInt32 typeIndex; 
						if (getMemoryType(memReqs.MemoryTypeBits, MgMemoryPropertyFlagBits.DEVICE_LOCAL_BIT, out typeIndex))
						{
							memAllocInfo.MemoryTypeIndex = typeIndex;
						}
						else
						{
							throw new NotSupportedException ();
						}
					}

					err = mDevice.AllocateMemory(memAllocInfo, mCallbacks, out texture.deviceMemory);
					Debug.Assert(err == Result.SUCCESS);
					err = mDevice.BindImageMemory(texture.image, texture.deviceMemory, 0);
					Debug.Assert(err == Result.SUCCESS);

					// Image barrier for optimal image (target)
					// Optimal image will be used as destination for the copy

					// Set initial layout for all mip levels of the optimal (target) tiled texture
					setImageLayout(
						mSetupCmdBuffer,
						texture.image,
						MgImageAspectFlagBits.COLOR_BIT,
						MgImageLayout.PREINITIALIZED,
						MgImageLayout.TRANSFER_DST_OPTIMAL,
						0,
						texture.mipLevels);

					// Copy mip levels one by one
					for (int level = 0; level < texture.mipLevels; ++level)
					{
						// Copy region for image blit
						var copyRegion = new MgImageCopy
						{
							SrcSubresource = new MgImageSubresourceLayers{
								AspectMask = MgImageAspectFlagBits.COLOR_BIT,
								BaseArrayLayer = 0,
								MipLevel = 0,
								LayerCount = 1,
							},
							SrcOffset = new  MgOffset3D{X=0, Y=0, Z=0},

							DstSubresource = new MgImageSubresourceLayers{
								AspectMask = MgImageAspectFlagBits.COLOR_BIT,
								BaseArrayLayer = 0,
								// Set mip level to copy the linear image to
								MipLevel = (UInt32) level,
								LayerCount = 1,
							},
							DstOffset = new  MgOffset3D{X=0, Y=0, Z=0},
							Extent = new MgExtent3D {
								Width = tex2D[level].dimensions().x,
								Height = tex2D[level].dimensions().y,
								Depth = 1,
							},
						};

						// Put image copy into command buffer
						mSetupCmdBuffer.CmdCopyImage(							
							mipLevels[level].image,
							MgImageLayout.TRANSFER_SRC_OPTIMAL,
							texture.image, 
							MgImageLayout.TRANSFER_DST_OPTIMAL,
							new [] {copyRegion});
					}

					// Change texture image layout to shader read after all mip levels have been copied
					texture.imageLayout = MgImageLayout.SHADER_READ_ONLY_OPTIMAL;
					setImageLayout(
						mSetupCmdBuffer,
						texture.image,
						MgImageAspectFlagBits.COLOR_BIT,
						MgImageLayout.TRANSFER_DST_OPTIMAL,
						texture.imageLayout,
						0,
						texture.mipLevels);

					//flushSetupCommandBuffer();
					//createSetupCommandBuffer();

					// Clean up linear images 
					// No longer required after mip levels
					// have been transformed over to optimal tiling
					foreach (var level in mipLevels)
					{
						mDevice.DestroyImage(level.image, mCallbacks);
						mDevice.FreeMemory(level.memory, mCallbacks);
					}
				}
				else
				{
					// Prefer using optimal tiling, as linear tiling 
					// may support only a small set of features 
					// depending on implementation (e.g. no mip maps, only one layer, etc.)

					MgImage mappableImage;
					MgDeviceMemory mappableMemory;

					// Load mip map level 0 to linear tiling image
					err = mDevice.CreateImage(imageCreateInfo, mCallbacks, out mappableImage);
					Debug.Assert(err == Result.SUCCESS);

					// Get memory requirements for this image 
					// like size and alignment
					mDevice.GetImageMemoryRequirements(mappableImage, out memReqs);
					// Set memory allocation size to required memory size
					memAllocInfo.AllocationSize = memReqs.Size;

					// Get memory type that can be mapped to host memory
					//getMemoryType(memReqs.MemoryTypeBits, MgMemoryPropertyFlagBits.HOST_VISIBLE_BIT, out memAllocInfo.MemoryTypeIndex);
					{
						UInt32 typeIndex; 
						if (getMemoryType (memReqs.MemoryTypeBits, MgMemoryPropertyFlagBits.HOST_VISIBLE_BIT, out typeIndex))
						{
							memAllocInfo.MemoryTypeIndex = typeIndex;
						}
						else
						{
							throw new NotSupportedException ();
						}
					}

					// Allocate host memory
					err = mDevice.AllocateMemory(memAllocInfo, mCallbacks, out mappableMemory);
					Debug.Assert(err == Result.SUCCESS);

					// Bind allocated image for use
					err = mDevice.BindImageMemory(mappableImage, mappableMemory, 0);
					Debug.Assert(err == Result.SUCCESS);

					// Get sub resource layout
					// Mip map count, array layer, etc.
					MgImageSubresource subRes = new MgImageSubresource{};
					subRes.AspectMask = MgImageAspectFlagBits.COLOR_BIT;

					MgSubresourceLayout subResLayout;
					IntPtr data = IntPtr.Zero;

					// Get sub resources layout 
					// Includes row pitch, size offsets, etc.
					mDevice.GetImageSubresourceLayout(mappableImage, subRes, out subResLayout);
					Debug.Assert(err == Result.SUCCESS);

					// Map image memory
					err = mDevice.MapMemory(mappableMemory, 0UL, memReqs.Size, 0U, data);
					Debug.Assert(err == Result.SUCCESS);

					// TODO : Copy image data into memory
					//memcpy(data, tex2D[subRes.mipLevel].data(), tex2D[subRes.mipLevel].size());

					mDevice.UnmapMemory(mappableMemory);

					// Linear tiled images don't need to be staged
					// and can be directly used as textures
					texture.image = mappableImage;
					texture.deviceMemory = mappableMemory;
					texture.imageLayout =  MgImageLayout.SHADER_READ_ONLY_OPTIMAL;

					// Setup image memory barrier
					setImageLayout(
						mSetupCmdBuffer,
						texture.image, 
						MgImageAspectFlagBits.COLOR_BIT, 
						MgImageLayout.UNDEFINED, 
						texture.imageLayout,
						0,
						1);
				}

				// For best compatibility and to keep the default wrap mode of XNA, only set ClampToEdge if either
				// dimension is not a power of two.
				var addressMode = MgSamplerAddressMode.REPEAT;
				if (((width & (width - 1)) != 0) || ((height & (height - 1)) != 0))
					addressMode = MgSamplerAddressMode.CLAMP_TO_EDGE;

				// Create sampler
				// In Vulkan textures are accessed by samplers
				// This separates all the sampling information from the 
				// texture data
				// This means you could have multiple sampler objects
				// for the same texture with different settings
				// Similar to the samplers available with OpenGL 3.3
				var samplerInfo = new MgSamplerCreateInfo{
					MagFilter = MgFilter.LINEAR,
					MinFilter = MgFilter.LINEAR,
					MipmapMode = MgSamplerMipmapMode.LINEAR,
					AddressModeU = addressMode,
					AddressModeV = addressMode,
					AddressModeW = addressMode,
					MipLodBias = 0.0f,
					CompareOp =  MgCompareOp.NEVER,
					MinLod = 0.0f,
					// Max level-of-detail should match mip level count
					MaxLod = (useStaging) ? (float)texture.mipLevels : 0.0f,
					// Enable anisotropic filtering
					MaxAnisotropy = 8,
					AnisotropyEnable = true,
					BorderColor = MgBorderColor.FLOAT_OPAQUE_WHITE,
				};
				err = mDevice.CreateSampler(samplerInfo, mCallbacks, out texture.sampler);
				Debug.Assert(err == Result.SUCCESS);

				// Create image view
				// Textures are not directly accessed by the shaders and
				// are abstracted by image views containing additional
				// information and sub resource ranges
				var viewInfo = new MgImageViewCreateInfo{
					ViewType = MgImageViewType.TYPE_2D,
					Format = format,
					Components = new MgComponentMapping{
						R = MgComponentSwizzle.R,
						G = MgComponentSwizzle.G,
						B = MgComponentSwizzle.B,
						A = MgComponentSwizzle.A
					},
					SubresourceRange = new MgImageSubresourceRange{
						AspectMask =  MgImageAspectFlagBits.COLOR_BIT,
						BaseMipLevel = 0,
						BaseArrayLayer = 0,
						LayerCount = 1,
						// Linear tiling usually won't support mip maps
						// Only set mip map count if optimal tiling is used
						LevelCount = (useStaging) ? texture.mipLevels : 1,
					},
					Image = texture.image,
				};
				err = mDevice.CreateImageView(viewInfo, mCallbacks, out texture.view);
				Debug.Assert(err == Result.SUCCESS);
            }
        }

		public void SetData<T>(int level, int arraySlice, Rectangle? rect, T[] data, int startIndex, int elementCount) 
			where T : struct
        {
//            Threading.BlockOnUIThread(() =>
//            {
//            var elementSizeInByte = Marshal.SizeOf(typeof(T));
//            var dataHandle = GCHandle.Alloc(data, GCHandleType.Pinned);
//            // Use try..finally to make sure dataHandle is freed in case of an error
//            try
//            {
//                var startBytes = startIndex * elementSizeInByte;
//                var dataPtr = (IntPtr)(dataHandle.AddrOfPinnedObject().ToInt64() + startBytes);
//                int x, y, w, h;
//                if (rect.HasValue)
//                {
//                    x = rect.Value.X;
//                    y = rect.Value.Y;
//                    w = rect.Value.Width;
//                    h = rect.Value.Height;
//                }
//                else
//                {
//                    x = 0;
//                    y = 0;
//                    w = Math.Max(width >> level, 1);
//                    h = Math.Max(height >> level, 1);
//
//                    // For DXT textures the width and height of each level is a multiple of 4.
//                    // OpenGL only: The last two mip levels require the width and height to be 
//                    // passed as 2x2 and 1x1, but there needs to be enough data passed to occupy 
//                    // a 4x4 block. 
//                    // Ref: http://www.mentby.com/Group/mac-opengl/issue-with-dxt-mipmapped-textures.html 
//                    if (_format == SurfaceFormat.Dxt1
//                        || _format == SurfaceFormat.Dxt1a
//                        || _format == SurfaceFormat.Dxt3
//                        || _format == SurfaceFormat.Dxt5
//                        || _format == SurfaceFormat.RgbaAtcExplicitAlpha
//                        || _format == SurfaceFormat.RgbaAtcInterpolatedAlpha
//                        || _format == SurfaceFormat.RgbPvrtc2Bpp
//                        || _format == SurfaceFormat.RgbPvrtc4Bpp
//                        || _format == SurfaceFormat.RgbaPvrtc2Bpp
//                        || _format == SurfaceFormat.RgbaPvrtc4Bpp
//                        || _format == SurfaceFormat.RgbEtc1
//                        )
//                    {
//                            if (w > 4)
//                                w = (w + 3) & ~3;
//                            if (h > 4)
//                                h = (h + 3) & ~3;
//                    }
//                }
//
//                    // Store the current bound texture.
//                    var prevTexture = GraphicsExtensions.GetBoundTexture2D();
//
//                    GenerateGLTextureIfRequired();
//
//                    GL.BindTexture(TextureTarget.Texture2D, this.glTexture);
//                    GraphicsExtensions.CheckGLError();
//                    if (glFormat == (GLPixelFormat)All.CompressedTextureFormats)
//                    {
//                        if (rect.HasValue)
//                        {
//                            GL.CompressedTexSubImage2D(TextureTarget.Texture2D, level, x, y, w, h, glFormat, data.Length - startBytes, dataPtr);
//                            GraphicsExtensions.CheckGLError();
//                        }
//                        else
//                        {
//                            GL.CompressedTexImage2D(TextureTarget.Texture2D, level, glInternalFormat, w, h, 0, data.Length - startBytes, dataPtr);
//                            GraphicsExtensions.CheckGLError();
//                        }
//                    }
//                    else
//                    {
//                        // Set pixel alignment to match texel size in bytes
//                        GL.PixelStore(PixelStoreParameter.UnpackAlignment, GraphicsExtensions.GetSize(this.Format));
//                        if (rect.HasValue)
//                        {
//                            GL.TexSubImage2D(TextureTarget.Texture2D, level,
//                                            x, y, w, h,
//                                            glFormat, glType, dataPtr);
//                            GraphicsExtensions.CheckGLError();
//                        }
//                        else
//                        {
//                            GL.TexImage2D(TextureTarget.Texture2D, level,
//                                glInternalFormat,
//                                w, h, 0, glFormat, glType, dataPtr);
//                            GraphicsExtensions.CheckGLError();
//                        }
//                        // Return to default pixel alignment
//                        GL.PixelStore(PixelStoreParameter.UnpackAlignment, 4);
//                    }
//
//#if !ANDROID
//                    GL.Finish();
//                    GraphicsExtensions.CheckGLError();
//#endif
//                    // Restore the bound texture.
//                    GL.BindTexture(TextureTarget.Texture2D, prevTexture);
//                    GraphicsExtensions.CheckGLError();
//            }
//            finally
//            {
//                dataHandle.Free();
//            }
//
//#if !ANDROID
//                // Required to make sure that any texture uploads on a thread are completed
//                // before the main thread tries to use the texture.
//                GL.Finish();
//#endif
//            });
        }

		public void GetData<T>(int level, int arraySlice, Rectangle? rect, T[] data, int startIndex, int elementCount) 
			where T : struct
        {
#if GLES
            // TODO: check for data size and for non renderable formats (formats that can't be attached to FBO)

            var framebufferId = 0;
			#if (IOS || ANDROID)
			GL.GenFramebuffers(1, out framebufferId);
			#else
            GL.GenFramebuffers(1, ref framebufferId);
			#endif
            GraphicsExtensions.CheckGLError();
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, framebufferId);
            GraphicsExtensions.CheckGLError();
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferSlot.ColorAttachment0, TextureTarget.Texture2D, this.glTexture, 0);
            GraphicsExtensions.CheckGLError();
            var x = 0;
            var y = 0;
            var width = this.width;
            var height = this.height;
            if (rect.HasValue)
            {
                x = rect.Value.X;
                y = rect.Value.Y;
                width = this.Width;
                height = this.Height;
            }
            GL.ReadPixels(x, y, width, height, this.glFormat, this.glType, data);
            GraphicsExtensions.CheckGLError();
            GL.DeleteFramebuffers(1, ref framebufferId);
            GraphicsExtensions.CheckGLError();
#else
//            GL.BindTexture(TextureTarget.Texture2D, this.glTexture);
//
//            if (glFormat == (GLPixelFormat)All.CompressedTextureFormats)
//            {
//                throw new NotImplementedException();
//            }
//            else
//            {
//                if (rect.HasValue)
//                {
//                    var temp = new T[this.width * this.height];
//                    GL.GetTexImage(TextureTarget.Texture2D, level, this.glFormat, this.glType, temp);
//                    int z = 0, w = 0;
//
//                    for (int y = rect.Value.Y; y < rect.Value.Y + rect.Value.Height; ++y)
//                    {
//                        for (int x = rect.Value.X; x < rect.Value.X + rect.Value.Width; ++x)
//                        {
//                            data[z * rect.Value.Width + w] = temp[(y * width) + x];
//                            ++w;
//                        }
//                        ++z;
//                        w = 0;
//                    }
//                }
//                else
//                {
//                    GL.GetTexImage(TextureTarget.Texture2D, level, this.glFormat, this.glType, data);
//                }
//            }
#endif
        }

        public Texture2D PlatformFromStream(Stream stream)
        {		

#if IOS || MONOMAC

#if IOS
			using (var uiImage = UIImage.LoadFromData(NSData.FromStream(stream)))
#elif MONOMAC
			using (var nsImage = NSImage.FromStream (stream))
#endif
			{
#if IOS
				var cgImage = uiImage.CGImage;
#elif MONOMAC
				var rectangle = RectangleF.Empty;
				var cgImage = nsImage.AsCGImage (ref rectangle, null, null);
#endif

			    return PlatformFromStream(graphicsDevice, cgImage);
			}
#endif
#if ANDROID
            using (Bitmap image = BitmapFactory.DecodeStream(stream, null, new BitmapFactory.Options
            {
                InScaled = false,
                InDither = false,
                InJustDecodeBounds = false,
                InPurgeable = true,
                InInputShareable = true,
            }))
            {
                return PlatformFromStream(graphicsDevice, image);
            }
#endif
#if DESKTOPGL || ANGLE
            Bitmap image = (Bitmap)Bitmap.FromStream(stream);
            try
            {
                // Fix up the Image to match the expected format
                image = (Bitmap)image.RGBToBGR();

                var data = new byte[image.Width * image.Height * 4];

                BitmapData bitmapData = image.LockBits(new System.Drawing.Rectangle(0, 0, image.Width, image.Height),
                    ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                if (bitmapData.Stride != image.Width * 4) 
                    throw new NotImplementedException();
                Marshal.Copy(bitmapData.Scan0, data, 0, data.Length);
                image.UnlockBits(bitmapData);

                Texture2D texture = null;
                texture = new Texture2D(graphicsDevice, image.Width, image.Height);
                texture.SetData(data);

                return texture;
            }
            finally
            {
                image.Dispose();
            }
#else 
			throw new NotSupportedException();
#endif
        }

#if IOS
        [CLSCompliant(false)]
        public static Texture2D FromStream(GraphicsDevice graphicsDevice, UIImage uiImage)
        {
            return PlatformFromStream(graphicsDevice, uiImage.CGImage);
        }
#elif ANDROID
        [CLSCompliant(false)]
        public static Texture2D FromStream(GraphicsDevice graphicsDevice, Bitmap bitmap)
        {
            return PlatformFromStream(graphicsDevice, bitmap);
        }

        [CLSCompliant(false)]
        public void Reload(Bitmap image)
        {
            var width = image.Width;
            var height = image.Height;

            int[] pixels = new int[width * height];
            if ((width != image.Width) || (height != image.Height))
            {
                using (Bitmap imagePadded = Bitmap.CreateBitmap(width, height, Bitmap.Config.Argb8888))
                {
                    Canvas canvas = new Canvas(imagePadded);
                    canvas.DrawARGB(0, 0, 0, 0);
                    canvas.DrawBitmap(image, 0, 0, null);
                    imagePadded.GetPixels(pixels, 0, width, 0, 0, width, height);
                    imagePadded.Recycle();
                }
            }
            else
            {
                image.GetPixels(pixels, 0, width, 0, 0, width, height);
            }

            image.Recycle();

            this.SetData<int>(pixels);
        }
#endif

#if MONOMAC
        public static Texture2D FromStream(GraphicsDevice graphicsDevice, NSImage nsImage)
        {
            var rectangle = RectangleF.Empty;
		    var cgImage = nsImage.AsCGImage (ref rectangle, null, null);
            return PlatformFromStream(graphicsDevice, cgImage);
        }
#endif

#if IOS || MONOMAC
        private static Texture2D PlatformFromStream(GraphicsDevice graphicsDevice, CGImage cgImage)
        {
			var width = cgImage.Width;
			var height = cgImage.Height;

            var data = new byte[width * height * 4];

            var colorSpace = CGColorSpace.CreateDeviceRGB();
            var bitmapContext = new CGBitmapContext(data, width, height, 8, width * 4, colorSpace, CGBitmapFlags.PremultipliedLast);
            bitmapContext.DrawImage(new RectangleF(0, 0, width, height), cgImage);
            bitmapContext.Dispose();
            colorSpace.Dispose();

            Texture2D texture = null;
            Threading.BlockOnUIThread(() =>
            {
                texture = new Texture2D(graphicsDevice, (int)width, (int)height, false, SurfaceFormat.Color);
                texture.SetData(data);
            });

            return texture;
        }
#elif ANDROID
        private static Texture2D PlatformFromStream(GraphicsDevice graphicsDevice, Bitmap image)
        {
            var width = image.Width;
            var height = image.Height;

            int[] pixels = new int[width * height];
            if ((width != image.Width) || (height != image.Height))
            {
                using (Bitmap imagePadded = Bitmap.CreateBitmap(width, height, Bitmap.Config.Argb8888))
                {
                    Canvas canvas = new Canvas(imagePadded);
                    canvas.DrawARGB(0, 0, 0, 0);
                    canvas.DrawBitmap(image, 0, 0, null);
                    imagePadded.GetPixels(pixels, 0, width, 0, 0, width, height);
                    imagePadded.Recycle();
                }
            }
            else
            {
                image.GetPixels(pixels, 0, width, 0, 0, width, height);
            }
            image.Recycle();

            // Convert from ARGB to ABGR
            ConvertToABGR(height, width, pixels);

            Texture2D texture = null;
            Threading.BlockOnUIThread(() =>
            {
                texture = new Texture2D(graphicsDevice, width, height, false, SurfaceFormat.Color);
                texture.SetData<int>(pixels);
            });

            return texture;
        }
#endif

        private void FillTextureFromStream(Stream stream)
        {
#if ANDROID
            using (Bitmap image = BitmapFactory.DecodeStream(stream, null, new BitmapFactory.Options
            {
                InScaled = false,
                InDither = false,
                InJustDecodeBounds = false,
                InPurgeable = true,
                InInputShareable = true,
            }))
            {
                var width = image.Width;
                var height = image.Height;

                int[] pixels = new int[width * height];
                image.GetPixels(pixels, 0, width, 0, 0, width, height);

                // Convert from ARGB to ABGR
                ConvertToABGR(height, width, pixels);

                this.SetData<int>(pixels);
                image.Recycle();
            }
#endif
        }

        public void SaveAsJpeg(Stream stream, int width, int height)
        {
#if MONOMAC || WINDOWS
			SaveAsImage(stream, width, height, ImageFormat.Jpeg);
#elif ANDROID
            SaveAsImage(stream, width, height, Bitmap.CompressFormat.Jpeg);
#else
            throw new NotImplementedException();
#endif
        }

        public void SaveAsPng(Stream stream, int width, int height)
        {
#if MONOMAC || WINDOWS || IOS
            var pngWriter = new PngWriter();
            pngWriter.Write(this, stream);
#elif ANDROID
            SaveAsImage(stream, width, height, Bitmap.CompressFormat.Png);
#else
            throw new NotImplementedException();
#endif
        }

#if MONOMAC || WINDOWS
		private void SaveAsImage(Stream stream, int width, int height, ImageFormat format)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream", "'stream' cannot be null (Nothing in Visual Basic)");
			}
			if (width <= 0)
			{
				throw new ArgumentOutOfRangeException("width", width, "'width' cannot be less than or equal to zero");
			}
			if (height <= 0)
			{
				throw new ArgumentOutOfRangeException("height", height, "'height' cannot be less than or equal to zero");
			}
			if (format == null)
			{
				throw new ArgumentNullException("format", "'format' cannot be null (Nothing in Visual Basic)");
			}
			
			byte[] data = null;
			GCHandle? handle = null;
			Bitmap bitmap = null;
			try 
			{
				data = new byte[width * height * 4];
				handle = GCHandle.Alloc(data, GCHandleType.Pinned);
				GetData(data);
				
				// internal structure is BGR while bitmap expects RGB
				for(int i = 0; i < data.Length; i += 4)
				{
					byte temp = data[i + 0];
					data[i + 0] = data[i + 2];
					data[i + 2] = temp;
				}
				
				bitmap = new Bitmap(width, height, width * 4, System.Drawing.Imaging.PixelFormat.Format32bppArgb, handle.Value.AddrOfPinnedObject());
				
				bitmap.Save(stream, format);
			} 
			finally 
			{
				if (bitmap != null)
				{
					bitmap.Dispose();
				}
				if (handle.HasValue)
				{
					handle.Value.Free();
				}
				if (data != null)
				{
					data = null;
				}
			}
		}
#elif ANDROID
        private void SaveAsImage(Stream stream, int width, int height, Bitmap.CompressFormat format)
        {
            int[] data = new int[width * height];
            GetData(data);
            // internal structure is BGR while bitmap expects RGB
            for (int i = 0; i < data.Length; ++i)
            {
                uint pixel = (uint)data[i];
                data[i] = (int)((pixel & 0xFF00FF00) | ((pixel & 0x00FF0000) >> 16) | ((pixel & 0x000000FF) << 16));
            }
            using (Bitmap bitmap = Bitmap.CreateBitmap(width, height, Bitmap.Config.Argb8888))
            {
                bitmap.SetPixels(data, 0, width, 0, 0, width, height);
                bitmap.Compress(format, 100, stream);
                bitmap.Recycle();
            }
        }
#endif

        // This method allows games that use Texture2D.FromStream 
        // to reload their textures after the GL context is lost.
        public void Reload(Stream textureStream, MgImage image, MgImageView view)
        {
			//GenerateGLTextureIfRequired(image);
            FillTextureFromStream(textureStream);
        }

		internal class MgTexture2DException : Exception
		{
			public Result ReturnValue { get; private set; }

			public MgTexture2DException (Magnesium.Result result, string message)
				: base(message)
			{
				ReturnValue = result;
			}
		}

//		private void GenerateGLTextureIfRequired(MgImage image)
//        {
//			IMgDevice device;
//			MgAllocationCallbacks callbacks;
//
//			if (image != null)
//            {
//                GL.GenTextures(1, out this.glTexture);
//                GraphicsExtensions.CheckGLError();
//
//                // For best compatibility and to keep the default wrap mode of XNA, only set ClampToEdge if either
//                // dimension is not a power of two.
//                var wrap = TextureWrapMode.Repeat;
//
//				var imgCreateInfo = new MgImageCreateInfo{ };			
//				imgCreateInfo.ArrayLayers = 1U;
//				imgCreateInfo.Tiling = MgImageTiling.OPTIMAL;
//				MgImage output;
//				Result exitCode = device.CreateImage (imgCreateInfo, callbacks, out output);
//				if (exitCode != Result.SUCCESS)
//				{
//					throw new MgTexture2DException (exitCode, "MgTexture2DPlatform : MgImage initialisation error");
//				}
//
//
//				MgImageView outputView;
//				var viewCreateInfo = new MgImageViewCreateInfo { };
//				viewCreateInfo.Image = output;
//				viewCreateInfo.ViewType = MgImageViewType.TYPE_2D;
//
//				exitCode = device.CreateImageView (viewCreateInfo, callbacks, out outputView);
//
//				if (exitCode != Result.SUCCESS)
//				{
//					throw new MgTexture2DException (exitCode, "MgTexture2DPlatform : MgImageView initialisation error");
//				}
//
//                if (((width & (width - 1)) != 0) || ((height & (height - 1)) != 0))
//                    wrap = TextureWrapMode.ClampToEdge;
//
//                GL.BindTexture(TextureTarget.Texture2D, this.glTexture);
//                GraphicsExtensions.CheckGLError();
//                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter,
//                                (_levelCount > 1) ? (int)TextureMinFilter.LinearMipmapLinear : (int)TextureMinFilter.Linear);
//                GraphicsExtensions.CheckGLError();
//                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter,
//                                (int)TextureMagFilter.Linear);
//                GraphicsExtensions.CheckGLError();
//                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)wrap);
//                GraphicsExtensions.CheckGLError();
//                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)wrap);
//                GraphicsExtensions.CheckGLError();
//            }
//        }
    }
}

