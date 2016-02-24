// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Input;

namespace Microsoft.Xna.Framework
{
	public interface IGameWindow : IDisposable
	{
		bool AllowUserResizing { get; set; }
		Rectangle ClientBounds { get; }
		bool AllowAltF4 { get; set; }
		DisplayOrientation CurrentOrientation { get; }
		IntPtr Handle { get; }
		string ScreenDeviceName { get; }
		string Title { get; set; }
		bool IsBorderless { get; set; }
		event EventHandler<EventArgs> ClientSizeChanged;
		event EventHandler<EventArgs> OrientationChanged;
		event EventHandler<EventArgs> ScreenDeviceNameChanged;
		void BeginScreenDeviceChange (bool willBeFullScreen);
		void EndScreenDeviceChange (string screenDeviceName, int clientWidth, int clientHeight);
		void EndScreenDeviceChange (string screenDeviceName);
		void SetSupportedOrientations (DisplayOrientation orientations);
		ITouchListener Touch { get; }
		//MouseState MouseState { get; }
		MouseState LastMouseState {
			get;
			set;
		}
	}

}
