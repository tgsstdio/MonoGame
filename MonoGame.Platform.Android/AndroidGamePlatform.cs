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

		public AndroidGamePlatform (
			IGraphicsDeviceManager manager,
			IPlatformActivator activator,
			// TODO : should remove mouse
			IMouseListener mouse,

			IAndroidGameActivity activity,
			IAndroidCompatibility compatibility,
			AndroidGameWindow window,
			IMediaPlayer mediaPlayer,
			IMediaLibrary mediaLibrary)
			: base (manager, activator, mouse)
        {
			mActivity = activity;
			mCompatibility = compatibility;
			mMediaLibrary = mediaLibrary;
			mMediaPlayer = mediaPlayer;

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
        private AndroidGameWindow _gameWindow;

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
			_gameWindow.GameView.Resume();
        }

        public override bool BeforeUpdate(GameTime gameTime)
        {
            if (!_initialized)
            {
                Game.DoInitialize();
                _initialized = true;
            }

            return true;
        }

        public override bool BeforeDraw(GameTime gameTime)
        {
            PrimaryThreadLoader.DoLoads();
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
			_gameWindow.GameView.Run();

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
            if (!IsActive)
            {
                IsActive = true;
				_gameWindow.GameView.Resume();
				if(_MediaPlayer_PrevState == MediaState.Playing && Game.Activity.AutoPauseAndResumeMediaPlayer)
                	mMediaPlayer.Resume();
				if (!_gameWindow.GameView.IsFocused)
					_gameWindow.GameView.RequestFocus();
            }
        }

		MediaState _MediaPlayer_PrevState = MediaState.Stopped;
	    // EnterBackground
        void Activity_Paused(object sender, EventArgs e)
        {
            if (IsActive)
            {
                IsActive = false;
				_MediaPlayer_PrevState = MediaPlayer.State;
				_gameWindow.GameView.Pause();
				_gameWindow.GameView.ClearFocus();
				if(Game.Activity.AutoPauseAndResumeMediaPlayer)
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
                var device = Game.GraphicsDevice;
                if (device != null)
                    device.Present();

				_gameWindow.GameView.SwapBuffers();
            }
            catch (Exception ex)
            {
                Android.Util.Log.Error("Error in swap buffers", ex.ToString());
            }
        }
    }
}
