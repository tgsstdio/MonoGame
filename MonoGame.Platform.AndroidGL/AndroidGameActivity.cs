// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Platform.AndroidGL
{
	[CLSCompliant(false)]
#if OUYA
    public class AndroidGameActivity : Ouya.Console.Api.OuyaActivity
#else
    public class AndroidGameActivity : Activity
#endif
    {
        //internal Game Game { private get; set; }
		// REMEMBER NO CONSTRUCTOR // THIS IS THE ENTRY POINT
//		public AndroidGameActivity (BroadcastReceiver receiver, IScreenLock screenLock, OrientationListener orientationListener, IGraphicsDeviceManager deviceManager)
//		{
//			_receiver = _receiver;
//			_sr = sr;
//			_orientationListener = orientationListener;
//			_deviceManager = deviceManager;
//		}

		//private IScreenLock mScreenLock;
		//private BroadcastReceiver mReceiver;
		private IGraphicsDeviceManager _deviceManager;
		//private readonly BroadcastReceiver mBroadcastReceiver;
		//private OrientationEventListener mOrientationEventListener;
		private IForceFullScreenToggle mFullScreenToggle;
		private IAndroidGLGameWindow mGameWindow;
		private Game mGame;
		private View mView;

      //  public bool AutoPauseAndResumeMediaPlayer = true;
        public bool RenderOnUIThread = true; 

		readonly IBaseActivity mBasicStarter = null;

		/// <summary>
		/// OnCreate called when the activity is launched from cold or after the app
		/// has been killed due to a higher priority app needing the memory
		/// </summary>
		/// <param name='savedInstanceState'>
		/// Saved instance state.
		/// </param>
		protected override void OnCreate (Bundle savedInstanceState)
		{
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);

			// IOC here

			mBasicStarter.OnCreate ();

           // _orientationListener = new OrientationListener(this);

			//Game.Activity = this;
		}

        public event EventHandler Paused;

//		public override void OnConfigurationChanged (Android.Content.Res.Configuration newConfig)
//		{
//			// we need to refresh the viewport here.
//			base.OnConfigurationChanged (newConfig);
//		}

        protected override void OnPause()
        {
            base.OnPause();
            if (Paused != null)
                Paused(this, EventArgs.Empty);

			mBasicStarter.OnPause ();
        }

        public event EventHandler Resumed;
        protected override void OnResume()
        {
            base.OnResume();
            if (Resumed != null)
                Resumed(this, EventArgs.Empty);

			if (mGame != null)
            {
				DuringResume ();
				mBasicStarter.OnResume ();
            }
        }

		void DuringResume()
		{
			mFullScreenToggle.ForceSetFullScreen ();
			mView.RequestFocus();
		}

		protected override void OnDestroy ()
		{
			mBasicStarter.OnDestroy ();
			mGame = null;
			base.OnDestroy ();
		}
    }

	[CLSCompliant(false)]
	public static class ActivityExtensions
    {
        public static ActivityAttribute GetActivityAttribute(this AndroidGameActivity obj)
        {			
            var attr = obj.GetType().GetCustomAttributes(typeof(ActivityAttribute), true);
			if (attr != null)
			{
            	return ((ActivityAttribute)attr[0]);
			}
			return null;
        }
    }

}
