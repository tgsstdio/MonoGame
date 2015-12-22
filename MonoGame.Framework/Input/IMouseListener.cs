using System;

namespace Microsoft.Xna.Framework.Input
{
	public interface IMouseListener
	{
		GameWindow PrimaryWindow { get; set; }
		IntPtr WindowHandle { get; }
		MouseState GetState (GameWindow window);
		MouseState GetState ();
		void UpdateStatePosition (int x, int y);
	}
}

