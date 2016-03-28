// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.
using Microsoft.Xna.Framework;
using Android.Views;

namespace MonoGame.Platform.AndroidGL
{
	public interface IAndroidGLGameWindow : IGameWindow
	{
		void SetOrientation (DisplayOrientation disporientation, bool b);

		DisplayOrientation GetEffectiveSupportedOrientations ();

		void ChangeClientBounds(Rectangle bounds);
	}
}
