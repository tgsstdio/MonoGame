using System;
using OpenTK;
using Microsoft.Xna.Framework;

namespace MonoGame.Platform.DesktopGL.Input
{
	using MouseState = Microsoft.Xna.Framework.Input.MouseState;
	using IMouseListener = Microsoft.Xna.Framework.Input.IMouseListener;
	using IGameWindow = Microsoft.Xna.Framework.IGameWindow; 
	using ButtonState = Microsoft.Xna.Framework.Input.ButtonState;

	public class DesktopGLMouseListener :  IMouseListener
	{
		public IGameWindow PrimaryWindow {
			get;
			set;
		}

		private MouseState _defaultState = new MouseState();
		#region IMouseListener implementation

		private readonly INativeWindow mSource;
		public DesktopGLMouseListener (INativeWindow source)
		{
			mSource = source;
		}

		public MouseState GetState (IGameWindow window)
		{
			var state = OpenTK.Input.Mouse.GetCursorState();

			var internalWindow = window as OpenTKGameWindow;
			var result = window.LastMouseState;
			if (internalWindow != null)
			{
				var pc = internalWindow.Window.PointToClient (new System.Drawing.Point (state.X, state.Y));

				result.X = pc.X;
				result.Y = pc.Y;

				result.LeftButton = (ButtonState)state.LeftButton;
				result.RightButton = (ButtonState)state.RightButton;
				result.MiddleButton = (ButtonState)state.MiddleButton;
				result.XButton1 = (ButtonState)state.XButton1;
				result.XButton2 = (ButtonState)state.XButton2;

				// XNA uses the winapi convention of 1 click = 120 delta
				// OpenTK scales 1 click = 1.0 delta, so make that match
				result.ScrollWheelValue = (int)(state.Scroll.Y * 120);
			}
			window.LastMouseState = result;
			return window.LastMouseState;
		}

		public MouseState GetNativeState (IGameWindow window)
		{
			var state = OpenTK.Input.Mouse.GetCursorState();

			var internalWindow = mSource;

			var result = new MouseState();
			if (internalWindow != null)
			{
				var pc = mSource.PointToClient (new System.Drawing.Point (state.X, state.Y));

				result.X = pc.X;
				result.Y = pc.Y;

				result.LeftButton = (ButtonState)state.LeftButton;
				result.RightButton = (ButtonState)state.RightButton;
				result.MiddleButton = (ButtonState)state.MiddleButton;
				result.XButton1 = (ButtonState)state.XButton1;
				result.XButton2 = (ButtonState)state.XButton2;

				// XNA uses the winapi convention of 1 click = 120 delta
				// OpenTK scales 1 click = 1.0 delta, so make that match
				result.ScrollWheelValue = (int)(state.Scroll.Y * 120);
			}
			window.LastMouseState = result;
			return result;
		}

		public MouseState GetState ()
		{
			if (PrimaryWindow != null)
			{
				//return GetState (PrimaryWindow);
				return GetNativeState(PrimaryWindow);
			}

			return _defaultState;
		}

		public void UpdateStatePosition (int x, int y)
		{
			var result = PrimaryWindow.LastMouseState;
			result.X = x;
			result.Y = y;
			PrimaryWindow.LastMouseState = result;
		}

		public IntPtr WindowHandle {
			get {
				return mSource.WindowInfo.Handle;
			}
		}

		#endregion


	}
}

