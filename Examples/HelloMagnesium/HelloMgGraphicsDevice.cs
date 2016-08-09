using System;
using Magnesium;
using OpenTK.Graphics;
using OpenTK;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Graphics;
using MonoGame.Platform.DesktopGL;
using Magnesium.OpenGL;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using MonoGame.Core;

namespace HelloMagnesium
{
	public class HelloMgGraphicsDevice : IHelloGraphicsDevice
	{
		private class NullImageView : IMgImageView
		{
			#region IMgImageView implementation

			public void DestroyImageView (IMgDevice device, MgAllocationCallbacks allocator)
			{
				throw new NotImplementedException ();
			}

			#endregion
		}

		#region IMgDepthStencilBuffer implementation

		public IMgRenderPass Renderpass {
			get {
				return mRenderpass;
			}
		}

		NullImageView mView;

		INativeWindow mWindow;

		private IMgDeviceQuery mDeviceQuery;

		internal IGraphicsContext Context { get; private set; }

		IPresentationParameters mPresentation;

		IGLExtensionLookup mExtensions;

		IGLDevicePlatform mGLPlatform;

		IGraphicsDeviceLogger mLogger;

		IMgThreadPartition mPartition;

		IGLFramebufferHelperSelector mSelector;

		IGraphicsCapabilities mCapabilities;

		IGLQueueRenderer mQueueRenderer;

		public HelloMgGraphicsDevice (
			INativeWindow window,
			IMgThreadPartition partition,
			IMgDeviceQuery deviceQuery,
			IPresentationParameters presentation,		
			IGLFramebufferHelperSelector selector,
			IGLExtensionLookup extensions,
			IGraphicsCapabilities capabilities,
			IGLDevicePlatform glPlatform,
			IGraphicsDeviceLogger logger,
			IGLQueueRenderer queueRenderer
		)
		{
			mPartition = partition;
			mView = new NullImageView ();
			mWindow = window;
			mDeviceQuery = deviceQuery;
			mPresentation = presentation;
			mExtensions = extensions;
			mCapabilities = capabilities;
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
				var color = mDeviceQuery.GetColorFormat (createInfo.Color);
				var depthBit = mDeviceQuery.GetDepthBit (createInfo.DepthStencil);
				var stencilBit = mDeviceQuery.GetStencilBit (createInfo.DepthStencil);
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
			Context.SwapInterval = mDeviceQuery.GetSwapInterval (mPresentation.PresentationInterval);
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
			mCapabilities.Initialize ();
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
			sc.Initialise (Context, (uint)createInfo.Swapchains.Buffers.Length);
		}

		~HelloMgGraphicsDevice()
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

		private bool mIsDisposed = false;
		protected virtual void Dispose(bool isDisposing)
		{
			if (mIsDisposed)
				return;

			ReleaseUnmanagedResources ();

			mIsDisposed = true;
		}

		#endregion

		public void Clear (Color value)
		{

		}

		public void OnDeviceReset ()
		{
			
		}

		public void Present ()
		{
			
		}

		public void Initialize ()
		{
			
		}

		public void OnDeviceResetting ()
		{
			
		}

		public IPresentationParameters PresentationParameters {
			get {
				throw new NotImplementedException ();
			}
		}

		public GraphicsProfile GraphicsProfile {
			get;
			set;
		}

		public Viewport Viewport {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

	}
}

