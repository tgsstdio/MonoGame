using System;
using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Hardware;
using Android.Views;

namespace MonoGame.Platform.AndroidGL
{
	public class OrientationListener : OrientationEventListener, IOrientationListener
    {
		private readonly IScreenLock mScreenLock;
		private readonly IAndroidGLGameWindow mGameWindow;
		private readonly IAndroidCompatibility mCompatibility;
        /// <summary>
        /// Constructor. SensorDelay.Ui is passed to the base class as this orientation listener 
        /// is just used for flipping the screen orientation, therefore high frequency data is not required.
        /// </summary>
		public OrientationListener(Context context, IScreenLock screenLock, IAndroidGLGameWindow window, IAndroidCompatibility compatibility)
            : base(context, SensorDelay.Ui)
        {
			mScreenLock = screenLock;
			mGameWindow = window;
			mCompatibility = compatibility;
        }

        public override void OnOrientationChanged(int orientation)
        {
            if (orientation == OrientationEventListener.OrientationUnknown)
                return;

            // Avoid changing orientation whilst the screen is locked
			if (mScreenLock.ScreenLocked)
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

		#region IOrientationListener implementation

		public void On ()
		{
			if (this.CanDetectOrientation ())
			{
				this.Enable ();
			}
		}

		public void Off ()
		{
			if (this.CanDetectOrientation ())
			{
				this.Disable ();
			}
		}
		#endregion
    }
}