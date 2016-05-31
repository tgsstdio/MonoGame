using System;
using Microsoft.Xna.Framework.Graphics;
using OpenTK.Graphics;

namespace MonoGame.Platform.DesktopGL
{
	public class OpenTKDeviceQuery : IOpenTKDeviceQuery
	{
		#region IOpenTKDeviceQuery implementation


		/// <summary>
		/// Converts <see cref="PresentInterval"/> to OpenGL swap interval.
		/// </summary>
		/// <returns>A value according to EXT_swap_control</returns>
		/// <param name="interval">The <see cref="PresentInterval"/> to convert.</param>
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

		/// <summary>
		/// Convert a <see cref="SurfaceFormat"/> to an OpenTK.Graphics.ColorFormat.
		/// This is used for setting up the backbuffer format of the OpenGL context.
		/// </summary>
		/// <returns>An OpenTK.Graphics.ColorFormat instance.</returns>
		/// <param name="format">The <see cref="SurfaceFormat"/> to convert.</param>
		public ColorFormat GetColorFormat (SurfaceFormat format)
		{
			switch (format)
			{
			case SurfaceFormat.Alpha8:
				return new ColorFormat(0, 0, 0, 8);
			case SurfaceFormat.Bgr565:
				return new ColorFormat(5, 6, 5, 0);
			case SurfaceFormat.Bgra4444:
				return new ColorFormat(4, 4, 4, 4);
			case SurfaceFormat.Bgra5551:
				return new ColorFormat(5, 5, 5, 1);
			case SurfaceFormat.Bgr32:
				return new ColorFormat(8, 8, 8, 0);
			case SurfaceFormat.Bgra32:
			case SurfaceFormat.Color:
			case SurfaceFormat.ColorSRgb:
				return new ColorFormat(8, 8, 8, 8);
			case SurfaceFormat.Rgba1010102:
				return new ColorFormat(10, 10, 10, 2);
			default:
				// Floating point backbuffers formats could be implemented
				// but they are not typically used on the backbuffer. In
				// those cases it is better to create a render target instead.
				throw new NotSupportedException();
			}
		}

		#endregion
	}
}

