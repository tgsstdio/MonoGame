// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;

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
	public class AndroidGLGraphicsDeviceManager : IGraphicsDeviceService, IGraphicsDeviceManager
	{
		private IAndroidGLGameWindow mWindow;
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

		#if (WINDOWS || WINDOWS_UAP) && DIRECTX
		private bool _firstLaunch = true;
		#endif

//		#if !WINRT || WINDOWS_UAP
//		private bool _wantFullScreen = false;
//		#endif
//		public int DefaultBackBufferHeight
//		{
//			get {
//				return 480;
//			}
//		}
//
//		public int DefaultBackBufferWidth {
//			get {
//				return 800;
//			}
//		}
		private readonly IGraphicsDeviceQuery mDeviceQuery;
		private readonly ITouchPanel mTouchPanel;
		private IGraphicsDevicePlatform mDevicePlatform;
	//	private ISamplerStateCollectionPlatform mSamplerStateCollectionPlatform;
	//	private ITextureCollectionPlatform mTextureCollectionPlatform;
		private IGraphicsAdapterCollection mAdapters;
		private IGraphicsDevicePreferences mDevicePreferences;
		public AndroidGLGraphicsDeviceManager(
			IAndroidGLGameWindow window,
			IGraphicsDevicePlatform devicePlatform,
			//ISamplerStateCollectionPlatform samplerStateCollectionPlatform,
			//ITextureCollectionPlatform texCollectionPlatform,
			IBackBufferPreferences backBufferPreferences,
			IPresentationParameters presentationParams,
			IGraphicsAdapterCollection adapters,
			IGraphicsDevicePreferences devicePreferences,
			ITouchPanel touchPanel,
			IGraphicsDeviceQuery deviceQuery
			)
		{
			mDevicePlatform = devicePlatform;
			//mSamplerStateCollectionPlatform = samplerStateCollectionPlatform;
		//	mTextureCollectionPlatform = texCollectionPlatform;
			mPresentationParameters = presentationParams;
			mDevicePreferences = devicePreferences;

			mWindow = window;
			mAdapters = adapters;
			mTouchPanel = touchPanel;
			mDeviceQuery = deviceQuery;

			if (mAdapters.Options.Length < 1)
			{
				throw new InvalidOperationException ("No adapters were provided");
			}

			_supportedOrientations = DisplayOrientation.Default;

			_preferredBackBufferHeight = backBufferPreferences.DefaultBackBufferHeight;
			_preferredBackBufferWidth = backBufferPreferences.DefaultBackBufferWidth;

			_preferredBackBufferFormat = SurfaceFormat.Color;
			_preferredDepthStencilFormat = DepthFormat.Depth24;
			_synchronizedWithVerticalRetrace = true;

			GraphicsProfile = devicePlatform.GetHighestSupportedGraphicsProfile(null);

//			if (_game.Services.GetService(typeof(IGraphicsDeviceManager)) != null)
//				throw new ArgumentException("Graphics Device Manager Already Present");
//
//			_game.Services.AddService(typeof(IGraphicsDeviceManager), this);
//			_game.Services.AddService(typeof(IGraphicsDeviceService), this);
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

			#if WINDOWS_PHONE
			_graphicsDevice.GraphicsProfile = GraphicsProfile;
			// Display orientation is always portrait on WP8
			_graphicsDevice.PresentationParameters.DisplayOrientation = DisplayOrientation.Portrait;
			#elif WINDOWS_STOREAPP || WINDOWS_UAP

			// TODO:  Does this need to occur here?
			_game.Window.SetSupportedOrientations(_supportedOrientations);

			_graphicsDevice.PresentationParameters.BackBufferFormat = _preferredBackBufferFormat;
			_graphicsDevice.PresentationParameters.BackBufferWidth = _preferredBackBufferWidth;
			_graphicsDevice.PresentationParameters.BackBufferHeight = _preferredBackBufferHeight;
			_graphicsDevice.PresentationParameters.DepthStencilFormat = _preferredDepthStencilFormat;

			// TODO: We probably should be resetting the whole device
			// if this changes as we are targeting a different 
			// hardware feature level.
			_graphicsDevice.GraphicsProfile = GraphicsProfile;

			#if WINDOWS_UAP
			_graphicsDevice.PresentationParameters.DeviceWindowHandle = IntPtr.Zero;
			_graphicsDevice.PresentationParameters.SwapChainPanel = this.SwapChainPanel;
			_graphicsDevice.PresentationParameters.IsFullScreen = _wantFullScreen;
			#else
			_graphicsDevice.PresentationParameters.IsFullScreen = false;

			// The graphics device can use a XAML panel or a window
			// to created the default swapchain target.
			if (this.SwapChainBackgroundPanel != null)
			{
			_graphicsDevice.PresentationParameters.DeviceWindowHandle = IntPtr.Zero;
			_graphicsDevice.PresentationParameters.SwapChainBackgroundPanel = this.SwapChainBackgroundPanel;
			}
			else
			{
			_graphicsDevice.PresentationParameters.DeviceWindowHandle = _game.Window.Handle;
			_graphicsDevice.PresentationParameters.SwapChainBackgroundPanel = null;
			}
			#endif
			// Update the back buffer.
			_graphicsDevice.CreateSizeDependentResources();
			_graphicsDevice.ApplyRenderTargets(null);

			#if WINDOWS_UAP
			((UAPGameWindow)_game.Window).SetClientSize(_preferredBackBufferWidth, _preferredBackBufferHeight);
			#endif

			#elif WINDOWS && DIRECTX

			_graphicsDevice.PresentationParameters.BackBufferFormat = _preferredBackBufferFormat;
			_graphicsDevice.PresentationParameters.BackBufferWidth = _preferredBackBufferWidth;
			_graphicsDevice.PresentationParameters.BackBufferHeight = _preferredBackBufferHeight;
			_graphicsDevice.PresentationParameters.DepthStencilFormat = _preferredDepthStencilFormat;
			_graphicsDevice.PresentationParameters.PresentationInterval = _synchronizedWithVerticalRetrace ? PresentInterval.Default : PresentInterval.Immediate;
			_graphicsDevice.PresentationParameters.IsFullScreen = _wantFullScreen;

			// TODO: We probably should be resetting the whole 
			// device if this changes as we are targeting a different
			// hardware feature level.
			_graphicsDevice.GraphicsProfile = GraphicsProfile;

			_graphicsDevice.PresentationParameters.DeviceWindowHandle = _game.Window.Handle;

			// Update the back buffer.
			_graphicsDevice.CreateSizeDependentResources();
			_graphicsDevice.ApplyRenderTargets(null);

			((MonoGame.Framework.WinFormsGamePlatform)_game.Platform).ResetWindowBounds();

			#elif DESKTOPGL
			mWindowReset.ResetWindowBounds();

		//Set the swap interval based on if vsync is desired or not.
		//See GetSwapInterval for more details
		int swapInterval;
		if (_synchronizedWithVerticalRetrace)
			swapInterval = GraphicsDevice.PresentationParameters.PresentationInterval.GetSwapInterval();
		else
			swapInterval = 0;
			
			// TODO : figure this out somehow
			//GraphicsDevice.Context.SwapInterval = swapInterval;
			#elif MONOMAC
			_graphicsDevice.PresentationParameters.IsFullScreen = _wantFullScreen;

			// TODO: Implement multisampling (aka anti-alising) for all platforms!

			_game.applyChanges(this);
			#else

			#if ANDROID
			// Trigger a change in orientation in case the supported orientations have changed
			mWindow.SetOrientation(mWindow.CurrentOrientation, false);
			#endif
			// Ensure the presentation parameter orientation and buffer size matches the window
			mPresentationParameters.DisplayOrientation = mWindow.CurrentOrientation;

			// Set the presentation parameters' actual buffer size to match the orientation
			bool isLandscape = (0 != (mWindow.CurrentOrientation & (DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight)));
			int w = mDeviceQuery.PreferredBackBufferWidth;
			int h = mDeviceQuery.PreferredBackBufferHeight;

			mPresentationParameters.BackBufferWidth = isLandscape ? Math.Max(w, h) : Math.Min(w, h);
			mPresentationParameters.BackBufferHeight = isLandscape ? Math.Min(w, h) : Math.Max(w, h);

			ResetClientBounds();
			#endif

			// Set the new display size on the touch panel.
			//
			// TODO: In XNA this seems to be done as part of the 
			// GraphicsDevice.DeviceReset event... we need to get 
			// those working.
			//
			mTouchPanel.DisplayWidth = GraphicsDevice.PresentationParameters.BackBufferWidth;
			mTouchPanel.DisplayHeight = GraphicsDevice.PresentationParameters.BackBufferHeight;

			#if (WINDOWS || WINDOWS_UAP) && DIRECTX

			if (!_firstLaunch)
			{
			if (IsFullScreen)
			{
			_game.Platform.EnterFullScreen();
			}
			else
			{
			_game.Platform.ExitFullScreen();
			}
			}
			_firstLaunch = false;
			#endif
			}

		private IPresentationParameters mPresentationParameters;
		private void Initialize()
		{

			mPresentationParameters.DepthStencilFormat = DepthFormat.Depth24;

			#if (WINDOWS || WINRT) && !DESKTOPGL
			_game.Window.SetSupportedOrientations(_supportedOrientations);

			presentationParameters.BackBufferFormat = _preferredBackBufferFormat;
			presentationParameters.BackBufferWidth = _preferredBackBufferWidth;
			presentationParameters.BackBufferHeight = _preferredBackBufferHeight;
			presentationParameters.DepthStencilFormat = _preferredDepthStencilFormat;
			presentationParameters.IsFullScreen = false;

			#if WINDOWS_PHONE
			// Nothing to do!
			#elif WINDOWS_UAP
			presentationParameters.DeviceWindowHandle = IntPtr.Zero;
			presentationParameters.SwapChainPanel = this.SwapChainPanel;
			#elif WINDOWS_STORE
			// The graphics device can use a XAML panel or a window
			// to created the default swapchain target.
			if (this.SwapChainBackgroundPanel != null)
			{
			presentationParameters.DeviceWindowHandle = IntPtr.Zero;
			presentationParameters.SwapChainBackgroundPanel = this.SwapChainBackgroundPanel;
			}
			else
			{
			presentationParameters.DeviceWindowHandle = _game.Window.Handle;
			presentationParameters.SwapChainBackgroundPanel = null;
			}
			#else
			presentationParameters.DeviceWindowHandle = _game.Window.Handle;
			#endif

			#else

			// SEE DesktopGLGraphicsDeviceQuery
//			#if MONOMAC
//			presentationParameters.IsFullScreen = _wantFullScreen;
//			#elif DESKTOPGL
//			mPresentationParameters.IsFullScreen = _wantFullScreen;
//			#else
//			// Set "full screen"  as default
//			presentationParameters.IsFullScreen = true;
//			#endif // MONOMAC

			#endif // WINDOWS || WINRT

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

		#if !MONOMAC
		ApplyChanges();
		#endif

		// Set the new display size on the touch panel.
		//
		// TODO: In XNA this seems to be done as part of the 
		// GraphicsDevice.DeviceReset event... we need to get 
		// those working.
		//
		mTouchPanel.DisplayWidth = GraphicsDevice.PresentationParameters.BackBufferWidth;
		mTouchPanel.DisplayHeight = GraphicsDevice.PresentationParameters.BackBufferHeight;
		mTouchPanel.DisplayOrientation = GraphicsDevice.PresentationParameters.DisplayOrientation;
	}

//	public void ToggleFullScreen()
//	{
//		IsFullScreen = !IsFullScreen;
//
//		#if (WINDOWS || WINDOWS_UAP) && DIRECTX
//		ApplyChanges();
//		#endif
//	}

	#if WINDOWS_STOREAPP
	[CLSCompliant(false)]
	public SwapChainBackgroundPanel SwapChainBackgroundPanel { get; set; }
	#endif

	#if WINDOWS_UAP
	[CLSCompliant(false)]
	public SwapChainPanel SwapChainPanel { get; set; }
	#endif

	public GraphicsProfile GraphicsProfile { get; set; }

//	public bool IsFullScreen
//	{
//		get
//		{
//			#if WINDOWS_UAP
//			return _wantFullScreen;
//			#elif WINRT
//			return true;
//			#else
//			if (GraphicsDevice != null)
//				return GraphicsDevice.PresentationParameters.IsFullScreen;
//			return _wantFullScreen;
//			#endif
//		}
//		set
//		{
//			#if WINDOWS_UAP
//			_wantFullScreen = value;
//			#elif WINRT
//			// Just ignore this as it is not relevant on Windows 8
//			#elif WINDOWS && DIRECTX
//			_wantFullScreen = value;
//			#else
//			_wantFullScreen = value;
//			if (GraphicsDevice != null)
//			{
//				GraphicsDevice.PresentationParameters.IsFullScreen = value;
//			#if ANDROID
//			ForceSetFullScreen();
//			#endif
//			}
//			#endif
//		}
//	}

	#if ANDROID
	internal void ForceSetFullScreen()
	{
	if (mDeviceQuery.IsFullScreen)
	{
			mActivity.ClearFlags(Android.Views.WindowManagerFlags.ForceNotFullscreen);
			mActivity.SetFlags(WindowManagerFlags.Fullscreen, WindowManagerFlags.Fullscreen);
	}
	else
			mActivity.SetFlags(WindowManagerFlags.ForceNotFullscreen, WindowManagerFlags.ForceNotFullscreen);
	}
	#endif

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
			if (mWindow != null)
				mWindow.SetSupportedOrientations(_supportedOrientations);
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
		#if ANDROID
		float preferredAspectRatio = (float) mDeviceQuery.PreferredBackBufferWidth /
			(float)mDeviceQuery.PreferredBackBufferHeight;
		float displayAspectRatio = (float)GraphicsDevice.DisplayMode.Width / 
		(float)GraphicsDevice.DisplayMode.Height;

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
		newClientBounds.Height = _graphicsDevice.DisplayMode.Height;
		newClientBounds.Width = (int)(newClientBounds.Height * adjustedAspectRatio);
		newClientBounds.X = (_graphicsDevice.DisplayMode.Width - newClientBounds.Width) / 2;
		}
		else if (displayAspectRatio < (adjustedAspectRatio - EPSILON))
		{
		// Fill the entire width and reduce the height to keep aspect ratio
		newClientBounds.Width = _graphicsDevice.DisplayMode.Width;
		newClientBounds.Height = (int)(newClientBounds.Width / adjustedAspectRatio);
		newClientBounds.Y = (_graphicsDevice.DisplayMode.Height - newClientBounds.Height) / 2;
		}
		else
		{
		// Set the ClientBounds to match the DisplayMode
		newClientBounds.Width = GraphicsDevice.DisplayMode.Width;
		newClientBounds.Height = GraphicsDevice.DisplayMode.Height;
		}

		// Ensure buffer size is reported correctly
		mPresentationParameters.BackBufferWidth = newClientBounds.Width;
		mPresentationParameters.BackBufferHeight = newClientBounds.Height;

		// Set the veiwport so the (potentially) resized client bounds are drawn in the middle of the screen
		_graphicsDevice.Viewport = new Viewport(newClientBounds.X, -newClientBounds.Y, newClientBounds.Width, newClientBounds.Height);

		mWindow.ChangeClientBounds(newClientBounds);

		// Touch panel needs latest buffer size for scaling
		mTouchPanel.DisplayWidth = newClientBounds.Width;
		mTouchPanel.DisplayHeight = newClientBounds.Height;

		Android.Util.Log.Debug("MonoGame", "GraphicsDeviceManager.ResetClientBounds: newClientBounds=" + newClientBounds.ToString());
		#endif
	}

}
}
