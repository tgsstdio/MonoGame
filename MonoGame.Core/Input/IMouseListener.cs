using System;

namespace Microsoft.Xna.Framework.Input
{
	public interface IMouseListener
	{
		IGameWindow PrimaryWindow { get; set; }
		IntPtr WindowHandle { get; }
		MouseState GetState (IGameWindow window);
		MouseState GetState ();
		void UpdateStatePosition (int x, int y);
	}
}

