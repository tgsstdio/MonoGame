// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

namespace Microsoft.Xna.Framework.Input.Touch
{
	public interface ITouchPanelCapabilities
	{
		void Initialize();
		bool HasPressure {
			get;
		}

		/// <summary>
		/// Returns true if a device is available for use.
		/// </summary>
		bool IsConnected {
			get;
		}

		/// <summary>
		/// Returns the maximum number of touch locations tracked by the touch panel device.
		/// </summary>
		int MaximumTouchCount {
			get;
		}
	}

}