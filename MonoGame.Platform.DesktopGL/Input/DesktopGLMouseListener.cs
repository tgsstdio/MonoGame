﻿using System;
using Microsoft.Xna.Framework.Input;
using OpenTK;

namespace Microsoft.Xna.Framework.DesktopGL.Input
{
	public class DesktopGLMouseListener : IMouseListener
	{
		public GameWindow PrimaryWindow {
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

		public MouseState GetState (GameWindow window)
		{
			var state = OpenTK.Input.Mouse.GetCursorState();
            var pc = ((OpenTKGameWindow)window).Window.PointToClient(new System.Drawing.Point(state.X, state.Y));
			window.MouseState.X = pc.X;
            window.MouseState.Y = pc.Y;

            window.MouseState.LeftButton = (ButtonState)state.LeftButton;
            window.MouseState.RightButton = (ButtonState)state.RightButton;
            window.MouseState.MiddleButton = (ButtonState)state.MiddleButton;
            window.MouseState.XButton1 = (ButtonState)state.XButton1;
            window.MouseState.XButton2 = (ButtonState)state.XButton2;

            // XNA uses the winapi convention of 1 click = 120 delta
            // OpenTK scales 1 click = 1.0 delta, so make that match
            window.MouseState.ScrollWheelValue = (int)(state.Scroll.Y * 120);

            return window.MouseState;
		}

		public MouseState GetState ()
		{
			if (PrimaryWindow != null)
				return GetState(PrimaryWindow);

			return _defaultState;
		}

		public void UpdateStatePosition (int x, int y)
		{
			PrimaryWindow.MouseState.X = x;
			PrimaryWindow.MouseState.Y = y;
		}

		public IntPtr WindowHandle {
			get {
				return mSource.WindowInfo.Handle;
			}
		}

		#endregion


	}
}

