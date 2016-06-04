using System;
using Microsoft.Xna.Framework.Graphics;
using Android.Views;
using System.Collections.Generic;

namespace MonoGame.Platform.AndroidGL.Graphics
{
	public class AndroidGLGraphicsAdapter : IGraphicsAdapter
	{
		private IAndroidGameActivity mActivity;
		public AndroidGLGraphicsAdapter (IAndroidGameActivity activity)
		{
			mActivity = activity;
			InitializeSupportedModes ();
		}

		private void InitializeSupportedModes()
		{
			var modes = new List<DisplayMode>(new[] { CurrentDisplayMode });
			SupportedDisplayModes = new DisplayModeCollection(modes);
		}

		#region IGraphicsAdapter implementation

		public DisplayMode CurrentDisplayMode {
			get {
				var deviceSize = mActivity.GetDeviceSize ();
				return new DisplayMode((int) deviceSize.X, (int) deviceSize.Y, 60, SurfaceFormat.Color); 
			}
		}

		public DisplayModeCollection SupportedDisplayModes {
			get;
			private set;
		}

		/// <summary>
		/// Gets a <see cref="System.Boolean"/> indicating whether
		/// <see cref="GraphicsAdapter.CurrentDisplayMode"/> has a
		/// Width:Height ratio corresponding to a widescreen <see cref="DisplayMode"/>.
		/// Common widescreen modes include 16:9, 16:10 and 2:1.
		/// </summary>
		public bool IsWideScreen
		{
			get
			{
				// Common non-widescreen modes: 4:3, 5:4, 1:1
				// Common widescreen modes: 16:9, 16:10, 2:1
				// XNA does not appear to account for rotated displays on the desktop
				const float limit = 4.0f / 3.0f;
				var aspect = CurrentDisplayMode.AspectRatio;
				return aspect > limit;
			}
		}

		#endregion
	}
}

