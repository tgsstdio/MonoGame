// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using DryIoc;
using MonoGame.Platform.AndroidGL.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using Android.Content.PM;
using MonoGame.Platform.AndroidGL.Input.Touch;
using MonoGame.Core;
using Microsoft.Xna.Framework.Input;
using MonoGame.Platform.AndroidGL.Graphics;
using MonoGame.Graphics;
using Microsoft.Core.Graphics;
using Microsoft.Xna.Framework.Input.Touch;

namespace MonoGame.Platform.AndroidGL.Example
{
	using Container = DryIoc.Container;

	[CLSCompliant(false)]
	[Activity (Label = "MonoGame.Platform.Android.Example",
		ConfigurationChanges = ConfigChanges.KeyboardHidden,
		ScreenOrientation = ScreenOrientation.SensorLandscape,
		MainLauncher = true,
		Icon = "@mipmap/icon")]
#if OUYA
	public class AndroidGameActivity : Ouya.Console.Api.OuyaActivity
#else
	public class AndroidGameActivity : Activity
#endif
    {
		private Container mContainer;

		private Game mGame;

		private IBaseActivity mBasicStarter = null;

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
			mContainer = new Container ();

			// register game here
			mContainer.Register<Game, PlaceholderGame>(Reuse.Singleton);

			RegisterMonoGameComponents();

			var se = this.RequestedOrientation;

			try
			{
				mBasicStarter = mContainer.Resolve<IBaseActivity> ();
				mGame = mContainer.Resolve<Game> ();
				mBasicStarter.OnCreate (savedInstanceState);
			}
			catch(Exception ex)
			{
				Console.WriteLine (ex.Message);
			}

           // _orientationListener = new OrientationListener(this);

			//Game.Activity = this;
		}

		private void RegisterMonoGameComponents()
		{
			mContainer.RegisterInstance<Context> (this);
			mContainer.RegisterInstance<Window> (this.Window);
			mContainer.RegisterInstance<Activity> (this);

			mContainer.Register<IScreenLock, ScreenLock> (Reuse.Singleton);
			mContainer.Register<BroadcastReceiver, ScreenReceiver> (Reuse.Singleton);
			mContainer.Register<IForceFullScreenToggle, ForceFullScreenToggle> (Reuse.Singleton);
			mContainer.Register<IBaseActivity, BaseActivity> (Reuse.Singleton);
			mContainer.Register<IAndroidGameActivity, EnclosedGameActivity> (Reuse.Singleton);
			mContainer.Register<IBaseActivityInfo, BaseActivityInfo> (Reuse.Singleton);
			mContainer.Register<IAndroidCompatibility, AndroidCompatibility> (Reuse.Singleton);
			mContainer.Register<IOrientationListener, OrientationListener> (Reuse.Singleton);
			mContainer.Register<IBroadcastReceiverRegistry, BroadcastReceiverRegistry> (Reuse.Singleton);
			mContainer.Register<IViewRefocuser, ViewRefocuser> (Reuse.Singleton);
			mContainer.Register<IAndroidKeyboardListener, AndroidKeyboardListener> (Reuse.Singleton);
			mContainer.Register<IGamePlatform, AndroidGamePlatform> (Reuse.Singleton);
			mContainer.Register<AndroidGLFalseWindowing> (Reuse.Singleton);
			mContainer.Register<AndroidGLTrueWindowing> (Reuse.Singleton);
			mContainer.Register<IWindowOrientationListener, MockWindowOrientationListener> (Reuse.Singleton);
			mContainer.Register<IClientWindowBounds, AndroidGLClientWindowBounds> (Reuse.Singleton);

			mContainer.Register<IGraphicsDevicePlatform, AndroidGLGraphicsDevicePlatform> (Reuse.Singleton);
			mContainer.Register<IAndroidGLDevicePlatform, Es20GLDevicePlatform> (Reuse.Singleton);
			mContainer.Register<IGLFramebufferHelper, AndroidGLFramebufferHelper> (Reuse.Singleton);
			mContainer.Register<IGLFramebufferHelperSelector, Es20GLFramebufferHelperSelector> (Reuse.Singleton);
			mContainer.Register<IGraphicsCapabilities, GraphicsCapabilities> (Reuse.Singleton);
			mContainer.Register<IGraphicsCapabilitiesLookup, Es20GraphicCapabilitiesLookup> (Reuse.Singleton);
			mContainer.Register<IGLExtensionLookup, Es20GLSpecificExtensionLookup> (Reuse.Singleton);
			mContainer.Register<IGraphicsAdapterCollection, AndroidGLGraphicsAdapterCollection> (Reuse.Singleton);
			mContainer.Register<IGraphicsDevicePreferences, DefaultGraphicsDevicePreferences> (Reuse.Singleton);

			mContainer.Register<AndroidGLThreading> (Reuse.Singleton);

			mContainer.Register<IViewResumer, ViewResumer> (Reuse.Singleton);

			mContainer.Register<MonoGameAndroidGameView> (Reuse.Singleton);
			mContainer.RegisterMapping<View, MonoGameAndroidGameView>();

			mContainer.Register<IPresentationParameters, DefaultPresentationParameters> (Reuse.Singleton);
			mContainer.Register<IBackBufferPreferences, DefaultBackBufferPreferences> (Reuse.Singleton);

			mContainer.Register<IGraphicsDeviceManager, AndroidGLGraphicsDeviceManager> (Reuse.Singleton);
			mContainer.Register<IGraphicsProfiler, DefaultGraphicsDeviceProfiler> (Reuse.Singleton);

			mContainer.Register<IAndroidGLGameWindow, AndroidGLGameWindow> (Reuse.Singleton);
			mContainer.RegisterMapping<Microsoft.Xna.Framework.GameWindow, IAndroidGLGameWindow>();

			mContainer.Register<IAndroidTouchEventManager, AndroidTouchEventManager> (Reuse.Singleton);
			mContainer.Register<ITouchListener, TouchPanelState> (Reuse.Singleton);
			mContainer.Register<ITouchPanelCapabilities, AndroidGLTouchPanelCapabilities>(Reuse.Singleton);

			mContainer.Register<IGraphicsDeviceQuery, DefaultGraphicsDeviceQuery> (Reuse.Singleton);
			mContainer.Register<IPlatformActivator, PlatformActivator> (Reuse.Singleton);

			
			mContainer.Register<IContentManager, NullContentManager> (Reuse.Singleton);
			mContainer.Register<IContentTypeReaderManager, NullContentTypeReaderManager> (Reuse.Singleton);
			mContainer.Register<IGameBackbone, GameBackbone> (Reuse.Singleton);

			mContainer.Register<IGraphicsDeviceService, NullGraphicsDeviceService> (Reuse.Singleton);
			mContainer.Register<IGraphicsDevice, NullGraphicsDevice> (Reuse.Singleton);
			mContainer.Register<IResumeManager, NullResumeManager> (Reuse.Singleton);

			mContainer.Register<IMediaPlayer, NullMediaPlayer> (Reuse.Singleton);
			mContainer.Register<IMediaLibrary, NullMediaLibrary> (Reuse.Singleton);

			// WINDOW EXIT
			mContainer.Register<IDrawSuppressor, DrawSupressor>(Reuse.Singleton);
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

			if (mGame != null)
			{
				mBasicStarter.OnPause ();
			}
        }

        public event EventHandler Resumed;
        protected override void OnResume()
        {
            base.OnResume();
            if (Resumed != null)
                Resumed(this, EventArgs.Empty);

			if (mGame != null)
            {
				mBasicStarter.OnResume ();
            }
        }

		protected override void OnDestroy ()
		{
			if (mGame != null)
			{			
				mBasicStarter.OnDestroy ();
				mGame = null;
			}
			mContainer.Dispose ();
			mContainer = null;
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
