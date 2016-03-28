// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Android.Views;

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

		public AndroidGamePlatform (
			IGraphicsDeviceManager manager,
			IPlatformActivator activator,

			IAndroidGameActivity activity,
			IAndroidCompatibility compatibility,
			IAndroidGLGameWindow window,
			IMediaPlayer mediaPlayer,
			IMediaLibrary mediaLibrary,
			IGraphicsDevice device,
			IGameBackbone backbone,
			IPrimaryThreadLoader primaryThreadLoader,
			IViewRefocuser viewRefocuser,
			IBaseActivityInfo activityInfo
			)
			: base (manager, activator)
        {
			mActivity = activity;
			mCompatibility = compatibility;
			mMediaLibrary = mediaLibrary;
			mMediaPlayer = mediaPlayer;
			mDevice = device;
			mBackbone = backbone;
			mPrimaryThreadLoader = primaryThreadLoader;
			mViewRefocuser = viewRefocuser;
			mActivityInfo = activityInfo;

			System.Diagnostics.Debug.Assert(mActivity != null, "Must set Game.Activity before creating the Game instance");
			mActivity.Paused += Activity_Paused;
			mActivity.Resumed += Activity_Resumed;

            // _gameWindow = new AndroidGameWindow(Game.Activity, game);
			Window = window;

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
        private IAndroidGLGameWindow _gameWindow;

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
			mViewRefocuser.Resume();
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
                    this._gameWindow.SetOrientation(currentOrientation == DisplayOrientation.PortraitDown ? DisplayOrientation.PortraitDown : DisplayOrientation.Portrait, false);
                    break;
                default:
                    this._gameWindow.SetOrientation(currentOrientation == DisplayOrientation.LandscapeRight ? DisplayOrientation.LandscapeRight : DisplayOrientation.LandscapeLeft, false);
                    break;
            }
            base.BeforeInitialize();
            _gameWindow.GameView.TouchEnabled = true;
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
            Graphics.ResetClientBounds();
        }

        // EnterForeground
        void Activity_Resumed(object sender, EventArgs e)
        {
			if (!Activator.IsActive)
            {
				Activator.IsActive = true;
				mViewRefocuser.Resume();
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
