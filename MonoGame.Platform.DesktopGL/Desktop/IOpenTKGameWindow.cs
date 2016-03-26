using Microsoft.Xna.Framework;
using OpenTK.Platform;

namespace MonoGame.Platform.DesktopGL
{
	public interface IOpenTKGameWindow : Microsoft.Xna.Framework.IGameWindow
	{
		void SetWindowVisible(bool visible);
		bool IsWindowFocused();
		void ProcessEvents ();
		void SetMouseVisible(bool visible);

		/// <summary>
		/// Toggles the full screen.
		/// FOR DesktopGLWindowResetter
		/// </summary>
		void ToggleFullScreen();
		void ChangeClientBounds(Rectangle clientBounds);
		IWindowInfo GetWindowInfo ();
	}
}

