using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Magnesium;
using MonoGame.Core;
using MonoGame.Platform.DesktopGL;
using MonoGame.Graphics;

namespace HelloMagnesium
{
	public class MgGraphicsDeviceManager : IMgGraphicsDeviceManager
	{
		IPresentationParameters mPresentationParameters;
		readonly IMgThreadPartition mPartition;
		readonly IMgSwapchainCollection mSwapchainCollection;

		bool _synchronizedWithVerticalRetrace;

		IGraphicsAdapterCollection mAdapters;

		GraphicsProfile mGraphicsProfile;

		IOpenTKWindowResetter mWindowResetter;

		private IHelloGraphicsDevice mDevice;
		public IMgGraphicsDevice Device {
			get {
				return mDevice;
			}
		}

		public MgGraphicsDeviceManager (
			IHelloGraphicsDevice device,
			IMgThreadPartition partition,
			IPresentationParameters presentationParameters,
			IMgSwapchainCollection swapchainCollection,
			IGraphicsAdapterCollection adapters,
			IGraphicsProfiler profiler,
			IOpenTKWindowResetter windowReset
		)
		{
			mDevice = device;
			mPartition = partition;
			mPresentationParameters = presentationParameters;
			mSwapchainCollection = swapchainCollection;
			mAdapters = adapters;
			mWindowResetter = windowReset;

			if (mAdapters.Options.Length < 1)
			{
				throw new InvalidOperationException ("No adapters were provided");
			}

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

			var width = (uint)mPresentationParameters.BackBufferWidth;
			var height = (uint)mPresentationParameters.BackBufferHeight;

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
		}

		public event EventHandler<PreparingDeviceSettingsEventArgs> PreparingDeviceSettings;

		private void Initialize()
		{

			mPresentationParameters.DepthStencilFormat = DepthFormat.Depth24;

			// TODO: Implement multisampling (aka anti-alising) for all platforms!
			if (PreparingDeviceSettings != null)
			{
				var gdi = new GraphicsDeviceInformation();
				gdi.GraphicsProfile = mGraphicsProfile; // Microsoft defaults this to Reach.
				gdi.Adapter = mAdapters.Options[0];
				gdi.PresentationParameters = mPresentationParameters;
				var pe = new PreparingDeviceSettingsEventArgs(gdi);
				PreparingDeviceSettings(this, pe);
				mPresentationParameters = pe.GraphicsDeviceInformation.PresentationParameters;
				mGraphicsProfile = pe.GraphicsDeviceInformation.GraphicsProfile;
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
//			mTouchPanel.DisplayWidth = mPresentationParameters.BackBufferWidth;
//			mTouchPanel.DisplayHeight = mPresentationParameters.BackBufferHeight;
//			mTouchPanel.DisplayOrientation = mPresentationParameters.DisplayOrientation;
		}

		public void ResetClientBounds ()
		{
			
		}

		public void ApplyChanges()
		{
			// Calling ApplyChanges() before CreateDevice() should have no effect
			if (GraphicsDevice == null)
				return;

			mWindowResetter.ResetWindowBounds();

			// TODO : figure out where to reset set intervals
			//Set the swap interval based on if vsync is desired or not.
			//See GetSwapInterval for more details
//			int swapInterval;
//			if (_synchronizedWithVerticalRetrace)
//				swapInterval = mDeviceQuery.GetSwapInterval(mPresentationParameters.PresentationInterval);
//			else
//				swapInterval = 0;

			// TODO : figure this out somehow
			//GraphicsDevice.Context.SwapInterval = swapInterval;

			// Set the new display size on the touch panel.
			//
			//
//			mTouchPanel.DisplayWidth = GraphicsDevice.PresentationParameters.BackBufferWidth;
//			mTouchPanel.DisplayHeight = GraphicsDevice.PresentationParameters.BackBufferHeight;

		}

		public void ToggleFullScreen ()
		{
			throw new NotImplementedException ();
		}

		public IGraphicsDevice GraphicsDevice {
			get {
				return mDevice;
			}
		}

		public DisplayOrientation SupportedOrientations {
			get;
			set;
		}

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

		#endregion

		#region IDisposable implementation

		public void Dispose ()
		{

		}

		#endregion
	}
}

