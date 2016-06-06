// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using MonoGame.Core;

#if MONOMAC
using MonoMac.OpenGL;
#elif GLES
using OpenTK.Graphics.ES20;
#elif OPENGL
using OpenTK.Graphics.OpenGL;
#elif WINDOWS_STOREAPP || WINDOWS_UAP
using Windows.UI.Xaml.Controls;
#endif

#if ANDROID
using Android.Views;
#endif

namespace MonoGame.Platform.AndroidGL
{
	public class AndroidGLGraphicsDeviceManager : IGraphicsDeviceManager
	{
		private readonly IAndroidGameActivity mActivity;
		private AndroidGLOrientationSetter mWindowing;
		public IGraphicsDevice GraphicsDevice { get; set; }
		private int _preferredBackBufferHeight;
		private int _preferredBackBufferWidth;
		private SurfaceFormat _preferredBackBufferFormat;
		private DepthFormat _preferredDepthStencilFormat;
		//private bool _preferMultiSampling;
		private DisplayOrientation _supportedOrientations;
		private bool _synchronizedWithVerticalRetrace = true;
		private bool _drawBegun;
		bool disposed;
		private bool _hardwareModeSwitch = true;

		private readonly IGraphicsDeviceQuery mDeviceQuery;
		private readonly ITouchListener mTouchPanel;
		private IGraphicsAdapterCollection mAdapters;
		private IGraphicsDevicePreferences mDevicePreferences;
		private readonly IGraphicsAdapter mGraphicsAdapter;
		private readonly IGraphicsDevice mDevice;
		private readonly IClientWindowBounds mClient;

		public AndroidGLGraphicsDeviceManager(
			IAndroidGameActivity activity,
			AndroidGLOrientationSetter windowing,
			IClientWindowBounds client,

			IBackBufferPreferences backBufferPreferences,
			IPresentationParameters presentationParams,
			IGraphicsAdapterCollection adapters,
			IGraphicsDevicePreferences devicePreferences,
			ITouchListener touchPanel,
			IGraphicsDeviceQuery deviceQuery,
			IGraphicsDevice device,
			IGraphicsProfiler profiler
			)
		{
			mClient = client;
			mPresentationParameters = presentationParams;
			mDevicePreferences = devicePreferences;

			mActivity = activity;
			mWindowing = windowing;
			mAdapters = adapters;
			mTouchPanel = touchPanel;
			mDeviceQuery = deviceQuery;

			mDevice = device;

			if (mAdapters.Options.Length < 1)
			{
				throw new InvalidOperationException ("No adapters were provided");
			}
			mGraphicsAdapter = mAdapters.Options[0];

			_supportedOrientations = DisplayOrientation.Default;

			_preferredBackBufferHeight = backBufferPreferences.DefaultBackBufferHeight;
			_preferredBackBufferWidth = backBufferPreferences.DefaultBackBufferWidth;

			_preferredBackBufferFormat = SurfaceFormat.Color;
			_preferredDepthStencilFormat = DepthFormat.Depth24;
			_synchronizedWithVerticalRetrace = true;

			GraphicsProfile = profiler.GetHighestSupportedGraphicsProfile();
			//GraphicsProfile = GraphicsProfile.HiDef;

		}

		~AndroidGLGraphicsDeviceManager()
		{
			Dispose(false);
		}

		public void CreateDevice()
		{
			Initialize();

			OnDeviceCreated(EventArgs.Empty);
		}

		public bool BeginDraw()
		{
			if (GraphicsDevice == null)
				return false;

			_drawBegun = true;
			return true;
		}

		public void EndDraw()
		{
			if (GraphicsDevice != null && _drawBegun)
			{
				_drawBegun = false;
				GraphicsDevice.Present();
			}
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

		#region IDisposable Members

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!disposed)
			{
				if (disposing)
				{
					if (GraphicsDevice != null)
					{
						GraphicsDevice.Dispose();
						GraphicsDevice = null;
					}
				}
				disposed = true;
			}
		}

		#endregion

		public void ApplyChanges()
		{
			// Calling ApplyChanges() before CreateDevice() should have no effect
			if (GraphicsDevice == null)
				return;

			// Trigger a change in orientation in case the supported orientations have changed
			mWindowing.SetOrientation(mWindowing.CurrentOrientation);

			// Ensure the presentation parameter orientation and buffer size matches the window
			mPresentationParameters.DisplayOrientation = mWindowing.CurrentOrientation;

			// Set the presentation parameters' actual buffer size to match the orientation
			bool isLandscape = (0 != (mWindowing.CurrentOrientation & (DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight)));
			int w = mDeviceQuery.PreferredBackBufferWidth;
			int h = mDeviceQuery.PreferredBackBufferHeight;

			mPresentationParameters.BackBufferWidth = isLandscape ? Math.Max(w, h) : Math.Min(w, h);
			mPresentationParameters.BackBufferHeight = isLandscape ? Math.Min(w, h) : Math.Max(w, h);

			ResetClientBounds();

			// Set the new display size on the touch panel.
			//
			// TODO: In XNA this seems to be done as part of the 
			// GraphicsDevice.DeviceReset event... we need to get 
			// those working.
			//
			mTouchPanel.DisplayWidth = GraphicsDevice.PresentationParameters.BackBufferWidth;
			mTouchPanel.DisplayHeight = GraphicsDevice.PresentationParameters.BackBufferHeight;

		}

		private IPresentationParameters mPresentationParameters;
		private void Initialize()
		{

			mPresentationParameters.DepthStencilFormat = DepthFormat.Depth24;

			// TODO: Implement multisampling (aka anti-alising) for all platforms!
			if (PreparingDeviceSettings != null)
			{
				var gdi = new GraphicsDeviceInformation();
				gdi.GraphicsProfile = GraphicsProfile; // Microsoft defaults this to Reach.
				gdi.Adapter = mAdapters.Options[0];
				gdi.PresentationParameters = mPresentationParameters;
				var pe = new PreparingDeviceSettingsEventArgs(gdi);
				PreparingDeviceSettings(this, pe);
				mPresentationParameters = pe.GraphicsDeviceInformation.PresentationParameters;
				GraphicsProfile = pe.GraphicsDeviceInformation.GraphicsProfile;
			}

			// Needs to be before ApplyChanges()
			// TODO : add graphicsdevice back
			//_graphicsDevice = new GraphicsDevice(mDevicePlatform, mSamplerStateCollectionPlatform, mTextureCollectionPlatform, GraphicsAdapter.DefaultAdapter, GraphicsProfile, presentationParameters);

			ApplyChanges();

			// Set the new display size on the touch panel.
			//
			// TODO: In XNA this seems to be done as part of the 
			// GraphicsDevice.DeviceReset event... we need to get 
			// those working.
			//
			mTouchPanel.DisplayWidth = mPresentationParameters.BackBufferWidth;
			mTouchPanel.DisplayHeight = mPresentationParameters.BackBufferHeight;
			mTouchPanel.DisplayOrientation = mPresentationParameters.DisplayOrientation;
		}

		public void ToggleFullScreen()
		{
			IsFullScreen = !IsFullScreen;
		}

		public GraphicsProfile GraphicsProfile { get; set; }

		private bool _wantFullScreen = false;
		public bool IsFullScreen
		{
			get
			{
				if (GraphicsDevice != null)
					return GraphicsDevice.PresentationParameters.IsFullScreen;
				return _wantFullScreen;
			}
			set
			{
				_wantFullScreen = value;
				if (GraphicsDevice != null)
				{
					GraphicsDevice.PresentationParameters.IsFullScreen = value;
					ForceSetFullScreen();
				}
			}
		}

		internal void ForceSetFullScreen()
		{
			mActivity.ForceFullScreen (IsFullScreen);
		}

		/// <summary>
		/// Gets or sets the boolean which defines how window switches from windowed to fullscreen state.
		/// "Hard" mode(true) is slow to switch, but more effecient for performance, while "soft" mode(false) is vice versa.
		/// The default value is <c>true</c>. Can only be changed before graphics device is created (in game's constructor).
		/// </summary>
		public bool HardwareModeSwitch
		{
			get { return _hardwareModeSwitch;}
			set
			{
				if (GraphicsDevice == null) _hardwareModeSwitch = value;
				else throw new InvalidOperationException("This property can only be changed before graphics device is created(in game constructor).");
			}
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

		public SurfaceFormat PreferredBackBufferFormat
		{
			get
			{
				return _preferredBackBufferFormat;
			}
			set
			{
				_preferredBackBufferFormat = value;
			}
		}

//	public int PreferredBackBufferHeight
//	{
//		get
//		{
//			return _preferredBackBufferHeight;
//		}
//		set
//		{
//			_preferredBackBufferHeight = value;
//		}
//	}
//
//	public int PreferredBackBufferWidth
//	{
//		get
//		{
//			return _preferredBackBufferWidth;
//		}
//		set
//		{
//			_preferredBackBufferWidth = value;
//		}
//	}

		public DepthFormat PreferredDepthStencilFormat
		{
			get
			{
				return _preferredDepthStencilFormat;
			}
			set
			{
				_preferredDepthStencilFormat = value;
			}
		}

		public bool SynchronizeWithVerticalRetrace
		{
			get
			{
				return _synchronizedWithVerticalRetrace;
			}
			set
			{
				_synchronizedWithVerticalRetrace = value;
			}
		}

		public DisplayOrientation SupportedOrientations
		{
			get
			{
				return _supportedOrientations;
			}
			set
			{
				_supportedOrientations = value;
				if (mActivity != null)
					mWindowing.SetSupportedOrientations(_supportedOrientations);
			}
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
		public void ResetClientBounds()
		{
			float preferredAspectRatio = (float) mDeviceQuery.PreferredBackBufferWidth /
				(float)mDeviceQuery.PreferredBackBufferHeight;
			float displayAspectRatio = (float) mGraphicsAdapter.CurrentDisplayMode.Width / 
				(float)mGraphicsAdapter.CurrentDisplayMode.Height;

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
				newClientBounds.Height = mGraphicsAdapter.CurrentDisplayMode.Height;
				newClientBounds.Width = (int)(newClientBounds.Height * adjustedAspectRatio);
				newClientBounds.X = (mGraphicsAdapter.CurrentDisplayMode.Width - newClientBounds.Width) / 2;
			}
			else if (displayAspectRatio < (adjustedAspectRatio - EPSILON))
			{
			// Fill the entire width and reduce the height to keep aspect ratio
				newClientBounds.Width = mGraphicsAdapter.CurrentDisplayMode.Width;
			newClientBounds.Height = (int)(newClientBounds.Width / adjustedAspectRatio);
				newClientBounds.Y = (mGraphicsAdapter.CurrentDisplayMode.Height - newClientBounds.Height) / 2;
			}
			else
			{
				// Set the ClientBounds to match the DisplayMode
				newClientBounds.Width = mGraphicsAdapter.CurrentDisplayMode.Width;
				newClientBounds.Height = mGraphicsAdapter.CurrentDisplayMode.Height;
			}

			// Ensure buffer size is reported correctly
			mPresentationParameters.BackBufferWidth = newClientBounds.Width;
			mPresentationParameters.BackBufferHeight = newClientBounds.Height;

			// Set the veiwport so the (potentially) resized client bounds are drawn in the middle of the screen
			mDevice.Viewport = new Viewport(newClientBounds.X, -newClientBounds.Y, newClientBounds.Width, newClientBounds.Height);

			mClient.ChangeClientBounds(newClientBounds);

			// Touch panel needs latest buffer size for scaling
			mTouchPanel.DisplayWidth = newClientBounds.Width;
			mTouchPanel.DisplayHeight = newClientBounds.Height;

			Android.Util.Log.Debug("MonoGame", "GraphicsDeviceManager.ResetClientBounds: newClientBounds=" + newClientBounds.ToString());
		}

	}
}
