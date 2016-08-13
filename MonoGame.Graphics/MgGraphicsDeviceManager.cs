using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Magnesium;
using MonoGame.Core;
using MonoGame.Graphics;

namespace MonoGame.Graphics
{
	public abstract class MgGraphicsDeviceManager : IGraphicsDeviceManager
	{
		protected IPresentationParameters PresentationParameters { get; private set; }
		readonly IMgThreadPartition mPartition;
		readonly IMgSwapchainCollection mSwapchainCollection;

		bool _synchronizedWithVerticalRetrace;

		IGraphicsAdapterCollection mAdapters;

		GraphicsProfile mGraphicsProfile;

		private IMgGraphicsDevice mDevice;
		public IMgGraphicsDevice Device {
			get {
				return mDevice;
			}
		}

		protected IGraphicsAdapter GraphicsAdapter { get; private set; }

		protected MgGraphicsDeviceManager (
			IMgGraphicsDevice device,
			IMgThreadPartition partition,
			IPresentationParameters presentationParameters,
			IMgSwapchainCollection swapchainCollection,
			IGraphicsAdapterCollection adapters,
			IGraphicsProfiler profiler
		)
		{
			mDevice = device;
			mPartition = partition;
			PresentationParameters = presentationParameters;
			mSwapchainCollection = swapchainCollection;
			mAdapters = adapters;

			if (mAdapters.Options.Length < 1)
			{
				throw new InvalidOperationException ("No adapters were provided");
			}
			GraphicsAdapter = mAdapters.Options[0];

			SupportedOrientations = DisplayOrientation.Default;

//			_preferredBackBufferHeight = backBufferPreferences.DefaultBackBufferHeight;
//			_preferredBackBufferWidth = backBufferPreferences.DefaultBackBufferWidth;

//			_preferredBackBufferFormat = SurfaceFormat.Color;
			PreferredDepthStencilFormat = DepthFormat.Depth24;
			_synchronizedWithVerticalRetrace = true;

			mGraphicsProfile = profiler.GetHighestSupportedGraphicsProfile();
		}

		#region IGraphicsDeviceManager implementation

		bool _drawBegun = false;
		public bool BeginDraw ()
		{
			if (GraphicsDevice == null)
				return false;

			_drawBegun = true;

			return true;
		}

		public void EndDraw ()
		{	
			if (GraphicsDevice != null && _drawBegun)
			{
				// PRESENT
				_drawBegun = false;
			}
		}


		public void CreateDevice ()
		{
			Initialize ();

			var width = (uint)PresentationParameters.BackBufferWidth;
			var height = (uint)PresentationParameters.BackBufferHeight;

			const int NO_OF_BUFFERS = 1;
			IMgCommandBuffer[] buffers = new IMgCommandBuffer[NO_OF_BUFFERS];
			var pAllocateInfo = new MgCommandBufferAllocateInfo {
				CommandBufferCount = NO_OF_BUFFERS,
				CommandPool = mPartition.CommandPool,
				Level = MgCommandBufferLevel.PRIMARY,
			};

			mPartition.Device.AllocateCommandBuffers (pAllocateInfo, buffers);

			var dsCreateInfo = new MgGraphicsDeviceCreateInfo
			{
				Command = buffers[0],
				Color = MgFormat.R8G8B8A8_UINT,
				DepthStencil = MgFormat.D24_UNORM_S8_UINT,
				Width = width,
				Height = height,
				Samples = MgSampleCountFlagBits.COUNT_1_BIT,
				Swapchains = mSwapchainCollection,
			};
			mDevice.Create(dsCreateInfo);

			var submitInfo = new MgSubmitInfo {				
				CommandBuffers = buffers,
			};

			var queue = mPartition.Queue;
			queue.QueueSubmit(new[] {submitInfo}, null);
			queue.QueueWaitIdle ();

			Viewport = new Viewport {
				X = (int) mDevice.CurrentViewport.X,
				Y = (int) mDevice.CurrentViewport.Y,
				Width = (int) mDevice.CurrentViewport.Width,
				Height = (int) mDevice.CurrentViewport.Height,
				MinDepth = mDevice.CurrentViewport.MinDepth,
				MaxDepth = mDevice.CurrentViewport.MaxDepth,
			};

			AfterDeviceCreation ();
		}

		public Viewport Viewport {
			get;
			set;
		}

		protected abstract void AfterDeviceCreation ();

		public event EventHandler<PreparingDeviceSettingsEventArgs> PreparingDeviceSettings;

		private void Initialize()
		{
			PresentationParameters.DepthStencilFormat = DepthFormat.Depth24;

			// TODO: Implement multisampling (aka anti-alising) for all platforms!
			if (PreparingDeviceSettings != null)
			{
				var gdi = new GraphicsDeviceInformation();
				gdi.GraphicsProfile = mGraphicsProfile; // Microsoft defaults this to Reach.
				gdi.Adapter = GraphicsAdapter;
				gdi.PresentationParameters = PresentationParameters;
				var pe = new PreparingDeviceSettingsEventArgs(gdi);
				PreparingDeviceSettings(this, pe);
				PresentationParameters = pe.GraphicsDeviceInformation.PresentationParameters;
				mGraphicsProfile = pe.GraphicsDeviceInformation.GraphicsProfile;
			}

			ApplyChanges();
		}

		public abstract void ResetClientBounds ();
		protected abstract void ApplyLocalChanges ();

		public void ApplyChanges()
		{
			// Calling ApplyChanges() before CreateDevice() should have no effect
			if (GraphicsDevice == null)
				return;

			ApplyLocalChanges ();
		}

		public void ToggleFullScreen()
		{
			IsFullScreen = !IsFullScreen;
		}

		public IMgGraphicsDevice GraphicsDevice {
			get {
				return mDevice;
			}
		}

		DisplayOrientation mSupportedOrientations;
		public DisplayOrientation SupportedOrientations {
			get {
				return mSupportedOrientations;
			}
			set {
				mSupportedOrientations = value;
				ApplySupportedOrientation (value);
			}
		}

		protected abstract void ApplySupportedOrientation (DisplayOrientation value);


		public DepthFormat PreferredDepthStencilFormat {
			get;
			set;
		}

		private bool _wantFullScreen = false;
		public bool IsFullScreen
		{
			get
			{
				if (GraphicsDevice != null)
					return PresentationParameters.IsFullScreen;
				return _wantFullScreen;
			}
			set
			{
				_wantFullScreen = value;
				if (GraphicsDevice != null)
				{
					PresentationParameters.IsFullScreen = value;
					ForceSetFullScreen ();
				}
			}
		}

		protected abstract void ForceSetFullScreen ();

		private bool _hardwareModeSwitch = true;

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

		#endregion

		#region IDisposable Members

		~MgGraphicsDeviceManager()
		{
			Dispose (false);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		private bool mDisposed = false;
		protected virtual void Dispose(bool disposing)
		{
			if (mDisposed)
				return;

			if (mDevice != null)
			{
				mDevice.Dispose();
				mDevice = null;
			}
			ReleaseUnmanagedResources ();

			if (disposing)
			{
				ReleaseManagedResources ();
			}

			mDisposed = true;

		}

		protected abstract void ReleaseManagedResources ();

		protected abstract void ReleaseUnmanagedResources ();

		#endregion
	}
}

