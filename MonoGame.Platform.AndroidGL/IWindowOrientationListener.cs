using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using Android.Content.PM;

namespace MonoGame.Platform.AndroidGL
{
	public interface IWindowOrientationListener
	{
		void OnOrientationChanged();
	}	
}

