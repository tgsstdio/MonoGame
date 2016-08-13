// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Android.Views;
using MonoGame.Graphics;

namespace MonoGame.Platform.AndroidGL
{
    public class AndroidGamePlatform : BaseGamePlatform
    {
		private IAndroidGameActivity mActivity;
		private IAndroidCompatibility mCompatibility;
		private IMediaLibrary mMediaLibrary;
		private IMediaPlayer mMediaPlayer;
		private readonly IGraphicsDevice mDevice;
		private IGameBackbone mBackbone;
		private IPrimaryThreadLoader mPrimaryThreadLoader;
		private readonly IViewRefocuser mViewRefocuser;
		private IBaseActivityInfo mActivityInfo;

		IViewResumer mViewResumer;
		private IGraphicsDeviceManager mManager;

		public AndroidGamePlatform (
			IPlatformActivator activator,
			IGraphicsDeviceManager manager,

			IAndroidGameActivity activity,
			IAndroidCompatibility compatibility,
			AndroidGLOrientationSetter windowing,
			IMediaPlayer mediaPlayer,
			IMediaLibrary mediaLibrary,
			IGraphicsDevice device,
			IGameBackbone backbone,
			IPrimaryThreadLoader primaryThreadLoader,
			IViewRefocuser viewRefocuser,
			IViewResumer viewResumer,
			IBaseActivityInfo activityInfo
			)
			: base (activator)
        {
			mManager = manager;
			mActivity = activity;
			mCompatibility = compatibility;
			mMediaLibrary = mediaLibrary;
			mMediaPlayer = mediaPlayer;
			mDevice = device;
			mBackbone = backbone;
			mPrimaryThreadLoader = primaryThreadLoader;
			mViewRefocuser = viewRefocuser;
			mViewResumer = viewResumer;
			mActivityInfo = activityInfo;
			mWindowing = windowing;

			System.Diagnostics.Debug.Assert(mActivity != null, "Must set Game.Activity before creating the Game instance");
			mActivity.Paused += Activity_Paused;
			mActivity.Resumed += Activity_Resumed;

            // _gameWindow = new AndroidGameWindow(Game.Activity, game);
			///Window = window;

//			mMediaLibrary.Context = Game.Activity;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
				mActivity.Paused -= Activity_Paused;
				mActivity.Resumed -= Activity_Resumed;
            }
            base.Dispose(disposing);
        }

        private bool _initialized;
        public static bool IsPlayingVdeo { get; set; }
		private AndroidGLOrientationSetter mWindowing;

        public override void Exit()
        {
			mActivity.MoveTaskToBack(true);
        }

		public override void RunLoop(Action doAction)
        {
            throw new NotSupportedException("The Android platform does not support synchronous run loops");
        }

        public override void StartRunLoop()
        {
			mViewResumer.Resume();
        }

        public override bool BeforeUpdate(GameTime gameTime)
        {
            if (!_initialized)
            {
				mBackbone.DoInitialize();
                _initialized = true;
            }

            return true;
        }

        public override bool BeforeDraw(GameTime gameTime)
        {
			mPrimaryThreadLoader.DoLoads();
            return !IsPlayingVdeo;
        }

        public override void BeforeInitialize()
        {
			var currentOrientation = mCompatibility.GetAbsoluteOrientation();

			switch (mActivity.ConfigurationOrientation)
            {
                case Android.Content.Res.Orientation.Portrait:
                    this.mWindowing.SetOrientation(currentOrientation == DisplayOrientation.PortraitDown ? DisplayOrientation.PortraitDown : DisplayOrientation.Portrait);
                    break;
                default:
                    this.mWindowing.SetOrientation(currentOrientation == DisplayOrientation.LandscapeRight ? DisplayOrientation.LandscapeRight : DisplayOrientation.LandscapeLeft);
                    break;
            }
            base.BeforeInitialize();
			mViewRefocuser.TouchEnabled = true;
        }

        public override bool BeforeRun()
        {

            // Run it as fast as we can to allow for more response on threaded GPU resource creation
			mViewRefocuser.Run();

            return false;
        }

        public override void EnterFullScreen()
        {
        }

        public override void ExitFullScreen()
        {
        }

        public override void BeginScreenDeviceChange(bool willBeFullScreen)
        {
        }

        public override void EndScreenDeviceChange(string screenDeviceName, int clientWidth, int clientHeight)
        {
            // Force the Viewport to be correctly set
            mManager.ResetClientBounds();
        }

        // EnterForeground
        void Activity_Resumed(object sender, EventArgs e)
        {
			if (!Activator.IsActive)
            {
				Activator.IsActive = true;
				mViewResumer.Resume();
				if(_MediaPlayer_PrevState == MediaState.Playing && mActivityInfo.AutoPauseAndResumeMediaPlayer)
                	mMediaPlayer.Resume();
				if (!mViewRefocuser.IsFocused)
					mViewRefocuser.Refocus();
            }
        }

		MediaState _MediaPlayer_PrevState = MediaState.Stopped;
	    // EnterBackground
        void Activity_Paused(object sender, EventArgs e)
        {
			if (Activator.IsActive)
            {
				Activator.IsActive = false;
				_MediaPlayer_PrevState = mMediaPlayer.State;
				//_gameWindow.GameView.Pause();
				mViewRefocuser.Pause();
				//_gameWindow.GameView.ClearFocus();
				mViewRefocuser.Clear();
				if(mActivityInfo.AutoPauseAndResumeMediaPlayer)
					mMediaPlayer.Pause();
            }
        }

        public override GameRunBehavior DefaultRunBehavior
        {
            get { return GameRunBehavior.Asynchronous; }
        }
		
		public override void Log(string Message) 
		{
#if LOGGING
			Android.Util.Log.Debug("MonoGameDebug", Message);
#endif
		}
		
        public override void Present()
        {
            try
            {
				mDevice.Present();

				mViewRefocuser.SwapBuffers();
            }
            catch (Exception ex)
            {
                Android.Util.Log.Error("Error in swap buffers", ex.ToString());
            }
        }
    }
}
