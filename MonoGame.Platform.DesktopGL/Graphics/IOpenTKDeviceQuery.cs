using Microsoft.Xna.Framework.Graphics;
using OpenTK.Graphics;

namespace MonoGame.Platform.DesktopGL
{
	public interface IOpenTKDeviceQuery
	{
		int GetSwapInterval(PresentInterval interval);
		ColorFormat GetColorFormat(SurfaceFormat format);
	}
}

