using System;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Core;

namespace MonoGame.Core
{
	public static class FormatExtensions
	{
		public static int GetSize(this VertexElementFormat elementFormat)
		{
			switch (elementFormat)
			{
			case VertexElementFormat.Single:
				return 4;

			case VertexElementFormat.Vector2:
				return 8;

			case VertexElementFormat.Vector3:
				return 12;

			case VertexElementFormat.Vector4:
				return 16;

			case VertexElementFormat.Color:
				return 4;

			case VertexElementFormat.Byte4:
				return 4;

			case VertexElementFormat.Short2:
				return 4;

			case VertexElementFormat.Short4:
				return 8;

			case VertexElementFormat.NormalizedShort2:
				return 4;

			case VertexElementFormat.NormalizedShort4:
				return 8;

			case VertexElementFormat.HalfVector2:
				return 4;

			case VertexElementFormat.HalfVector4:
				return 8;
			}
			return 0;
		}

		public static uint GetSize(this SurfaceFormat surfaceFormat)
		{
			switch (surfaceFormat)
			{
			case SurfaceFormat.Dxt1:
			case SurfaceFormat.Dxt1SRgb:
			case SurfaceFormat.Dxt1a:
			case SurfaceFormat.RgbPvrtc2Bpp:
			case SurfaceFormat.RgbaPvrtc2Bpp:
			case SurfaceFormat.RgbEtc1:
				// One texel in DXT1, PVRTC 2bpp and ETC1 is a minimum 4x4 block, which is 8 bytes
				return 8u;
			case SurfaceFormat.Dxt3:
			case SurfaceFormat.Dxt3SRgb:
			case SurfaceFormat.Dxt5:
			case SurfaceFormat.Dxt5SRgb:
			case SurfaceFormat.RgbPvrtc4Bpp:
			case SurfaceFormat.RgbaPvrtc4Bpp:
			case SurfaceFormat.RgbaAtcExplicitAlpha:
			case SurfaceFormat.RgbaAtcInterpolatedAlpha:
				// One texel in DXT3, DXT5 and PVRTC 4bpp is a minimum 4x4 block, which is 16 bytes
				return 16u;
			case SurfaceFormat.Alpha8:
				return 1u;
			case SurfaceFormat.Bgr565:
			case SurfaceFormat.Bgra4444:
			case SurfaceFormat.Bgra5551:
			case SurfaceFormat.HalfSingle:
			case SurfaceFormat.NormalizedByte2:
				return 2u;
			case SurfaceFormat.Color:
			case SurfaceFormat.ColorSRgb:
			case SurfaceFormat.Single:
			case SurfaceFormat.Rg32:
			case SurfaceFormat.HalfVector2:
			case SurfaceFormat.NormalizedByte4:
			case SurfaceFormat.Rgba1010102:
			case SurfaceFormat.Bgra32:
			case SurfaceFormat.Bgra32SRgb:
			case SurfaceFormat.Bgr32:
			case SurfaceFormat.Bgr32SRgb:
				return 4u;
			case SurfaceFormat.HalfVector4:
			case SurfaceFormat.Rgba64:
			case SurfaceFormat.Vector2:
				return 8u;
			case SurfaceFormat.Vector4:
				return 16u;
			default:
				throw new ArgumentException();
			}
		}
	}
}

