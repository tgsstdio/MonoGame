using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using Android.Content.PM;

namespace MonoGame.Platform.AndroidGL
{
	public class AndroidGLOrientationSetter
	{
		private DisplayOrientation _supportedOrientations = DisplayOrientation.Default;
		private DisplayOrientation _currentOrientation;

        public void SetSupportedOrientations(DisplayOrientation orientations)
        {
            _supportedOrientations = orientations;
        }

		public DisplayOrientation CurrentOrientation
		{
			get
			{
				return _currentOrientation;
			}
		}

		private IGraphicsDeviceQuery mDeviceQuery;
		private readonly ITouchListener mTouchPanel;
		private readonly IAndroidGameActivity mActivity;
		private readonly IWindowOrientationListener mListener;
		public AndroidGLOrientationSetter (
			IGraphicsDeviceQuery deviceQuery,
			ITouchListener touchPanel, 
			IAndroidGameActivity activity,
			IWindowOrientationListener listener)
		{
			mDeviceQuery = deviceQuery;
			mTouchPanel = touchPanel;
			mActivity = activity;
			mListener = listener;
		}

		/// <summary>
		/// In Xna, setting SupportedOrientations = DisplayOrientation.Default (which is the default value)
		/// has the effect of setting SupportedOrientations to landscape only or portrait only, based on the
		/// aspect ratio of PreferredBackBufferWidth / PreferredBackBufferHeight
		/// </summary>
		/// <returns></returns>
		public DisplayOrientation GetEffectiveSupportedOrientations()
		{
			if (_supportedOrientations == DisplayOrientation.Default)
			{
				if (mDeviceQuery.PreferredBackBufferWidth > mDeviceQuery.PreferredBackBufferHeight)
				{
					return DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;
				}
				else
				{
					return DisplayOrientation.Portrait | DisplayOrientation.PortraitDown;
				}
			}
			else
			{
				return _supportedOrientations;
			}
		}

		/// <summary>
		/// Updates the screen orientation. Filters out requests for unsupported orientations.
		/// </summary>
		public bool SetOrientation(DisplayOrientation newOrientation)
		{
			DisplayOrientation supported = GetEffectiveSupportedOrientations();

			// If the new orientation is not supported, force a supported orientation
			if ((supported & newOrientation) == 0)
			{
				if ((supported & DisplayOrientation.LandscapeLeft) != 0)
					newOrientation = DisplayOrientation.LandscapeLeft;
				else if ((supported & DisplayOrientation.LandscapeRight) != 0)
					newOrientation = DisplayOrientation.LandscapeRight;
				else if ((supported & DisplayOrientation.Portrait) != 0)
					newOrientation = DisplayOrientation.Portrait;
				else if ((supported & DisplayOrientation.PortraitDown) != 0)
					newOrientation = DisplayOrientation.PortraitDown;
			}

			DisplayOrientation oldOrientation = CurrentOrientation;

			SetDisplayOrientation(newOrientation);
			mTouchPanel.DisplayOrientation = newOrientation;

			return (oldOrientation != CurrentOrientation);
		}

		// A copy of ScreenOrientation from Android 2.3
		// This allows us to continue to support 2.2 whilst
		// utilising the 2.3 improved orientation support.
		enum ScreenOrientationAll
		{
			Unspecified = -1,
			Landscape = 0,
			Portrait = 1,
			User = 2,
			Behind = 3,
			Sensor = 4,
			Nosensor = 5,
			SensorLandscape = 6,
			SensorPortrait = 7,
			ReverseLandscape = 8,
			ReversePortrait = 9,
			FullSensor = 10,
		}

		private void SetDisplayOrientation(DisplayOrientation value)
		{
			if (value != _currentOrientation)
			{
				DisplayOrientation supported = GetEffectiveSupportedOrientations();
				ScreenOrientation requestedOrientation = ScreenOrientation.Unspecified;
				bool wasPortrait = _currentOrientation == DisplayOrientation.Portrait || _currentOrientation == DisplayOrientation.PortraitDown;
				bool requestPortrait = false;

				bool didOrientationChange = false;
				// Android 2.3 and above support reverse orientations
				int sdkVer = (int)Android.OS.Build.VERSION.SdkInt;
				if (sdkVer >= 10)
				{
					// Check if the requested orientation is supported. Default means all are supported.
					if ((supported & value) != 0)
					{
						didOrientationChange = true;
						_currentOrientation = value;
						switch (value)
						{
						case DisplayOrientation.LandscapeLeft:
							requestedOrientation = (ScreenOrientation)ScreenOrientationAll.Landscape;
							requestPortrait = false;
							break;
						case DisplayOrientation.LandscapeRight:
							requestedOrientation = (ScreenOrientation)ScreenOrientationAll.ReverseLandscape;
							requestPortrait = false;
							break;
						case DisplayOrientation.Portrait:
							requestedOrientation = (ScreenOrientation)ScreenOrientationAll.Portrait;
							requestPortrait = true;
							break;
						case DisplayOrientation.PortraitDown:
							requestedOrientation = (ScreenOrientation)ScreenOrientationAll.ReversePortrait;
							requestPortrait = true;
							break;
						}
					}
				}
				else
				{
					// Check if the requested orientation is either of the landscape orientations and any landscape orientation is supported.
					if ((value == DisplayOrientation.LandscapeLeft || value == DisplayOrientation.LandscapeRight) &&
						((supported & (DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight)) != 0))
					{
						didOrientationChange = true;
						_currentOrientation = DisplayOrientation.LandscapeLeft;
						requestedOrientation = ScreenOrientation.Landscape;
						requestPortrait = false;
					}
					// Check if the requested orientation is either of the portrain orientations and any portrait orientation is supported.
					else if ((value == DisplayOrientation.Portrait || value == DisplayOrientation.PortraitDown) &&
						((supported & (DisplayOrientation.Portrait | DisplayOrientation.PortraitDown)) != 0))
					{
						didOrientationChange = true;
						_currentOrientation = DisplayOrientation.Portrait;
						requestedOrientation = ScreenOrientation.Portrait;
						requestPortrait = true;
					}
				}

				if (didOrientationChange)
				{
					// Android doesn't fire Released events for existing touches
					// so we need to clear them out.
					if (wasPortrait != requestPortrait)
					{
						mTouchPanel.ReleaseAllTouches();
					}

					mActivity.RequestedOrientation = requestedOrientation;

					mListener.OnOrientationChanged();
				}
			}
		}
	}
}

