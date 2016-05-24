using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;
using System.IO;
using System.Collections.Concurrent;

namespace Magnesium.OpenGL
{
	public class GLDevice : IMgDevice
	{
		#region IMgDevice implementation
		public PFN_vkVoidFunction GetDeviceProcAddr (string pName)
		{
			throw new NotImplementedException ();
		}
		public void DestroyDevice (MgAllocationCallbacks allocator)
		{
			throw new NotImplementedException ();
		}
		public void GetDeviceQueue (uint queueFamilyIndex, uint queueIndex, out IMgQueue pQueue)
		{
			throw new NotImplementedException ();
		}
		public Result DeviceWaitIdle ()
		{
			throw new NotImplementedException ();
		}
		public Result AllocateMemory (MgMemoryAllocateInfo pAllocateInfo, MgAllocationCallbacks allocator, out MgDeviceMemory pMemory)
		{
			throw new NotImplementedException ();
		}
		public void FreeMemory (MgDeviceMemory memory, MgAllocationCallbacks allocator)
		{
			throw new NotImplementedException ();
		}
		public Result MapMemory (MgDeviceMemory memory, ulong offset, ulong size, uint flags, out IntPtr ppData)
		{
			throw new NotImplementedException ();
		}
		public void UnmapMemory (MgDeviceMemory memory)
		{
			throw new NotImplementedException ();
		}
		public Result FlushMappedMemoryRanges (MgMappedMemoryRange[] pMemoryRanges)
		{
			throw new NotImplementedException ();
		}
		public Result InvalidateMappedMemoryRanges (MgMappedMemoryRange[] pMemoryRanges)
		{
			throw new NotImplementedException ();
		}
		public void GetDeviceMemoryCommitment (MgDeviceMemory memory, ref ulong pCommittedMemoryInBytes)
		{
			throw new NotImplementedException ();
		}
		public void GetBufferMemoryRequirements (MgBuffer buffer, out MgMemoryRequirements pMemoryRequirements)
		{
			throw new NotImplementedException ();
		}
		public Result BindBufferMemory (MgBuffer buffer, MgDeviceMemory memory, ulong memoryOffset)
		{
			throw new NotImplementedException ();
		}
		public void GetImageMemoryRequirements (IMgImage image, out MgMemoryRequirements memoryRequirements)
		{
			throw new NotImplementedException ();
		}
		public Result BindImageMemory (IMgImage image, MgDeviceMemory memory, ulong memoryOffset)
		{
			throw new NotImplementedException ();
		}
		public void GetImageSparseMemoryRequirements (IMgImage image, out MgSparseImageMemoryRequirements[] sparseMemoryRequirements)
		{
			throw new NotImplementedException ();
		}
		public Result CreateFence (MgFenceCreateInfo pCreateInfo, MgAllocationCallbacks allocator, out MgFence fence)
		{
			throw new NotImplementedException ();
		}
		public void DestroyFence (MgFence fence, MgAllocationCallbacks allocator)
		{
			throw new NotImplementedException ();
		}
		public Result ResetFences (MgFence[] pFences)
		{
			throw new NotImplementedException ();
		}
		public Result GetFenceStatus (MgFence fence)
		{
			throw new NotImplementedException ();
		}
		public Result WaitForFences (MgFence[] pFences, bool waitAll, ulong timeout)
		{
			throw new NotImplementedException ();
		}
		public Result CreateSemaphore (MgSemaphoreCreateInfo pCreateInfo, MgAllocationCallbacks allocator, out MgSemaphore pSemaphore)
		{
			throw new NotImplementedException ();
		}
		public void DestroySemaphore (MgSemaphore semaphore, MgAllocationCallbacks allocator)
		{
			throw new NotImplementedException ();
		}
		public Result CreateEvent (MgEventCreateInfo pCreateInfo, MgAllocationCallbacks allocator, out MgEvent @event)
		{
			throw new NotImplementedException ();
		}
		public void DestroyEvent (MgEvent @event, MgAllocationCallbacks allocator)
		{
			throw new NotImplementedException ();
		}
		public Result GetEventStatus (MgEvent @event)
		{
			throw new NotImplementedException ();
		}
		public Result SetEvent (MgEvent @event)
		{
			throw new NotImplementedException ();
		}
		public Result ResetEvent (MgEvent @event)
		{
			throw new NotImplementedException ();
		}
		public Result CreateQueryPool (MgQueryPoolCreateInfo pCreateInfo, MgAllocationCallbacks allocator, out MgQueryPool queryPool)
		{
			throw new NotImplementedException ();
		}
		public void DestroyQueryPool (MgQueryPool queryPool, MgAllocationCallbacks allocator)
		{
			throw new NotImplementedException ();
		}
		public Result GetQueryPoolResults (MgQueryPool queryPool, uint firstQuery, uint queryCount, IntPtr dataSize, IntPtr pData, ulong stride, MgQueryResultFlagBits flags)
		{
			throw new NotImplementedException ();
		}
		public Result CreateBuffer (MgBufferCreateInfo pCreateInfo, MgAllocationCallbacks allocator, out MgBuffer pBuffer)
		{
			throw new NotImplementedException ();
		}
		public void DestroyBuffer (MgBuffer buffer, MgAllocationCallbacks allocator)
		{
			throw new NotImplementedException ();
		}
		public Result CreateBufferView (MgBufferViewCreateInfo pCreateInfo, MgAllocationCallbacks allocator, out MgBufferView pView)
		{
			throw new NotImplementedException ();
		}
		public void DestroyBufferView (MgBufferView bufferView, MgAllocationCallbacks allocator)
		{
			throw new NotImplementedException ();
		}

		internal static void GetGLFormat (MgFormat format,
			bool supportsSRgb,
			out PixelInternalFormat glInternalFormat,
			out PixelFormat glFormat,
			out PixelType glType)
		{
			glInternalFormat = PixelInternalFormat.Rgba;
			glFormat = PixelFormat.Rgba;
			glType = PixelType.UnsignedByte;

			switch (format) {
			case MgFormat.R8G8B8A8_UINT:
			//case SurfaceFormat.Color:
				glInternalFormat = PixelInternalFormat.Rgba;
				glFormat = PixelFormat.Rgba;
				glType = PixelType.UnsignedByte;
				break;
			case MgFormat.R8G8B8A8_SRGB:
			//case SurfaceFormat.ColorSRgb:
				if (!supportsSRgb)
					//goto case SurfaceFormat.Color;
					goto case MgFormat.R8G8B8A8_UINT;
				glInternalFormat = (PixelInternalFormat) 0x8C40; // PixelInternalFormat.Srgb;
				glFormat = PixelFormat.Rgba;
				glType = PixelType.UnsignedByte;
				break;
			case MgFormat.B5G6R5_UNORM_PACK16:
			//case SurfaceFormat.Bgr565:
				glInternalFormat = PixelInternalFormat.Rgb;
				glFormat = PixelFormat.Rgb;
				glType = PixelType.UnsignedShort565;
				break;
			case MgFormat.B4G4R4A4_UNORM_PACK16:
			//case SurfaceFormat.Bgra4444:
				#if IOS || ANDROID
				glInternalFormat = PixelInternalFormat.Rgba;
				#else
				glInternalFormat = PixelInternalFormat.Rgba4;
				#endif
				glFormat = PixelFormat.Rgba;
				glType = PixelType.UnsignedShort4444;
				break;
			case MgFormat.B5G5R5A1_UNORM_PACK16:
			//case SurfaceFormat.Bgra5551:
				glInternalFormat = PixelInternalFormat.Rgba;
				glFormat = PixelFormat.Rgba;
				glType = PixelType.UnsignedShort5551;
				break;
			case MgFormat.R8_UINT:
			//case SurfaceFormat.Alpha8:
				glInternalFormat = PixelInternalFormat.Luminance;
				glFormat = PixelFormat.Luminance;
				glType = PixelType.UnsignedByte;
				break;
			#if !IOS && !ANDROID && !ANGLE
			case MgFormat.BC1_RGB_UNORM_BLOCK:
			//case SurfaceFormat.Dxt1:
				glInternalFormat = PixelInternalFormat.CompressedRgbS3tcDxt1Ext;
				glFormat = (PixelFormat)All.CompressedTextureFormats;
				break;
			case MgFormat.BC1_RGB_SRGB_BLOCK:
			// case SurfaceFormat.Dxt1SRgb:
				if (!supportsSRgb)
					//goto case SurfaceFormat.Dxt1;
					goto case MgFormat.BC1_RGB_SRGB_BLOCK;
				glInternalFormat = PixelInternalFormat.CompressedSrgbS3tcDxt1Ext;
				glFormat = (PixelFormat) All.CompressedTextureFormats;
				break;
			case MgFormat.BC1_RGBA_UNORM_BLOCK:
			//case SurfaceFormat.Dxt1a:
				glInternalFormat = PixelInternalFormat.CompressedRgbaS3tcDxt1Ext;
				glFormat = (PixelFormat)All.CompressedTextureFormats;
				break;
			case MgFormat.BC2_UNORM_BLOCK:
			//case SurfaceFormat.Dxt3:
				glInternalFormat = PixelInternalFormat.CompressedRgbaS3tcDxt3Ext;
				glFormat = (PixelFormat)All.CompressedTextureFormats;
				break;
			case MgFormat.BC2_SRGB_BLOCK:
			//case SurfaceFormat.Dxt3SRgb:
				if (!supportsSRgb)
					goto case MgFormat.BC2_UNORM_BLOCK;
				//	goto case SurfaceFormat.Dxt3;
				glInternalFormat = PixelInternalFormat.CompressedSrgbAlphaS3tcDxt3Ext;
				glFormat = (PixelFormat) All.CompressedTextureFormats;
				break;
			case MgFormat.BC3_UNORM_BLOCK:
			//case SurfaceFormat.Dxt5:
				glInternalFormat = PixelInternalFormat.CompressedRgbaS3tcDxt5Ext;
				glFormat = (PixelFormat)All.CompressedTextureFormats;
				break;
			case MgFormat.BC3_SRGB_BLOCK:
			//case SurfaceFormat.Dxt5SRgb:
				if (!supportsSRgb)
					goto case MgFormat.BC3_UNORM_BLOCK;
					//goto case SurfaceFormat.Dxt5;
				glInternalFormat = PixelInternalFormat.CompressedSrgbAlphaS3tcDxt5Ext;
				glFormat = (PixelFormat) All.CompressedTextureFormats;
				break;
			case MgFormat.R32_SFLOAT:
			// case SurfaceFormat.Single:
				glInternalFormat = PixelInternalFormat.R32f;
				glFormat = PixelFormat.Red;
				glType = PixelType.Float;
				break;
			case MgFormat.R16G16_SFLOAT:
			// case SurfaceFormat.HalfVector2:
				glInternalFormat = PixelInternalFormat.Rg16f;
				glFormat = PixelFormat.Rg;
				glType = PixelType.HalfFloat;
				break;

			// HdrBlendable implemented as HalfVector4 (see http://blogs.msdn.com/b/shawnhar/archive/2010/07/09/surfaceformat-hdrblendable.aspx)			
			case MgFormat.R16G16B16A16_SFLOAT:
			//case SurfaceFormat.HdrBlendable:
			//case SurfaceFormat.HalfVector4:
				glInternalFormat = PixelInternalFormat.Rgba16f;
				glFormat = PixelFormat.Rgba;
				glType = PixelType.HalfFloat;
				break;
			case MgFormat.R16_SFLOAT:
			//case SurfaceFormat.HalfSingle:
				glInternalFormat = PixelInternalFormat.R16f;
				glFormat = PixelFormat.Red;
				glType = PixelType.HalfFloat;
				break;
			case MgFormat.R32G32_SFLOAT:
			//case SurfaceFormat.Vector2:
				glInternalFormat = PixelInternalFormat.Rg32f;
				glFormat = PixelFormat.Rg;
				glType = PixelType.Float;
				break;
			case MgFormat.R32G32B32A32_SFLOAT:
			//case SurfaceFormat.Vector4:
				glInternalFormat = PixelInternalFormat.Rgba32f;
				glFormat = PixelFormat.Rgba;
				glType = PixelType.Float;
				break;
			case MgFormat.R8G8_SNORM:
			//case SurfaceFormat.NormalizedByte2:
				glInternalFormat = PixelInternalFormat.Rg8i;
				glFormat = PixelFormat.Rg;
				glType = PixelType.Byte;
				break;
			case MgFormat.R8G8B8A8_SNORM:
			//case SurfaceFormat.NormalizedByte4:
				glInternalFormat = PixelInternalFormat.Rgba8i;
				glFormat = PixelFormat.Rgba;
				glType = PixelType.Byte;
				break;
			case MgFormat.R16G16_UINT:
			//case SurfaceFormat.Rg32:
				glInternalFormat = PixelInternalFormat.Rg16ui;
				glFormat = PixelFormat.Rg;
				glType = PixelType.UnsignedShort;
				break;
			case MgFormat.R16G16B16A16_UINT:
			//case SurfaceFormat.Rgba64:
				glInternalFormat = PixelInternalFormat.Rgba16ui;
				glFormat = PixelFormat.Rgba;
				glType = PixelType.UnsignedShort;
				break;
			case MgFormat.A2B10G10R10_UINT_PACK32:
			//case SurfaceFormat.Rgba1010102:
				glInternalFormat = PixelInternalFormat.Rgb10A2ui;
				glFormat = PixelFormat.Rgba;
				glType = PixelType.UnsignedInt1010102;
				break;
				#endif

				#if ANDROID
				case SurfaceFormat.Dxt1:
				// 0x83F0 is the RGB version, 0x83F1 is the RGBA version (1-bit alpha)
				// XNA uses the RGB version.
				glInternalFormat = (PixelInternalFormat)0x83F0; 
				glFormat = (PixelFormat)All.CompressedTextureFormats;
				break;
				case SurfaceFormat.Dxt1SRgb:
				if (!supportsSRgb)
				goto case SurfaceFormat.Dxt1;
				glInternalFormat = (PixelInternalFormat)0x8C4C;
				glFormat = (PixelFormat)All.CompressedTextureFormats;
				break;
				case SurfaceFormat.Dxt1a:
				// 0x83F0 is the RGB version, 0x83F1 is the RGBA version (1-bit alpha)
				glInternalFormat = (PixelInternalFormat)0x83F1;
				glFormat = (PixelFormat)All.CompressedTextureFormats;
				break;
				case SurfaceFormat.Dxt3:
				glInternalFormat = (PixelInternalFormat)0x83F2;
				glFormat = (PixelFormat)All.CompressedTextureFormats;
				break;
				case SurfaceFormat.Dxt3SRgb:
				if (!supportsSRgb)
				goto case SurfaceFormat.Dxt3;
				glInternalFormat = (PixelInternalFormat)0x8C4E;
				glFormat = (PixelFormat)All.CompressedTextureFormats;
				break;
				case SurfaceFormat.Dxt5:
				glInternalFormat = (PixelInternalFormat)0x83F3;
				glFormat = (PixelFormat)All.CompressedTextureFormats;
				break;
				case SurfaceFormat.Dxt5SRgb:
				if (!supportsSRgb)
				goto case SurfaceFormat.Dxt5;
				glInternalFormat = (PixelInternalFormat)0x8C4F;
				glFormat = (PixelFormat)All.CompressedTextureFormats;
				break;
				case SurfaceFormat.RgbaAtcExplicitAlpha:
				glInternalFormat = (PixelInternalFormat)All.AtcRgbaExplicitAlphaAmd;
				glFormat = (PixelFormat)All.CompressedTextureFormats;
				break;
				case SurfaceFormat.RgbaAtcInterpolatedAlpha:
				glInternalFormat = (PixelInternalFormat)All.AtcRgbaInterpolatedAlphaAmd;
				glFormat = (PixelFormat)All.CompressedTextureFormats;
				break;
				case SurfaceFormat.RgbEtc1:
				glInternalFormat = (PixelInternalFormat)0x8D64; // GL_ETC1_RGB8_OES
				glFormat = (PixelFormat)All.CompressedTextureFormats;
				break;
				#endif
				#if IOS || ANDROID
				case SurfaceFormat.RgbPvrtc2Bpp:
				glInternalFormat = (PixelInternalFormat)All.CompressedRgbPvrtc2Bppv1Img;
				glFormat = (PixelFormat)All.CompressedTextureFormats;
				break;
				case SurfaceFormat.RgbPvrtc4Bpp:
				glInternalFormat = (PixelInternalFormat)All.CompressedRgbPvrtc4Bppv1Img;
				glFormat = (PixelFormat)All.CompressedTextureFormats;
				break;
				case SurfaceFormat.RgbaPvrtc2Bpp:
				glInternalFormat = (PixelInternalFormat)All.CompressedRgbaPvrtc2Bppv1Img;
				glFormat = (PixelFormat)All.CompressedTextureFormats;
				break;
				case SurfaceFormat.RgbaPvrtc4Bpp:
				glInternalFormat = (PixelInternalFormat)All.CompressedRgbaPvrtc4Bppv1Img;
				glFormat = (PixelFormat)All.CompressedTextureFormats;
				break;
				#endif
			default:
				throw new NotSupportedException();
			}
		}

		private static int GetSize(MgFormat surfaceFormat)
		{
			switch (surfaceFormat)
			{
			case MgFormat.BC1_RGB_UNORM_BLOCK:
			//case SurfaceFormat.Dxt1:
			case MgFormat.BC1_RGB_SRGB_BLOCK:
			//case SurfaceFormat.Dxt1SRgb:
			case MgFormat.BC1_RGBA_UNORM_BLOCK:
			//case SurfaceFormat.Dxt1a:
			//case SurfaceFormat.RgbPvrtc2Bpp:
			//case SurfaceFormat.RgbaPvrtc2Bpp:			
			//case SurfaceFormat.RgbEtc1:
				// One texel in DXT1, PVRTC 2bpp and ETC1 is a minimum 4x4 block, which is 8 bytes
				return 8;
			case MgFormat.BC2_UNORM_BLOCK:
			//case SurfaceFormat.Dxt3:
			case MgFormat.BC2_SRGB_BLOCK:
			//case SurfaceFormat.Dxt3SRgb:
			case MgFormat.BC3_UNORM_BLOCK:
			//case SurfaceFormat.Dxt5:
			case MgFormat.BC3_SRGB_BLOCK:
			//case SurfaceFormat.Dxt5SRgb:
			//case SurfaceFormat.RgbPvrtc4Bpp:
			//case SurfaceFormat.RgbaPvrtc4Bpp:
			//case SurfaceFormat.RgbaAtcExplicitAlpha:
			//case SurfaceFormat.RgbaAtcInterpolatedAlpha:
				// One texel in DXT3, DXT5 and PVRTC 4bpp is a minimum 4x4 block, which is 16 bytes
				return 16;
			case MgFormat.R8_UNORM:
			//case SurfaceFormat.Alpha8:
				return 1;
			case MgFormat.B5G6R5_UNORM_PACK16:
			//case SurfaceFormat.Bgr565:
			case MgFormat.B4G4R4A4_UNORM_PACK16:
			//case SurfaceFormat.Bgra4444:
			case MgFormat.B5G5R5A1_UNORM_PACK16:
			//case SurfaceFormat.Bgra5551:
			case MgFormat.R16_SFLOAT:
			//case SurfaceFormat.HalfSingle:
			case MgFormat.R16_UNORM:
			//case SurfaceFormat.NormalizedByte2:
				return 2;
			case MgFormat.R8G8B8A8_UINT:
				//case SurfaceFormat.Color:
			case MgFormat.R8G8B8A8_SRGB:
				//case SurfaceFormat.ColorSRgb:
			case MgFormat.R32_SFLOAT:
				//case SurfaceFormat.Single:
			case MgFormat.R16G16_UINT:
				//case SurfaceFormat.Rg32:
			case MgFormat.R16G16_SFLOAT:
				//case SurfaceFormat.HalfVector2:
			case MgFormat.R8G8B8A8_SNORM:
				//case SurfaceFormat.NormalizedByte4:
			case MgFormat.A2B10G10R10_UINT_PACK32:
				//case SurfaceFormat.Rgba1010102:
				//case SurfaceFormat.Bgra32:
				//case SurfaceFormat.Bgra32SRgb:
				//case SurfaceFormat.Bgr32:
				//case SurfaceFormat.Bgr32SRgb:
				return 4;
			case MgFormat.R16G16B16A16_SFLOAT:				
				//case SurfaceFormat.HalfVector4:
				//case SurfaceFormat.Rgba64:
			case MgFormat.R32G32_SFLOAT:
				//case SurfaceFormat.Vector2:
				return 8;
			case MgFormat.R32G32B32A32_SFLOAT:				
				//case SurfaceFormat.Vector4:
				return 16;
			default:
				throw new ArgumentException();
			}
		}

		public Result CreateImage (MgImageCreateInfo pCreateInfo, MgAllocationCallbacks allocator, out IMgImage pImage)
		{
			var image = new GLImage((int) pCreateInfo.Extent.Width, (int) pCreateInfo.Extent.Height);
			pImage = image;
			return Result.SUCCESS;
		}

//		public void DestroyImage (MgImage image, MgAllocationCallbacks allocator)
//		{
//			mImages [image.Key].Destroy ();
//		}

		public void GetImageSubresourceLayout (IMgImage image, MgImageSubresource pSubresource, out MgSubresourceLayout pLayout)
		{
			throw new NotImplementedException ();
		}

		public Result CreateImageView (MgImageViewCreateInfo pCreateInfo, MgAllocationCallbacks allocator, out IMgImageView pView)
		{
			if (pCreateInfo.Image == null)
			{
				throw new ArgumentNullException ("pCreateInfo.Image is null");
			}

			PixelInternalFormat glInternalFormat;
			PixelFormat glFormat;
			PixelType glType;
			GetGLFormat(pCreateInfo.Format, true, out glInternalFormat, out glFormat, out glType);

			var image = pCreateInfo.Image as GLImage;

			if (pCreateInfo.ViewType == MgImageViewType.TYPE_2D)
			{
				var prevTexture = 0;
				GL.GetInteger(GetPName.TextureBinding2D, out prevTexture);

				int textureId;
				GL.GenTextures(1, out textureId);
				GL.BindTexture (TextureTarget.Texture2D, textureId);

				if (glFormat == (PixelFormat)All.CompressedTextureFormats)
				{
					var imageSize = 0;
					switch (pCreateInfo.Format)
					{
					// FIXME : 
					//				//case SurfaceFormat.RgbPvrtc2Bpp:
					//				case SurfaceFormat.RgbaPvrtc2Bpp:
					//					imageSize = (Math.Max(this.width, 16) * Math.Max(this.height, 8) * 2 + 7) / 8;
					//					break;
					//				case SurfaceFormat.RgbPvrtc4Bpp:
					//				case SurfaceFormat.RgbaPvrtc4Bpp:
					//					imageSize = (Math.Max(this.width, 8) * Math.Max(this.height, 8) * 4 + 7) / 8;
					//					break;
					case MgFormat.BC1_RGB_UNORM_BLOCK:
					//case SurfaceFormat.Dxt1:
					case MgFormat.BC1_RGBA_UNORM_BLOCK:
					//case SurfaceFormat.Dxt1a:
					case MgFormat.BC1_RGB_SRGB_BLOCK:
					//case SurfaceFormat.Dxt1SRgb:
					case MgFormat.BC2_UNORM_BLOCK:
					//case SurfaceFormat.Dxt3:
					case MgFormat.BC2_SRGB_BLOCK:				
					//case SurfaceFormat.Dxt3SRgb:
					case MgFormat.BC3_UNORM_BLOCK:
					//case SurfaceFormat.Dxt5:
					case MgFormat.BC3_SRGB_BLOCK:
					//case SurfaceFormat.Dxt5SRgb:
					//case SurfaceFormat.RgbEtc1:
					//case SurfaceFormat.RgbaAtcExplicitAlpha:
					//case SurfaceFormat.RgbaAtcInterpolatedAlpha:
						imageSize = ((image.Width + 3) / 4) * ((image.Height + 3) / 4) * GetSize (pCreateInfo.Format);
						break;
					default:
						throw new NotSupportedException ();
						//return Result.ERROR_FEATURE_NOT_PRESENT;
					}

					GL.CompressedTexImage2D (TextureTarget.Texture2D, 0, glInternalFormat,
						image.Width, image.Height, 0,
						imageSize, IntPtr.Zero);
					//GraphicsExtensions.CheckGLError();
				} else
				{
					GL.TexImage2D (TextureTarget.Texture2D, 0, glInternalFormat,
						image.Width, image.Height, 0,
						glFormat, glType, IntPtr.Zero);
					//GraphicsExtensions.CheckGLError();
				}

				// Restore the bound texture.
				GL.BindTexture (TextureTarget.Texture2D, prevTexture);

				var view = new GLImageView (textureId); 
				pView = view;

				return Result.SUCCESS;
			}
			else
			{
				throw new NotSupportedException ();
			}
		}

//		public void DestroyImageView (IMgImageView imageView, MgAllocationCallbacks allocator)
//		{
//			mImageViews [imageView.Key].Destroy ();
//		}

		class GLShaderModule
		{
			public MgShaderModule Handle { get; set; }
			public int? ShaderId { get; set; }
			public MgShaderModuleCreateInfo Info { get; set; }

			public void Destroy()
			{
				if (ShaderId.HasValue)
				{
					GL.DeleteShader(ShaderId.Value);
					ShaderId = null;
				}
			}
		}
		private List<GLShaderModule> mShaderModules = new List<GLShaderModule>();
		public Result CreateShaderModule (MgShaderModuleCreateInfo pCreateInfo, MgAllocationCallbacks allocator, out MgShaderModule pShaderModule)
		{
			var result = new GLShaderModule { 
				Handle = new MgShaderModule{ Key = mShaderModules.Count },
				Info = pCreateInfo,
				ShaderId = null,
			};

			mShaderModules.Add (result);
			pShaderModule = result.Handle;
			return Result.SUCCESS;
		}

		public void DestroyShaderModule (MgShaderModule shaderModule, MgAllocationCallbacks allocator)
		{
			mShaderModules[shaderModule.Key].Destroy();
		}

		public Result CreatePipelineCache (MgPipelineCacheCreateInfo pCreateInfo, MgAllocationCallbacks allocator, out IMgPipelineCache pPipelineCache)
		{
			throw new NotImplementedException ();
		}
//		public void DestroyPipelineCache (IMgPipelineCache pipelineCache, MgAllocationCallbacks allocator)
//		{
//			throw new NotImplementedException ();
//		}
		public Result GetPipelineCacheData (IMgPipelineCache pipelineCache, UIntPtr pDataSize, IntPtr pData)
		{
			throw new NotImplementedException ();
		}
		public Result MergePipelineCaches (IMgPipelineCache dstCache, IMgPipelineCache[] pSrcCaches)
		{
			throw new NotImplementedException ();
		}
		//private List<GLGraphicsPipeline> mPipelines = new List<GLGraphicsPipeline> ();

		int CompileShaderModules (MgGraphicsPipelineCreateInfo info)
		{
			var modules = new List<int> ();
			foreach (var stage in info.Stages)
			{
				var shaderType = ShaderType.VertexShader;
				if (stage.Stage == MgShaderStageFlagBits.FRAGMENT_BIT)
				{
					shaderType = ShaderType.FragmentShader;
				}
				else if (stage.Stage == MgShaderStageFlagBits.VERTEX_BIT)
				{
					shaderType = ShaderType.VertexShader;
				}
				else if (stage.Stage == MgShaderStageFlagBits.COMPUTE_BIT)
				{
					shaderType = ShaderType.ComputeShader;
				}
				else if (stage.Stage == MgShaderStageFlagBits.GEOMETRY_BIT)
				{
					shaderType = ShaderType.GeometryShader;
				}
				var module = mShaderModules [stage.Module.Key];
				if (module.ShaderId.HasValue)
				{
					modules.Add (module.ShaderId.Value);
				}
				else
				{
					using (var ms = new MemoryStream ())
					{
						module.Info.Code.CopyTo (ms, (int)module.Info.CodeSize.ToUInt32 ());
						ms.Seek (0, SeekOrigin.Begin);
						// FIXME : Encoding type 
						using (var sr = new StreamReader (ms))
						{
							string fileContents = sr.ReadToEnd ();
							module.ShaderId = GLSLTextShader.CompileShader (shaderType, fileContents, string.Empty);
							modules.Add (module.ShaderId.Value);
						}
					}
				}
			}
			return GLSLTextShader.LinkShaders (modules.ToArray ());
		}

		public Result CreateGraphicsPipelines (IMgPipelineCache pipelineCache, MgGraphicsPipelineCreateInfo[] pCreateInfos, MgAllocationCallbacks allocator, out IMgPipeline[] pPipelines)
		{
			var output = new List<IMgPipeline> ();

			foreach (var info in pCreateInfos)
			{
				var layout = info.Layout as GLPipelineLayout;
				if (layout == null)
				{
					throw new ArgumentException ("pCreateInfos[].Layout");
				}

				if (info.VertexInputState == null)
				{
					throw new ArgumentNullException ("pCreateInfos[].VertexInputState");
				}

				if (info.InputAssemblyState == null)
				{
					throw new ArgumentNullException ("pCreateInfos[].InputAssemblyState");
				}

				if (info.RasterizationState == null)
				{
					throw new ArgumentNullException ("pCreateInfos[].RasterizationState");
				}

				var programId = CompileShaderModules (info);
				var pipeline = new GLGraphicsPipeline (programId, info.RasterizationState);

				var uniqueLocations = new SortedDictionary<int, GLVariableBind> ();
				foreach (var binding in layout.Bindings)
				{
					int locationQuery;
					GL.Ext.GetUniform (programId, binding.Location, out locationQuery);
					// ONLY ACTIVE UNIFORMS
					// FIXME : input attachment
					var bind = new GLVariableBind{
						IsActive = (locationQuery != -1), 
						Location = binding.Location,
						DescriptorType = binding.DescriptorType };

					// WILL THROW ERROR HERE IF COLLISION
					uniqueLocations.Add(binding.Location, bind);
				}

				// ASSUME NO GAPS ARE SUPPLIED
				pipeline.UniformBinder = new GLProgramUniformBinder (uniqueLocations.Values.Count);
				foreach (var bind in uniqueLocations.Values)
				{
					pipeline.UniformBinder.Bindings[bind.Location] = bind;
				}

				output.Add (pipeline);
			}
			pPipelines = output.ToArray ();
			return Result.SUCCESS;
		}
		public Result CreateComputePipelines (IMgPipelineCache pipelineCache, MgComputePipelineCreateInfo[] pCreateInfos, MgAllocationCallbacks allocator, out IMgPipeline[] pPipelines)
		{
			throw new NotImplementedException ();
		}
//		public void DestroyPipeline (IMgPipeline pipeline, MgAllocationCallbacks allocator)
//		{
//			mPipelines [pipeline.Key].Destroy ();	
//		}

		//private List<GLPipelineLayout> mPipelineLayouts = new List<GLPipelineLayout> ();
		public Result CreatePipelineLayout (MgPipelineLayoutCreateInfo pCreateInfo, MgAllocationCallbacks allocator, out IMgPipelineLayout pPipelineLayout)
		{
			if (pCreateInfo != null)
			{
				throw new ArgumentNullException ("pCreateInfo");
			}

			if (pCreateInfo.SetLayouts == null)
			{
				throw new ArgumentNullException ("pCreateInfo.SetLayouts");
			}

			if (pCreateInfo.SetLayouts.Length > 1)
			{
				throw new NotSupportedException ("DESKTOPGL - SetLayouts must be <= 1");
			}

			pPipelineLayout = new GLPipelineLayout (pCreateInfo);
			return Result.SUCCESS;
		}
//		public void DestroyPipelineLayout (IMgPipelineLayout pipelineLayout, MgAllocationCallbacks allocator)
//		{
//			mPipelineLayouts [pipelineLayout.Key].Destroy ();
//		}

		public Result CreateSampler (MgSamplerCreateInfo pCreateInfo, MgAllocationCallbacks allocator, out IMgSampler pSampler)
		{
			var sampler = new GLSampler (GL.GenSampler (), pCreateInfo);
			pSampler = sampler;
			return Result.SUCCESS;
		}
//		public void DestroySampler (IMgSampler sampler, MgAllocationCallbacks allocator)
//		{
//			mSamplers [sampler.Key].Destroy ();	
//		}

		//private List<GLDescriptorSetLayout> mDescriptorSetLayouts = new List<GLDescriptorSetLayout> ();
		public Result CreateDescriptorSetLayout (MgDescriptorSetLayoutCreateInfo pCreateInfo, MgAllocationCallbacks allocator, out IMgDescriptorSetLayout pSetLayout)
		{
			if (pCreateInfo == null)
			{
				throw new ArgumentNullException ("pCreateInfo");
			}
			pSetLayout  = new GLDescriptorSetLayout (pCreateInfo); 
			return Result.SUCCESS;
		}
//		public void DestroyDescriptorSetLayout (IMgDescriptorSetLayout descriptorSetLayout, MgAllocationCallbacks allocator)
//		{
//			mDescriptorSetLayouts [descriptorSetLayout.Key].Destroy ();
//		}

		public Result CreateDescriptorPool (MgDescriptorPoolCreateInfo pCreateInfo, MgAllocationCallbacks allocator, out IMgDescriptorPool pDescriptorPool)
		{
			var pool = new GLDescriptorPool ((int) pCreateInfo.MaxSets);
			pDescriptorPool = pool;
			return Result.SUCCESS;
		}

//		public void DestroyDescriptorPool (IMgDescriptorPool descriptorPool, MgAllocationCallbacks allocator)
//		{
//			mPools [descriptorPool.Key].Destroy ();
//		}

		public Result ResetDescriptorPool (IMgDescriptorPool descriptorPool, uint flags)
		{
			throw new NotImplementedException ();
		}

		//private ConcurrentDictionary<int, GLDescriptorSet> mDescriptorSets = new ConcurrentDictionary<int, GLDescriptorSet>();
		public Result AllocateDescriptorSets (MgDescriptorSetAllocateInfo pAllocateInfo, out MgDescriptorSet[] pDescriptorSets)
		{
			if (pAllocateInfo == null)
			{	
				throw new ArgumentNullException ("pAllocateInfo");
			}

			var pool = pAllocateInfo.DescriptorPool as GLDescriptorPool;
			if (pool == null)
			{
				throw new ArgumentNullException ("pAllocateInfo.DescriptorPool");
			}

			var noOfSetsRequested = pAllocateInfo.SetLayouts.Length;
			if (pool.Sets.Count < noOfSetsRequested)
			{
				throw new InvalidOperationException ();
			}

			pDescriptorSets = new GLDescriptorSet[noOfSetsRequested];

			for (int i = 0; i < noOfSetsRequested; ++i)
			{
				var setLayout = pAllocateInfo.SetLayouts[i] as GLDescriptorSetLayout;

				GLDescriptorSet dSet;
				if (!pool.Sets.TryTake (out dSet))
				{
					throw new InvalidOperationException ();
				}
				// copy here
				dSet.Populate (setLayout);
				pDescriptorSets[i] = dSet;
			}

			return Result.SUCCESS;
		}

		public Result FreeDescriptorSets (IMgDescriptorPool descriptorPool, MgDescriptorSet[] pDescriptorSets)
		{
			if (descriptorPool == null)
			{	
				throw new ArgumentNullException ("descriptorPool");
			}

			var localPool = descriptorPool as GLDescriptorPool;

			foreach (var dSet in pDescriptorSets)
			{
				var localSet = dSet as GLDescriptorSet;
				if (localSet != null)
				{
					localSet.Destroy ();
					localPool.Sets.Add (localSet);
				}
			}
			return Result.SUCCESS;

		}

		public void UpdateDescriptorSets (MgWriteDescriptorSet[] pDescriptorWrites, MgCopyDescriptorSet[] pDescriptorCopies)
		{
			if (pDescriptorWrites != null)
			{
				foreach (var desc in pDescriptorWrites)
				{
					GLDescriptorSet localSet = desc.DstSet as GLDescriptorSet;
					if (localSet == null)
					{
						throw new NotSupportedException ();
					}

					switch (desc.DescriptorType)
					{
					case MgDescriptorType.SAMPLER:
					case MgDescriptorType.COMBINED_IMAGE_SAMPLER:
					case MgDescriptorType.SAMPLED_IMAGE:
						var x = desc.DstBinding;

						int offset = (int)desc.DstArrayElement;
						int count = (int)desc.DescriptorCount;

						if (localSet.Bindings.Length >= (offset + count))
						{
							// VULKAN WOULD CONTINUE ONTO WRITE ADDITIONAL VALUES TO NEXT BINDING
							// ONLY ONE SET OF BINDING USED
							throw new IndexOutOfRangeException ();
						}

						// HOPEFULLY DESCRIPTOR SETS ARE GROUPED BY COMMON TYPES
						for (int i = 0; i < count; ++i)
						{
							MgDescriptorImageInfo info = desc.ImageInfo [i];						

							var localSampler = info.Sampler as GLSampler;
							var localView = info.ImageView as GLImageView;	

							// Generate bindless texture handle 
							// FIXME : messy as F***

							var texHandle = GL.Arb.GetTextureSamplerHandle (
								                 localView.TextureId,
												 localSampler.SamplerId);

							var imageDesc = localSet.Bindings [offset + i].ImageDesc;
							imageDesc.Replace (texHandle);
						}					
						break;
					default:
						throw new NotSupportedException ();					
					}

				}
			}
		}
		public Result CreateFramebuffer (MgFramebufferCreateInfo pCreateInfo, MgAllocationCallbacks allocator, out MgFramebuffer pFramebuffer)
		{
			throw new NotImplementedException ();
		}
		public void DestroyFramebuffer (MgFramebuffer framebuffer, MgAllocationCallbacks allocator)
		{
			throw new NotImplementedException ();
		}
		public Result CreateRenderPass (MgRenderPassCreateInfo pCreateInfo, MgAllocationCallbacks allocator, out MgRenderPass pRenderPass)
		{
			throw new NotImplementedException ();
		}
		public void DestroyRenderPass (MgRenderPass renderPass, MgAllocationCallbacks allocator)
		{
			throw new NotImplementedException ();
		}
		public void GetRenderAreaGranularity (MgRenderPass renderPass, out MgExtent2D pGranularity)
		{
			throw new NotImplementedException ();
		}
		public Result CreateCommandPool (MgCommandPoolCreateInfo pCreateInfo, MgAllocationCallbacks allocator, out IMgCommandPool pCommandPool)
		{
			pCommandPool = new GLCommandPool ();
			return Result.SUCCESS;
		}
//		public void DestroyCommandPool (IMgCommandPool commandPool, MgAllocationCallbacks allocator)
//		{
//			throw new NotImplementedException ();
//		}
//		public Result ResetCommandPool (IMgCommandPool commandPool, MgCommandPoolResetFlagBits flags)
//		{
//			throw new NotImplementedException ();
//		}
		public Result AllocateCommandBuffers (MgCommandBufferAllocateInfo pAllocateInfo, IMgCommandBuffer[] pCommandBuffers)
		{
			throw new NotImplementedException ();
		}
		public void FreeCommandBuffers (IMgCommandPool commandPool, IMgCommandBuffer[] pCommandBuffers)
		{
			throw new NotImplementedException ();
		}
		public Result CreateSharedSwapchainsKHR (MgSwapchainCreateInfoKHR[] pCreateInfos, MgAllocationCallbacks allocator, out MgSwapchainKHR[] pSwapchains)
		{
			throw new NotImplementedException ();
		}
		public Result CreateSwapchainKHR (MgSwapchainCreateInfoKHR pCreateInfo, MgAllocationCallbacks allocator, out MgSwapchainKHR pSwapchain)
		{
			throw new NotImplementedException ();
		}
		public void DestroySwapchainKHR (MgSwapchainKHR swapchain, MgAllocationCallbacks allocator)
		{
			throw new NotImplementedException ();
		}
		public Result GetSwapchainImagesKHR (MgSwapchainKHR swapchain, out IMgImage[] pSwapchainImages)
		{
			throw new NotImplementedException ();
		}
		public Result AcquireNextImageKHR (MgSwapchainKHR swapchain, ulong timeout, MgSemaphore semaphore, MgFence fence, out uint pImageIndex)
		{
			throw new NotImplementedException ();
		}
		#endregion
		
	}
}

