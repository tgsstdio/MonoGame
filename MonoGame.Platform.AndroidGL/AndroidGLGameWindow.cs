// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using Android.Content;
using Android.Content.PM;
using Android.Views;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework;
using OpenTK;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Core;

#if OUYA
using Microsoft.Xna.Framework.Input;
#endif

namespace MonoGame.Platform.AndroidGL
{
    [CLSCompliant(false)]
	public class AndroidGLGameWindow : GameWindow, IAndroidGLGameWindow
    {

        internal MonoGameAndroidGameView GameView { get; private set; }
        internal IResumeManager Resumer;
		internal IGraphicsDeviceManager mDeviceManager;
		private readonly IGraphicsDeviceQuery mDeviceQuery;
		private IPlatformActivator mActivator;

		private IGraphicsDevicePlatform _devicePlatform;
        private Rectangle _clientBounds;
        private DisplayOrientation _supportedOrientations = DisplayOrientation.Default;
        private DisplayOrientation _currentOrientation;

        public override IntPtr Handle { get { return IntPtr.Zero; } }

		protected override void ReleaseManagedResources()
		{
			GameView = null;
			GameView.RequestFocus ();
		}

		protected override void ReleaseUnmanagedResources()
		{
			if (GameView != null)
			{
				GameView.Dispose();
				GameView = null;
			}
		}

//        public void SetResumer(IResumeManager resumer)
//        {
//            Resumer = resumer;
//        }
		private ITouchPanel mTouchPanel;
		private TouchPanelState mTouchPanelState;
		private IGameBackbone mBackbone;
		private AndroidGLThreading mThreading;
		private IScreenLock mScreenLock;
		private Context mContext;
		private IAndroidGameActivity mActivity;

		public AndroidGLGameWindow(
			Context context,
			IAndroidGameActivity activity,
			IGraphicsDevicePlatform devicePlatform,
			IGameBackbone backbone,
			MonoGameAndroidGameView view,
			IGraphicsDeviceManager deviceManager,
			IGraphicsDeviceQuery deviceQuery,
			IPlatformActivator activator,
			IResumeManager resumer,
			ITouchPanel touchPanel,
			TouchPanelState touchPanelState,
			AndroidGLThreading threading,
			IScreenLock screenLock
			)
        {
			mContext = context;
			mActivity = activity;
			_devicePlatform = devicePlatform;
			mBackbone = backbone;
			GameView = view;
			mDeviceManager = deviceManager;
			mDeviceQuery = deviceQuery;
			mActivator = activator;
			Resumer = resumer;
			mTouchPanel = touchPanel;
			mTouchPanelState = touchPanelState;
			mThreading = threading;
			mScreenLock = screenLock;
            Initialize(context);

            //game.Services.AddService(typeof(View), GameView);
        }

        private void Initialize(Context context)
        {
            _clientBounds = new Rectangle(0, 0, context.Resources.DisplayMetrics.WidthPixels, context.Resources.DisplayMetrics.HeightPixels);

//            GameView = new MonoGameAndroidGameView(context, this, _game);
//            GameView.RenderOnUIThread = Game.Activity.RenderOnUIThread;
//            GameView.RenderFrame += OnRenderFrame;
//            GameView.UpdateFrame += OnUpdateFrame;
//
//            GameView.RequestFocus();
//            GameView.FocusableInTouchMode = true;

#if OUYA
            GamePad.Initialize();
#endif
        }

        #region AndroidGameView Methods

        private void OnRenderFrame(object sender, FrameEventArgs frameEventArgs)
        {
            if (GameView.GraphicsContext == null || GameView.GraphicsContext.IsDisposed)
                return;

            if (!GameView.GraphicsContext.IsCurrent)
                GameView.MakeCurrent();

			mThreading.Run();
        }

        private void OnUpdateFrame(object sender, FrameEventArgs frameEventArgs)
        {
            if (!GameView.GraphicsContext.IsCurrent)
                GameView.MakeCurrent();

			mThreading.Run();

            //if (_game != null)
            {
				if (!GameView.IsResuming && mActivator.IsActive && !mScreenLock.ScreenLocked) //Only call draw if an update has occured
                {
					mBackbone.Tick();
                }
				else if (mDeviceManager.GraphicsDevice != null)
                {
					mDeviceManager.GraphicsDevice.Clear(Color.Black);
                    if (GameView.IsResuming && Resumer != null)
                    {
                        Resumer.Draw();
                    }
					_devicePlatform.Present();
                }
            }
        }

        #endregion


        public override void SetSupportedOrientations(DisplayOrientation orientations)
        {
            _supportedOrientations = orientations;
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
				var deviceManager = mDeviceManager;
                if (deviceManager == null)
                    return DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;

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
        public void SetOrientation(DisplayOrientation newOrientation, bool applyGraphicsChanges)
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

            if (applyGraphicsChanges && oldOrientation != CurrentOrientation)
				mDeviceManager.ApplyChanges();
        }

        public override string ScreenDeviceName 
        {
            get 
            {
                throw new NotImplementedException ();
            }
        }
   

        public override Rectangle ClientBounds 
        {
            get 
            {
                return _clientBounds;
            }
        }
        
        public void ChangeClientBounds(Rectangle bounds)
        {
            if (bounds != _clientBounds)
            {
                _clientBounds = bounds;
                OnClientSizeChanged();
            }
        }

        public override bool AllowUserResizing 
        {
            get 
            {
                return false;
            }
            set 
            {
                // Do nothing; Ignore rather than raising an exception
            }
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

        public override DisplayOrientation CurrentOrientation
        {
            get
            {
                return _currentOrientation;
            }
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
						mTouchPanelState.ReleaseAllTouches();
                    }

					mActivity.RequestedOrientation = requestedOrientation;

                    OnOrientationChanged();
                }
            }
        }


        public override void BeginScreenDeviceChange(bool willBeFullScreen)
        {
        }

        public override void EndScreenDeviceChange(string screenDeviceName, int clientWidth, int clientHeight)
        {
        }

        protected override void SetTitle(string title)
        {
        }
    }
}

