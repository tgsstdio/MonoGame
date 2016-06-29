using OpenTK.Graphics.OpenGL;
using System;

namespace Magnesium.OpenGL
{
	public static class GLImageFormatExtensions
	{
		internal static void GetGLFormat (this MgFormat format,
			bool supportsSRgb,
			out PixelInternalFormat glInternalFormat,
			out PixelFormat glFormat,
			out PixelType glType)
		{
			glInternalFormat = PixelInternalFormat.Rgba;
			glFormat = PixelFormat.Rgba;
			glType = PixelType.UnsignedByte;

			switch (format) {
			case MgFormat.R8G8B8_UINT:
				glInternalFormat = PixelInternalFormat.Rgb8ui;
				glFormat = PixelFormat.Rgb;
				glType = PixelType.UnsignedByte;
				break;
			case MgFormat.R8G8B8A8_UINT:
				//case SurfaceFormat.Color:
				glInternalFormat = PixelInternalFormat.Rgba8ui;
				glFormat = PixelFormat.Rgba;
				glType = PixelType.UnsignedByte;
				break;
			case MgFormat.R8G8B8A8_SRGB:
				//case SurfaceFormat.ColorSRgb:
				if (!supportsSRgb)
					//goto case SurfaceFormat.Color;
					goto case MgFormat.R8G8B8A8_UINT;
				glInternalFormat = PixelInternalFormat.Srgb; //(PixelInternalFormat) 0x8C40; // PixelInternalFormat.Srgb;
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
	}
}

