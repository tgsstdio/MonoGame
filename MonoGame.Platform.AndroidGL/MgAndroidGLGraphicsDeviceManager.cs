// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using MonoGame.Core;

using MonoGame.Graphics;
using Magnesium;

namespace MonoGame.Platform.AndroidGL
{
	public class MgAndroidGLGraphicsDeviceManager : MgGraphicsDeviceManager
	{
		private readonly IAndroidGameActivity mActivity;
		private AndroidGLOrientationSetter mWindowing;

		private int _preferredBackBufferHeight;
		private int _preferredBackBufferWidth;

		//private bool _preferMultiSampling;
		private bool _drawBegun;
		private bool _hardwareModeSwitch = true;

		private readonly IGraphicsDeviceQuery mDeviceQuery;
		private readonly ITouchListener mTouchPanel;
		private IGraphicsDevicePreferences mDevicePreferences;

		private readonly IClientWindowBounds mClient;

		public MgAndroidGLGraphicsDeviceManager(
			IMgGraphicsDevice device,
			IMgThreadPartition partition,
			IPresentationParameters presentationParameters,
			IMgSwapchainCollection swapchainCollection,
			IGraphicsAdapterCollection adapters,
			IGraphicsProfiler profiler,

			IAndroidGameActivity activity,
			AndroidGLOrientationSetter windowing,
			IClientWindowBounds client,
			IBackBufferPreferences backBufferPreferences,
			IGraphicsDevicePreferences devicePreferences,
			ITouchListener touchPanel,
			IGraphicsDeviceQuery deviceQuery
			)
			: base(device, partition, presentationParameters, swapchainCollection, adapters, profiler)

		{
			mClient = client;
			mDevicePreferences = devicePreferences;

			mActivity = activity;
			mWindowing = windowing;
			mTouchPanel = touchPanel;
			mDeviceQuery = deviceQuery;

			_preferredBackBufferHeight = backBufferPreferences.DefaultBackBufferHeight;
			_preferredBackBufferWidth = backBufferPreferences.DefaultBackBufferWidth;

			//GraphicsProfile = GraphicsProfile.HiDef;

		}

		~MgAndroidGLGraphicsDeviceManager()
		{
			Dispose(false);
		}

		protected override void AfterDeviceCreation()
		{
			OnDeviceCreated(EventArgs.Empty);
		}

		#region IGraphicsDeviceService Members

		public event EventHandler<EventArgs> DeviceCreated;
		public event EventHandler<EventArgs> DeviceDisposing;
		public event EventHandler<EventArgs> DeviceReset;
		public event EventHandler<EventArgs> DeviceResetting;
		public event EventHandler<PreparingDeviceSettingsEventArgs> PreparingDeviceSettings;

		// FIXME: Why does the GraphicsDeviceManager not know enough about the
		//        GraphicsDevice to raise these events without help?
		internal void OnDeviceDisposing(EventArgs e)
		{
			Raise(DeviceDisposing, e);
		}

		// FIXME: Why does the GraphicsDeviceManager not know enough about the
		//        GraphicsDevice to raise these events without help?
		internal void OnDeviceResetting(EventArgs e)
		{
			Raise(DeviceResetting, e);
		}

		// FIXME: Why does the GraphicsDeviceManager not know enough about the
		//        GraphicsDevice to raise these events without help?
		internal void OnDeviceReset(EventArgs e)
		{
			Raise(DeviceReset, e);
		}

		// FIXME: Why does the GraphicsDeviceManager not know enough about the
		//        GraphicsDevice to raise these events without help?
		internal void OnDeviceCreated(EventArgs e)
		{
			Raise(DeviceCreated, e);
		}

		private void Raise<TEventArgs>(EventHandler<TEventArgs> handler, TEventArgs e)
			where TEventArgs : EventArgs
		{
			if (handler != null)
				handler(this, e);
		}

		#endregion

		protected override void ApplyLocalChanges ()
		{
			// Trigger a change in orientation in case the supported orientations have changed
			mWindowing.SetOrientation (mWindowing.CurrentOrientation);
			// Ensure the presentation parameter orientation and buffer size matches the window
			PresentationParameters.DisplayOrientation = mWindowing.CurrentOrientation;
			// Set the presentation parameters' actual buffer size to match the orientation
			bool isLandscape = (0 != (mWindowing.CurrentOrientation & (DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight)));
			int w = mDeviceQuery.PreferredBackBufferWidth;
			int h = mDeviceQuery.PreferredBackBufferHeight;
			PresentationParameters.BackBufferWidth = isLandscape ? Math.Max (w, h) : Math.Min (w, h);
			PresentationParameters.BackBufferHeight = isLandscape ? Math.Min (w, h) : Math.Max (w, h);
			ResetClientBounds ();
			// Set the new display size on the touch panel.
			//
			// TODO: In XNA this seems to be done as part of the 
			// GraphicsDevice.DeviceReset event... we need to get 
			// those working.
			//
			mTouchPanel.DisplayWidth = PresentationParameters.BackBufferWidth;
			mTouchPanel.DisplayHeight = PresentationParameters.BackBufferHeight;
		}

		protected override void ForceSetFullScreen()
		{
			mActivity.ForceFullScreen (IsFullScreen);
		}

		public bool PreferMultiSampling
		{
			get
			{
				return mDevicePreferences.PreferMultiSampling;
			}
			set
			{
				mDevicePreferences.PreferMultiSampling = value;
			}
		}

		public SurfaceFormat PreferredBackBufferFormat {
			get;
			set;
		}

		protected override void ReleaseManagedResources ()
		{

		}

		protected override void ReleaseUnmanagedResources ()
		{

		}

		protected override void ApplySupportedOrientation (DisplayOrientation value)
		{
			if (mActivity != null)
				mWindowing.SetSupportedOrientations(value);
		}

		/// <summary>
		/// This method is used by MonoGame Android to adjust the game's drawn to area to fill
		/// as much of the screen as possible whilst retaining the aspect ratio inferred from
		/// aspectRatio = (PreferredBackBufferWidth / PreferredBackBufferHeight)
		///
		/// NOTE: this is a hack that should be removed if proper back buffer to screen scaling
		/// is implemented. To disable it's effect, in the game's constructor use:
		///
		///     graphics.IsFullScreen = true;
		///     graphics.PreferredBackBufferHeight = Window.ClientBounds.Height;
		///     graphics.PreferredBackBufferWidth = Window.ClientBounds.Width;
		///
		/// </summary>
		public override void ResetClientBounds()
		{
			float preferredAspectRatio = (float) mDeviceQuery.PreferredBackBufferWidth /
				(float)mDeviceQuery.PreferredBackBufferHeight;
			float displayAspectRatio = (float) GraphicsAdapter.CurrentDisplayMode.Width / 
				(float)GraphicsAdapter.CurrentDisplayMode.Height;

			float adjustedAspectRatio = preferredAspectRatio;

			if ((preferredAspectRatio > 1.0f && displayAspectRatio < 1.0f) ||
			(preferredAspectRatio < 1.0f && displayAspectRatio > 1.0f))
			{
			// Invert preferred aspect ratio if it's orientation differs from the display mode orientation.
			// This occurs when user sets preferredBackBufferWidth/Height and also allows multiple supported orientations
			adjustedAspectRatio = 1.0f / preferredAspectRatio;
			}

			const float EPSILON = 0.00001f;
			var newClientBounds = new Rectangle();
			if (displayAspectRatio > (adjustedAspectRatio + EPSILON))
			{
				// Fill the entire height and reduce the width to keep aspect ratio
				newClientBounds.Height = GraphicsAdapter.CurrentDisplayMode.Height;
				newClientBounds.Width = (int)(newClientBounds.Height * adjustedAspectRatio);
				newClientBounds.X = (GraphicsAdapter.CurrentDisplayMode.Width - newClientBounds.Width) / 2;
			}
			else if (displayAspectRatio < (adjustedAspectRatio - EPSILON))
			{
			// Fill the entire width and reduce the height to keep aspect ratio
				newClientBounds.Width = GraphicsAdapter.CurrentDisplayMode.Width;
			newClientBounds.Height = (int)(newClientBounds.Width / adjustedAspectRatio);
				newClientBounds.Y = (GraphicsAdapter.CurrentDisplayMode.Height - newClientBounds.Height) / 2;
			}
			else
			{
				// Set the ClientBounds to match the DisplayMode
				newClientBounds.Width = GraphicsAdapter.CurrentDisplayMode.Width;
				newClientBounds.Height = GraphicsAdapter.CurrentDisplayMode.Height;
			}

			// Ensure buffer size is reported correctly
			PresentationParameters.BackBufferWidth = newClientBounds.Width;
			PresentationParameters.BackBufferHeight = newClientBounds.Height;

//			// Set the veiwport so the (potentially) resized client bounds are drawn in the middle of the screen
//			GraphicsDevice.CurrentViewport = new MgViewport {
//				X = newClientBounds.X,
//				Y = -newClientBounds.Y, 
//				Width = newClientBounds.Width,
//				Height = newClientBounds.Height,
//				MinDepth = 0f,
//				MaxDepth = 1,
//			};

			mClient.ChangeClientBounds(newClientBounds);

			// Touch panel needs latest buffer size for scaling
			mTouchPanel.DisplayWidth = newClientBounds.Width;
			mTouchPanel.DisplayHeight = newClientBounds.Height;

			Android.Util.Log.Debug("MonoGame", "GraphicsDeviceManager.ResetClientBounds: newClientBounds=" + newClientBounds.ToString());
		}

	}
}
