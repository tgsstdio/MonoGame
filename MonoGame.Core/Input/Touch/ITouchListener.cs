// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.
using System;

namespace Microsoft.Xna.Framework.Input.Touch
{
	public interface ITouchListener
	{
		bool EnableMouseGestures {
			get;
			set;
		}

		GestureSample GetGestumeSample ();

		//TouchPanelState GetPanelState ();

		bool IsGestureAvailable {
			get;
		}

		bool EnableMouseTouchPoint {
			get;
			set;
		}

		GestureType EnabledGestures {
			get;
			set;
		}

		DisplayOrientation DisplayOrientation {
			get;
			set;
		}

		int DisplayWidth {
			get;
			set;
		}

		int DisplayHeight {
			get;
			set;
		}

		IntPtr WindowHandle {
			get;
			set;
		}

		void AddEvent (int id, TouchLocationState state, Vector2 position);
		void AddEvent (int id, TouchLocationState state, Vector2 position, bool isMouse);
		ITouchPanelCapabilities GetCapabilities ();
		TouchCollection GetState();

		void ReleaseAllTouches ();
	}
}
