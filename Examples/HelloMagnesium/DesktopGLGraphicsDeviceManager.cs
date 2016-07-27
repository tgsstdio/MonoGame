// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using MonoGame.Platform.DesktopGL;
using MonoGame.Core;

namespace HelloMagnesium
{
	public class DesktopGLGraphicsDeviceManager : IGraphicsDeviceManager
	{
		private IOpenTKGameWindow mWindow;
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

		private ITouchListener mTouchPanel;
		private IOpenTKWindowResetter mWindowReset;
		private IGraphicsDevicePlatform mDevicePlatform;
		private IOpenTKDeviceQuery mDeviceQuery;
		private IGraphicsAdapterCollection mAdapters;
		private IGraphicsDevicePreferences mDevicePreferences;
		private IGraphicsDeviceService mDeviceService;
		public DesktopGLGraphicsDeviceManager(
			IOpenTKGameWindow window,
			IOpenTKWindowResetter windowReset,
			IGraphicsDevicePlatform devicePlatform,
			IOpenTKDeviceQuery deviceQuery,
			IBackBufferPreferences backBufferPreferences,
			IPresentationParameters presentationParams,
			IGraphicsAdapterCollection adapters,
			IGraphicsDevicePreferences devicePreferences,
			ITouchListener touchPanel,
			IGraphicsDeviceService deviceService,
			IGraphicsProfiler profiler
			)
		{
			mWindowReset = windowReset;
			mDevicePlatform = devicePlatform;
			mDeviceQuery = deviceQuery;
			mPresentationParameters = presentationParams;
			mDevicePreferences = devicePreferences;

			mWindow = window;
			mAdapters = adapters;
			mTouchPanel = touchPanel;
			mDeviceService = deviceService;

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

			GraphicsProfile = profiler.GetHighestSupportedGraphicsProfile();
		}

		~DesktopGLGraphicsDeviceManager()
		{
			Dispose(false);
		}

		public void CreateDevice()
		{
			Initialize();

			mDeviceService.OnDeviceCreated(this, EventArgs.Empty);
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

		mWindowReset.ResetWindowBounds();

		//Set the swap interval based on if vsync is desired or not.
		//See GetSwapInterval for more details
		int swapInterval;
		if (_synchronizedWithVerticalRetrace)
			swapInterval = mDeviceQuery.GetSwapInterval(mPresentationParameters.PresentationInterval);
		else
			swapInterval = 0;
			
		// TODO : figure this out somehow
		//GraphicsDevice.Context.SwapInterval = swapInterval;

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

		#if !MONOMAC
		ApplyChanges();
		#endif

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
				return mPresentationParameters.IsFullScreen;
			return _wantFullScreen;
		}
		set
		{
			_wantFullScreen = value;
			if (GraphicsDevice != null)
			{
				mPresentationParameters.IsFullScreen = value;
			}
		}
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

	}

}
}
