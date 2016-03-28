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
using Microsoft.Xna.Framework;
using OpenTK.Platform.Android;
using DryIoc;
using MonoGame.Platform.AndroidGL.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace MonoGame.Platform.AndroidGL
{
	using Container = DryIoc.Container;

	[CLSCompliant(false)]
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

			RegisterMonoGameComponents();

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

			mContainer.Register<IScreenLock, IScreenLock> (Reuse.Singleton);
			mContainer.Register<IForceFullScreenToggle, ForceFullScreenToggle> (Reuse.Singleton);
			mContainer.Register<IBaseActivity, BaseActivity> (Reuse.Singleton);
			mContainer.Register<IBaseActivityInfo, BaseActivityInfo> (Reuse.Singleton);
			mContainer.Register<IAndroidCompatibility, AndroidCompatibility> (Reuse.Singleton);
			mContainer.Register<IOrientationListener, OrientationListener> (Reuse.Singleton);
			mContainer.Register<IBroadcastReceiverRegistry, BroadcastReceiverRegistry> (Reuse.Singleton);
			mContainer.Register<IViewRefocuser, ViewRefocuser> (Reuse.Singleton);
			mContainer.Register<IAndroidKeyboardListener, AndroidKeyboardListener> (Reuse.Singleton);
			mContainer.Register<IGamePlatform, AndroidGamePlatform> (Reuse.Singleton);
			mContainer.Register<AndroidGLThreading> (Reuse.Singleton);

			mContainer.Register<GameWindow, AndroidGLGameWindow> (Reuse.Singleton);
			mContainer.Register<IContentManager, NullContentManager> (Reuse.Singleton);
			mContainer.Register<IContentTypeReaderManager, NullContentTypeReaderManager> (Reuse.Singleton);
			mContainer.Register<IGameBackbone, GameBackbone> (Reuse.Singleton);

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
				mBasicStarter.OnResume ();
            }
        }

		protected override void OnDestroy ()
		{
			mBasicStarter.OnDestroy ();
			mGame = null;
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
