using System;
using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Hardware;
using Android.Views;

namespace MonoGame.Platform.AndroidGL
{
    internal class OrientationListener : OrientationEventListener
    {
		private readonly ScreenReceiver mScreenReceiver;
		private readonly AndroidGameWindow mGameWindow;
		private readonly IAndroidCompatibility mCompatibility;
        /// <summary>
        /// Constructor. SensorDelay.Ui is passed to the base class as this orientation listener 
        /// is just used for flipping the screen orientation, therefore high frequency data is not required.
        /// </summary>
		public OrientationListener(Context context, ScreenReceiver receiver, AndroidGameWindow window, IAndroidCompatibility compatibility)
            : base(context, SensorDelay.Ui)
        {
			mScreenReceiver = receiver;
			mGameWindow = window;
			mCompatibility = compatibility;
        }

        public override void OnOrientationChanged(int orientation)
        {
            if (orientation == OrientationEventListener.OrientationUnknown)
                return;

            // Avoid changing orientation whilst the screen is locked
			if (mScreenReceiver.ScreenLocked)
                return;

			var disporientation = mCompatibility.GetAbsoluteOrientation(orientation);

            // Only auto-rotate if target orientation is supported and not current
           // AndroidGameWindow gameWindow = (AndroidGameWindow)Game.Instance.Window;
			if ((mGameWindow.GetEffectiveSupportedOrientations() & disporientation) != 0 &&
				disporientation != mGameWindow.CurrentOrientation)
            {
				mGameWindow.SetOrientation(disporientation, true);
            }
        }
    }
}