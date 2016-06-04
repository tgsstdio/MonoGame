using System;
using Android.App;
using Android.Views;
using Microsoft.Xna.Framework;

namespace MonoGame.Platform.AndroidGL.Example
{
	public class EnclosedGameActivity : IAndroidGameActivity
	{
		private readonly Activity mActivity;
		public EnclosedGameActivity (Activity activity)
		{
			mActivity = activity;
		}

		#region IAndroidGameActivity implementation

		public event EventHandler Resumed;

		public event EventHandler Paused;

		public void MoveTaskToBack (bool value)
		{
			mActivity.MoveTaskToBack (value);
		}

		public bool HasSystemFeature (string feature)
		{
			return mActivity.PackageManager.HasSystemFeature (feature);
		}

		public Android.Content.Context GetContext ()
		{
			return mActivity;
		}

		public Android.Content.Res.Orientation ConfigurationOrientation {
			get {
				return mActivity.Resources.Configuration.Orientation;
			}
			set {
				mActivity.Resources.Configuration.Orientation = value;
			}
		}

		public Android.Content.PM.ScreenOrientation RequestedOrientation {
			get {
				return mActivity.RequestedOrientation;
			}
			set {
				mActivity.RequestedOrientation = value;
			}
		}

		public void ForceFullScreen (bool value)
		{
			if (value)
			{
				mActivity.Window.ClearFlags (WindowManagerFlags.ForceNotFullscreen);
				mActivity.Window.SetFlags (WindowManagerFlags.Fullscreen, WindowManagerFlags.Fullscreen);
			} else
			{
				mActivity.Window.SetFlags (WindowManagerFlags.ForceNotFullscreen, WindowManagerFlags.ForceNotFullscreen);
			}
		}

		public Vector2 GetDeviceSize ()
		{
			float density = mActivity.Resources.DisplayMetrics.Density;

			return new Vector2
				(
					(mActivity.Resources.DisplayMetrics.WidthPixels / density),
					(mActivity.Resources.DisplayMetrics.HeightPixels / density)
				);
		}
		#endregion
	}
}

