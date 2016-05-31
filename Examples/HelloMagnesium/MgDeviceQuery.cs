using System;
using Microsoft.Xna.Framework.Graphics;
using OpenTK.Graphics;

namespace HelloMagnesium
{
	public class MgDeviceQuery : IMgDeviceQuery
	{
		#region IMgDeviceQuery implementation

		public int GetStencilBit (DepthFormat format)
		{
			return format == DepthFormat.Depth24Stencil8 ? 8 : 0;
		}

		public int GetSwapInterval (PresentInterval interval)
		{
			// See http://www.opengl.org/registry/specs/EXT/swap_control.txt
			// and https://www.opengl.org/registry/specs/EXT/glx_swap_control_tear.txt
			// OpenTK checks for EXT_swap_control_tear:
			// if supported, a swap interval of -1 enables adaptive vsync;
			// otherwise -1 is converted to 1 (vsync enabled.)

			switch (interval)
			{

			case PresentInterval.Immediate:
				return 0;
			case PresentInterval.One:
				return 1;
			case PresentInterval.Two:
				return 2;
			case PresentInterval.Default:
			default:
				return -1;
			}
		}

		public int GetDepthBit (DepthFormat format)
		{
			return format == DepthFormat.None ? 0 : format == DepthFormat.Depth16 ? 16 : 24;
		}

		public ColorFormat GetColorFormat(Magnesium.MgFormat format)
		{
			switch (format)
			{
			case Magnesium.MgFormat.R8_UNORM:
				return new ColorFormat(0, 0, 0, 8);
			case Magnesium.MgFormat.B5G6R5_UNORM_PACK16:
				return new ColorFormat(5, 6, 5, 0);
			case Magnesium.MgFormat.B4G4R4A4_UNORM_PACK16:
				return new ColorFormat(4, 4, 4, 4);
			case Magnesium.MgFormat.B5G5R5A1_UNORM_PACK16:
				return new ColorFormat(5, 5, 5, 1);
			case Magnesium.MgFormat.B8G8R8_UNORM:
				return new ColorFormat(8, 8, 8, 0);
			case Magnesium.MgFormat.B8G8R8A8_UNORM:
			case Magnesium.MgFormat.R8G8B8A8_UNORM:
			case Magnesium.MgFormat.R8G8B8A8_SRGB:
				return new ColorFormat(8, 8, 8, 8);
			case Magnesium.MgFormat.A2B10G10R10_UNORM_PACK32:
				return new ColorFormat(10, 10, 10, 2);
			default:
				// Floating point backbuffers formats could be implemented
				// but they are not typically used on the backbuffer. In
				// those cases it is better to create a render target instead.
				throw new NotSupportedException();
			}
		}

		public Magnesium.MgFormat GetFormat (SurfaceFormat format)
		{
			switch (format)
			{
			case SurfaceFormat.Alpha8:
				return Magnesium.MgFormat.R8_UNORM;
			case SurfaceFormat.Bgr565:
				return Magnesium.MgFormat.B5G6R5_UNORM_PACK16;
			case SurfaceFormat.Bgra4444:
				return Magnesium.MgFormat.B4G4R4A4_UNORM_PACK16;
			case SurfaceFormat.Bgra5551:
				return Magnesium.MgFormat.B5G5R5A1_UNORM_PACK16;
			case SurfaceFormat.Bgr32:
				return Magnesium.MgFormat.B8G8R8_UNORM;
			case SurfaceFormat.Bgra32:
				return Magnesium.MgFormat.B8G8R8A8_UNORM;
			case SurfaceFormat.Color:
				return Magnesium.MgFormat.R8G8B8A8_UNORM;
			case SurfaceFormat.ColorSRgb:
				return Magnesium.MgFormat.R8G8B8A8_SRGB;
			case SurfaceFormat.Rgba1010102:
				return Magnesium.MgFormat.A2B10G10R10_UNORM_PACK32;
			default:
				// Floating point backbuffers formats could be implemented
				// but they are not typically used on the backbuffer. In
				// those cases it is better to create a render target instead.
				throw new NotSupportedException();
			}
		}

		public Magnesium.MgFormat GetDepthStencilFormat (DepthFormat format)
		{
			switch (format)
			{
			case DepthFormat.Depth24Stencil8:
				return Magnesium.MgFormat.D24_UNORM_S8_UINT;
			case DepthFormat.Depth16:
				return Magnesium.MgFormat.D16_UNORM_S8_UINT;
			case DepthFormat.Depth24:
				return Magnesium.MgFormat.D24_UNORM_S8_UINT;
			default:
				throw new NotSupportedException();
			}
		}

		#endregion


	}
}

