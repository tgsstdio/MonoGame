using System;
using Microsoft.Xna.Framework.Graphics;
using Android.Views;

namespace MonoGame.Platform.AndroidGL
{
	public class ForceFullScreenToggle : IForceFullScreenToggle
	{
		private readonly Window mWindow;
		private readonly IPresentationParameters mPresentationParameters;

		public ForceFullScreenToggle (IPresentationParameters presentation, Window window)
		{
			mPresentationParameters = presentation;
			mWindow = window;
		}

		#region IForceFullScreenToggle implementation

		public void ForceSetFullScreen()
		{
			if (mPresentationParameters.IsFullScreen)
			{
				mWindow.ClearFlags(Android.Views.WindowManagerFlags.ForceNotFullscreen);
				mWindow.SetFlags(WindowManagerFlags.Fullscreen, WindowManagerFlags.Fullscreen);
			}
			else
				mWindow.SetFlags(WindowManagerFlags.ForceNotFullscreen, WindowManagerFlags.ForceNotFullscreen);
		}

		#endregion
	}
}

