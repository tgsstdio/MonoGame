// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.
using System;
using Android.Content;
using Microsoft.Xna.Framework;

namespace MonoGame.Platform.AndroidGL
{
	public interface IAndroidGameActivity
	{
		Android.Content.Res.Orientation ConfigurationOrientation {
			get;
			set;
		}

		Android.Content.PM.ScreenOrientation RequestedOrientation { set; get; }

		void MoveTaskToBack (bool value);

		event EventHandler Resumed;
		event EventHandler Paused;

		Context GetContext();
	}
}
