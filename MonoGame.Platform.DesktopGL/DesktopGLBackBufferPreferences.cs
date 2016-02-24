using Microsoft.Xna.Framework;

namespace MonoGame.Platform.DesktopGL
{
	//#if WINDOWS || MONOMAC || DESKTOPGL
	public class DesktopGLBackBufferPreferences : IBackBufferPreferences
	{
		#region IBackBufferPreferences implementation

		public int DefaultBackBufferHeight
		{
			get {
				return 480;
			}
		}

		public int DefaultBackBufferWidth {
			get {
				return 800;
			}
		}

		#endregion

	}
}

