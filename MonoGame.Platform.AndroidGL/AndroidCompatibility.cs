using System;
using System.Linq;
using Android.App;
using Android.Content.Res;
using Android.OS;
using Android.Views;
using Microsoft.Xna.Framework;

namespace MonoGame.Platform.AndroidGL
{
    /// <summary>
    /// Properties that change from how XNA works by default
    /// </summary>
    public class AndroidCompatibility : IAndroidCompatibility
    {
		/// <summary>
		/// Because the Kindle Fire devices default orientation is fliped by 180 degrees from all the other android devices
		/// on the market we need to do some special processing to make sure that LandscapeLeft is the correct way round.
		/// This list contains all the Build.Model strings of the effected devices, it should be added to if and when
		/// more devices exhibit the same issues.
		/// </summary>
        private readonly string[] Kindles = new[] { "KFTT", "KFJWI", "KFJWA", "KFSOWI", "KFTHWA", "KFTHWI", "KFAPWA", "KFAPWI" };

        public bool FlipLandscape { get; private set; }
        public Lazy<Orientation> NaturalOrientation { get; private set; }

//        static AndroidCompatibility()
//        {
//			FlipLandscape = Kindles.Contains(Build.Model);
//            NaturalOrientation = new Lazy<Orientation>(GetDeviceNaturalOrientation);
//        }

        private Orientation GetDeviceNaturalOrientation()
        {
			var orientation = mConfiguration.Orientation;
			SurfaceOrientation rotation = mWindowManager.DefaultDisplay.Rotation;

            if (((rotation == SurfaceOrientation.Rotation0 || rotation == SurfaceOrientation.Rotation180) &&
                orientation == Orientation.Landscape)
                || ((rotation == SurfaceOrientation.Rotation90 || rotation == SurfaceOrientation.Rotation270) &&
                orientation == Orientation.Portrait))
            {
                return Orientation.Landscape;
            }
            else
            {
                return Orientation.Portrait;
            }
        }

        public DisplayOrientation GetAbsoluteOrientation(int orientation)
        {
            // Orientation is reported by the device in degrees compared to the natural orientation
            // Some tablets have a natural landscape orientation, which we need to account for
            if (NaturalOrientation.Value == Orientation.Landscape)
                orientation += 270;

            // Round orientation into one of 4 positions, either 0, 90, 180, 270. 
            int ort = ((orientation + 45) / 90 * 90) % 360;

            // Surprisingly 90 degree is landscape right, except on Kindle devices
            var disporientation = DisplayOrientation.Unknown;
            switch (ort)
            {
                case 90: disporientation = FlipLandscape ? DisplayOrientation.LandscapeLeft : DisplayOrientation.LandscapeRight;
                    break;
                case 270: disporientation = FlipLandscape ? DisplayOrientation.LandscapeRight : DisplayOrientation.LandscapeLeft;
                    break;
                case 0: disporientation = DisplayOrientation.Portrait;
                    break;
                case 180: disporientation = DisplayOrientation.PortraitDown;
                    break;
                default:
                    disporientation = DisplayOrientation.LandscapeLeft;
                    break;
            }

            return disporientation;
        }

		private readonly IWindowManager mWindowManager;
		private readonly Configuration mConfiguration;
		public AndroidCompatibility (IWindowManager windowManager, Configuration configuration)
		{
			mWindowManager = windowManager;
			mConfiguration = configuration;

			FlipLandscape = Kindles.Contains(Build.Model);
			NaturalOrientation = new Lazy<Orientation>(GetDeviceNaturalOrientation);
		}

        /// <summary>
        /// Get the absolute orientation of the device, accounting for platform differences.
        /// </summary>
        /// <returns></returns>
        public DisplayOrientation GetAbsoluteOrientation()
        {
			var orientation = mWindowManager.DefaultDisplay.Rotation;

            // Landscape degrees (provided by the OrientationListener) are swapped by default
            // Since we use the code used by OrientationListener, we have to swap manually
            int degrees;
            switch (orientation)
            {
                case SurfaceOrientation.Rotation90:
                    degrees = 270;
                    break;
                case SurfaceOrientation.Rotation180:
                    degrees = 180;
                    break;
                case SurfaceOrientation.Rotation270:
                    degrees = 90;
                    break;
                default:
                    degrees = 0;
                    break;
            }

            return GetAbsoluteOrientation(degrees);
        }
    }
}
