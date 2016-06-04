// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using Microsoft.Xna.Framework.Input.Touch;

using Android.Content.PM;

namespace MonoGame.Platform.AndroidGL
{
    /// <summary>
    /// Allows retrieval of capabilities information from touch panel device.
    /// </summary>
    public class AndroidGLTouchPanelCapabilities : ITouchPanelCapabilities
    {
        private bool hasPressure;
        private bool isConnected;
        private int maximumTouchCount;
        private bool initialized;


		private bool mHasFeatureTouchscreenMultitouchJazzhand;
		private bool mHasFeatureTouchscreenMultitouchDistinct;
		public AndroidGLTouchPanelCapabilities (IAndroidGameActivity activity)
		{
			// http://developer.android.com/reference/android/content/pm/PackageManager.html#FEATURE_TOUCHSCREEN			
			isConnected = activity.HasSystemFeature(PackageManager.FeatureTouchscreen);
			mHasFeatureTouchscreenMultitouchJazzhand = activity.HasSystemFeature (PackageManager.FeatureTouchscreenMultitouchJazzhand);
			mHasFeatureTouchscreenMultitouchDistinct = activity.HasSystemFeature (PackageManager.FeatureTouchscreenMultitouchDistinct);
		}

        public void Initialize()
        {
            if (!initialized)
            {
                initialized = true;

                // There does not appear to be a way of finding out if a touch device supports pressure.
                // XNA does not expose a pressure value, so let's assume it doesn't support it.
                hasPressure = false;

				if (mHasFeatureTouchscreenMultitouchJazzhand)
                    maximumTouchCount = 5;
				else if (mHasFeatureTouchscreenMultitouchDistinct)
                    maximumTouchCount = 2;
                else
                    maximumTouchCount = 1;
            }
        }

        public bool HasPressure
        {
            get
            {
                return hasPressure;
            }
        }

        /// <summary>
        /// Returns true if a device is available for use.
        /// </summary>
        public bool IsConnected
        {
            get
            {
                return isConnected;
            }
        }

        /// <summary>
        /// Returns the maximum number of touch locations tracked by the touch panel device.
        /// </summary>
        public int MaximumTouchCount
        {
            get
            {
                return maximumTouchCount;
            }
        }

    }
}