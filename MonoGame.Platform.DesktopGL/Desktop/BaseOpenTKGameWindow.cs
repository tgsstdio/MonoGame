using Microsoft.Xna.Framework;
using OpenTK.Platform;

namespace MonoGame.Platform.DesktopGL
{
	public abstract class BaseOpenTKGameWindow : GameWindow
	{
		public abstract void SetWindowVisible(bool visible);
		public abstract bool IsWindowFocused();
		public abstract void ProcessEvents ();
		public abstract void SetMouseVisible(bool visible);

		/// <summary>
		/// Toggles the full screen.
		/// FOR DesktopGLWindowResetter
		/// </summary>
		public abstract void ToggleFullScreen();
		public abstract void ChangeClientBounds(Rectangle clientBounds);
		public abstract IWindowInfo GetWindowInfo ();
	}
}

