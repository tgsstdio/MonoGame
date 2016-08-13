using System;
using Magnesium;
using OpenTK.Graphics;
using OpenTK;
using Magnesium.OpenGL;
using System.Diagnostics;

namespace Magnesium.OpenGL
{
	public class OpenTKGraphicsDevice : IMgGraphicsDevice
	{
		#region IMgDepthStencilBuffer implementation

		public IMgRenderPass Renderpass {
			get {
				return mRenderpass;
			}
		}

		GLNullImageView mView;

		INativeWindow mWindow;

		internal IGraphicsContext Context { get; private set; }

		IGLExtensionLookup mExtensions;

		IGLDevicePlatform mGLPlatform;

		IMgGraphicsDeviceLogger mLogger;

		IMgThreadPartition mPartition;

		IGLFramebufferHelperSelector mSelector;

		IGLQueueRenderer mQueueRenderer;

		public OpenTKGraphicsDevice (
			INativeWindow window,
			IMgThreadPartition partition,
			IGLFramebufferHelperSelector selector,
			IGLExtensionLookup extensions,
			IGLDevicePlatform glPlatform,
			IMgGraphicsDeviceLogger logger,
			IGLQueueRenderer queueRenderer
		)
		{
			mPartition = partition;
			mView = new GLNullImageView ();
			mWindow = window;
			mExtensions = extensions;
			mGLPlatform = glPlatform;
			mLogger = logger;
			mSelector = selector;
			mQueueRenderer = queueRenderer;
		}

		public IMgImageView View {
			get {
				return mView;
			}
		}

		private IMgFramebuffer[] mFramebuffers;
		public IMgFramebuffer[] Framebuffers {
			get {
				return mFramebuffers;
			}
		}

//		public void CreateDevice (IGraphicsAdapter adapter, GraphicsProfile graphicsProfile)
//		{
//			
//		}

		private ColorFormat GetColorFormat(Magnesium.MgFormat format)
		{
			switch (format)
			{
			case MgFormat.R8_UNORM:
			case MgFormat.R8_SINT:				
			case MgFormat.R8_UINT:			
				return new ColorFormat(8, 0, 0, 0);
			case MgFormat.R8G8_UNORM:
			case MgFormat.R8G8_SINT:				
			case MgFormat.R8G8_UINT:
				return new ColorFormat(8, 8, 0, 0);

			case MgFormat.B5G6R5_UNORM_PACK16:
				return new ColorFormat(5, 6, 5, 0);
			case MgFormat.B4G4R4A4_UNORM_PACK16:
				return new ColorFormat(4, 4, 4, 4);
			case MgFormat.B5G5R5A1_UNORM_PACK16:
				return new ColorFormat(5, 5, 5, 1);

			case MgFormat.B8G8R8_UINT:
			case MgFormat.B8G8R8_SINT:			
			case MgFormat.R8G8B8_UINT:
			case MgFormat.R8G8B8_SINT:
			case MgFormat.R8G8B8_SRGB:				
			case MgFormat.B8G8R8_SRGB:
			case MgFormat.B8G8R8_UNORM:
			case MgFormat.R8G8B8_UNORM:	
				return new ColorFormat(8, 8, 8, 0);

			case MgFormat.B8G8R8A8_UNORM:
			case MgFormat.R8G8B8A8_UNORM:
			case MgFormat.R8G8B8A8_SRGB:
			case MgFormat.B8G8R8A8_SRGB:
			case MgFormat.R8G8B8A8_UINT:
			case MgFormat.R8G8B8A8_SINT:
			case MgFormat.B8G8R8A8_UINT:
			case MgFormat.B8G8R8A8_SINT:				
				return new ColorFormat(8, 8, 8, 8);

			case MgFormat.A2B10G10R10_UNORM_PACK32:
				return new ColorFormat(10, 10, 10, 2);

			default:
				// Floating point backbuffers formats could be implemented
				// but they are not typically used on the backbuffer. In
				// those cases it is better to create a render target instead.
				throw new NotSupportedException();
			}
		}

		int GetDepthBit (MgFormat format)
		{
			switch (format)
			{
			case MgFormat.D16_UNORM:
			case MgFormat.D16_UNORM_S8_UINT:
				return 16;
			case MgFormat.D24_UNORM_S8_UINT:
				return 24;
			case MgFormat.D32_SFLOAT:	
			case MgFormat.D32_SFLOAT_S8_UINT:				
				return 32;
			default:
				throw new NotSupportedException ();
			}
		}

		int GetStencilBit (MgFormat format)
		{
			switch (format)
			{
			case MgFormat.D16_UNORM:
			case MgFormat.D32_SFLOAT:				
				return 0;
			case MgFormat.D16_UNORM_S8_UINT:
			case MgFormat.D24_UNORM_S8_UINT:
			case MgFormat.D32_SFLOAT_S8_UINT:
				return 8;
			default:
				throw new NotSupportedException ();
			}
		}

		void SetupContext (MgGraphicsDeviceCreateInfo createInfo)
		{
			GraphicsMode mode;
			var wnd = mWindow.WindowInfo;
			// Create an OpenGL compatibility context
			var flags = GraphicsContextFlags.Default;
			int major = 1;
			int minor = 0;
			if (Context == null || Context.IsDisposed)
			{
				var color = GetColorFormat (createInfo.Color);
				var depthBit = GetDepthBit (createInfo.DepthStencil);
				var stencilBit = GetStencilBit (createInfo.DepthStencil);
				var samples = (int)createInfo.Samples;
				if (samples == 0)
				{
					// Use a default of 4x samples if PreferMultiSampling is enabled
					// without explicitly setting the desired MultiSampleCount.
					samples = 4;
				}
				mode = new GraphicsMode (color, depthBit, stencilBit, samples);
				try
				{
					Context = new GraphicsContext (mode, wnd, major, minor, flags);
				}
				catch (Exception e)
				{
					mLogger.Log (string.Format ("Failed to create OpenGL context, retrying. Error: {0}", e));
					major = 1;
					minor = 0;
					flags = GraphicsContextFlags.Default;
					Context = new GraphicsContext (mode, wnd, major, minor, flags);
				}
			}
			Context.MakeCurrent (wnd);
			(Context as IGraphicsContextInternal).LoadAll ();
			//Context.SwapInterval = mDeviceQuery.GetSwapInterval (mPresentation.PresentationInterval);
			// TODO : background threading 
			// Provide the graphics context for background loading
			// Note: this context should use the same GraphicsMode,
			// major, minor version and flags parameters as the main
			// context. Otherwise, context sharing will very likely fail.
			//			if (Threading.BackgroundContext == null)
			//			{
			//				Threading.BackgroundContext = new GraphicsContext(mode, wnd, major, minor, flags);
			//				Threading.WindowInfo = wnd;
			//				Threading.BackgroundContext.MakeCurrent(null);
			//			}
			Context.MakeCurrent (wnd);

			mExtensions.Initialize ();
			//mCapabilities.Initialize ();
			mGLPlatform.Initialize ();
			mSelector.Initialize();
			mQueueRenderer.SetDefault ();
		}

		GLRenderPass mRenderpass;
		void SetupRenderpass (MgGraphicsDeviceCreateInfo createInfo)
		{
			var attachmentDescriptions = new [] {
				new MgAttachmentDescription {
					Format = createInfo.Color,
					LoadOp = MgAttachmentLoadOp.CLEAR,
					StoreOp = MgAttachmentStoreOp.STORE,
					StencilLoadOp = MgAttachmentLoadOp.DONT_CARE,
					StencilStoreOp = MgAttachmentStoreOp.DONT_CARE,
				},
				new MgAttachmentDescription {
					Format = createInfo.DepthStencil,
					LoadOp = MgAttachmentLoadOp.CLEAR,
					StoreOp = MgAttachmentStoreOp.STORE,
					StencilLoadOp = MgAttachmentLoadOp.CLEAR,
					StencilStoreOp = MgAttachmentStoreOp.STORE,
				},
			};

			mRenderpass = new GLRenderPass (attachmentDescriptions);
		}

		void CreateFramebuffers (MgGraphicsDeviceCreateInfo createInfo)
		{
			var frameBuffers = new IMgFramebuffer[createInfo.Swapchains.Buffers.Length];
			for (uint i = 0; i < frameBuffers.Length; i++)
			{
				var frameBufferCreateInfo = new MgFramebufferCreateInfo
				{
					RenderPass = mRenderpass,
					Attachments = new []
					{
						createInfo.Swapchains.Buffers[i].View,
						// Depth/Stencil attachment is the same for all frame buffers
						mView,
					},
					Width = createInfo.Width,
					Height = createInfo.Height,
					Layers = 1,
				};

				var err = mPartition.Device.CreateFramebuffer(frameBufferCreateInfo, null, out frameBuffers[i]);
				Debug.Assert(err == Result.SUCCESS);
			}

			mFramebuffers = frameBuffers;
		}

		public void Create (MgGraphicsDeviceCreateInfo createInfo)
		{	
			if (createInfo == null)
			{
				throw new ArgumentNullException ("createInfo");
			}

			if (createInfo.Command == null)
			{
				throw new ArgumentNullException ("createInfo.Command");
			}

			if (createInfo.Swapchains == null)
			{
				throw new ArgumentNullException ("createInfo.Swapchains");
			}

			ReleaseUnmanagedResources ();
			mDeviceCreated = false;

			SetupContext (createInfo);
			SetupRenderpass (createInfo);

			// MANDATORY
			createInfo.Swapchains.Create (createInfo.Command, createInfo.Width, createInfo.Height);
					
			SetupSwapchain (createInfo);

			CreateFramebuffers (createInfo);

			Scissor = new MgRect2D { 
				Extent = new MgExtent2D{ Width = createInfo.Width, Height = createInfo.Height },
				Offset = new MgOffset2D{ X = 0, Y = 0 },
			};

			// initialise viewport
			CurrentViewport = new MgViewport {
				Width = createInfo.Width,
				Height = createInfo.Height,
				X = 0,
				Y = 0,
				MinDepth = 0f,
				MaxDepth = 1f,
			};

			mDeviceCreated = true;
		}

		bool mDeviceCreated = false;
		public bool DeviceCreated ()
		{
			return mDeviceCreated;
		}

		public MgViewport CurrentViewport {
			get;
			set;
		}

		public MgRect2D Scissor {
			get;
			private set;
		}

		void SetupSwapchain (MgGraphicsDeviceCreateInfo createInfo)
		{
			if (createInfo.Swapchains.Swapchain == null)
			{
				throw new ArgumentNullException ("createInfo.Swapchains.Swapchain");
			}
			var sc = createInfo.Swapchains.Swapchain as IOpenTKSwapchainKHR;
			if (sc == null)
			{
				throw new InvalidCastException ("Not a IOpenTKSwapchainKHR type");
			}
			sc.Initialize (Context, (uint)createInfo.Swapchains.Buffers.Length);
		}

		~OpenTKGraphicsDevice()
		{
			Dispose (false);
		}

		public void Dispose ()
		{
			Dispose (true);
			GC.SuppressFinalize (this);
		}

		void ReleaseUnmanagedResources ()
		{
			if (Context != null)
				Context.Dispose ();

			if (mRenderpass != null)
			{
				mRenderpass.DestroyRenderPass (mPartition.Device, null);
			}
		}

		public bool IsDisposed ()
		{
			return mIsDisposed;
		}

		private bool mIsDisposed = false;
		protected virtual void Dispose(bool isDisposing)
		{
			if (mIsDisposed)
				return;

			ReleaseUnmanagedResources ();

			mIsDisposed = true;
		}

		#endregion



	}
}

