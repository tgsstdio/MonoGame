using System;
using MonoGame.Core;
using MonoGame.Content;
using FreeImageAPI;
using Magnesium;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace MonoGame.Textures.FreeImageNET
{
	public class FITexture2DLoader : ITexture2DLoader
	{
		private readonly IContentStreamer mContentStreamer;
		private readonly ITextureSortingKeyGenerator mKeyGenerator;
		private readonly IMgThreadPartition mPartition;	
		private readonly IMgTextureGenerator mLoader;
		public FITexture2DLoader (IContentStreamer cStreamer, ITextureSortingKeyGenerator keyGenerator, IMgThreadPartition partition, IMgTextureGenerator loader)
		{
			mContentStreamer = cStreamer;
			mKeyGenerator = keyGenerator;
			mPartition = partition;
			mLoader = loader;

			// Check if FreeImage is available
			if (!FreeImage.IsAvailable())
			{
				throw new Exception("FreeImage is not available!");
			}
		}

		#region ITexture2DLoader implementation

		MgFormat GetFormatType (uint bpp, FREE_IMAGE_TYPE imageType, FREE_IMAGE_COLOR_TYPE colorType)
		{
			if (imageType == FREE_IMAGE_TYPE.FIT_BITMAP)
			{
				if (bpp == 24 && colorType == FREE_IMAGE_COLOR_TYPE.FIC_RGB)
				{
					return MgFormat.R8G8B8_UINT;
				} 
				else if (bpp == 32 && colorType == FREE_IMAGE_COLOR_TYPE.FIC_RGBALPHA)
				{
					return MgFormat.R8G8B8A8_UINT;
				}
			}

			// default
			throw new NotSupportedException ();
		}

		public ITexture2D Load (AssetIdentifier assetId)
		{
			FIBITMAP dib = new FIBITMAP();
			try
			{
				using (var fs = mContentStreamer.LoadContent (assetId, new []{".png"}))
				{
					dib = FreeImage.LoadFromStream(fs);
					// Check success
					if (dib.IsNull)
					{
						throw new Exception("FITexture2DLoader image load error");
					}

					// stuff here
					int key = mKeyGenerator.GenerateSortingKey();

					var width = FreeImage.GetWidth(dib);
					var height = FreeImage.GetHeight(dib);
					var size = FreeImage.GetDIBSize(dib);

					var bpp = FreeImage.GetBPP(dib);
					var imageType = FreeImage.GetImageType(dib);
					var colorType = FreeImage.GetColorType(dib);
					var pitch = FreeImage.GetPitch(dib);

					var formatType = GetFormatType(bpp, imageType, colorType);

					// staging buffer here
					var imageInfo = new MgImageSource
					{
						Mipmaps = new []
						{
							new MgImageMipmap
							{
								Height = height,
								Width = width,
								Offset = 0,
								Size = size
							}
						},
						Format = formatType,
						Width = width,
						Height = height,
						Size = size,					
					};

					var src = FreeImage.GetBits(dib);

					byte[] imageData = new byte[size];
					Marshal.Copy(src, imageData, 0, (int)size);

					var textureInfo = mLoader.Load(imageData, imageInfo, null, null);
					IMgImage image = textureInfo.Image;

					IMgImageView view;
					var viewCreateInfo = new MgImageViewCreateInfo{
						ViewType = MgImageViewType.TYPE_2D,
						Format = formatType,
						Image = image,
						Components = new MgComponentMapping{
							R = MgComponentSwizzle.R,
							G = MgComponentSwizzle.G,
							B = MgComponentSwizzle.B, 
							A = MgComponentSwizzle.A,
						},
						SubresourceRange = new MgImageSubresourceRange
						{
							AspectMask = MgImageAspectFlagBits.COLOR_BIT,
							BaseArrayLayer = 0,
							BaseMipLevel = 0,
							LayerCount = 1,
							LevelCount = 1,
						},
					};

					var result = mPartition.Device.CreateImageView(viewCreateInfo, null, out view);
					Debug.Assert(result == Result.SUCCESS);

					// For best compatibility and to keep the default wrap mode of XNA, only set ClampToEdge if either
					// dimension is not a power of two.
					var addressMode = MgSamplerAddressMode.REPEAT;
					if (((width & (width - 1)) != 0) || ((height & (height - 1)) != 0))
						addressMode = MgSamplerAddressMode.CLAMP_TO_EDGE;

					IMgSampler sampler;
					var samplerCreateInfo = new MgSamplerCreateInfo{
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
						//MaxLod = (useStaging) ? (float)texture.mipLevels : 0.0f,
						MaxLod = 1,
						// Enable anisotropic filtering
						MaxAnisotropy = 8,
						AnisotropyEnable = true,
						BorderColor = MgBorderColor.FLOAT_OPAQUE_WHITE,
					};
					result = mPartition.Device.CreateSampler(samplerCreateInfo, null, out sampler);
					Debug.Assert(result == Result.SUCCESS);

					var texture = new FITexture2D(key, image, view, sampler, textureInfo.ImageLayout, textureInfo.DeviceMemory);
					texture.Format = Microsoft.Xna.Framework.Graphics.SurfaceFormat.Color;
					texture.Bounds = new Rectangle(0,0, (int) width, (int) height);
					texture.Width = (int)width;
					texture.Height = (int)height;

					result = mPartition.Queue.QueueWaitIdle();
					Debug.Assert(result == Result.SUCCESS);
					mPartition.Device.FreeCommandBuffers(mPartition.CommandPool, new[]{textureInfo.Command});

					return texture;
				}
			}
			finally
			{
				FreeImage.UnloadEx (ref dib);
			}
		}

		#endregion
	}
}

