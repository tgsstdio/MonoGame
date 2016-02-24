using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using OpenTK.Graphics;
using MonoGame.Platform.DesktopGL.Graphics;
using MonoGame.Graphics;

namespace MonoGame.Platform.DesktopGL
{
	public class DesktopGLGraphicsDevice : BaseGraphicsDevice
	{
		private readonly BaseOpenTKGameWindow mWindow;
		private readonly IGraphicsDevicePreferences mDevicePreferences;
		private readonly IGraphicsDeviceLogger mLogger;
		private readonly IGraphicsCapabilities mCapabilities;
		private readonly IGLFramebufferHelper mFramebufferHelper;
		private readonly IGLDevicePlatform mGLPlatform;
		public DesktopGLGraphicsDevice 
		(
			// REQUIRED FOR BASE CLASS 
			 IGraphicsDevicePlatform platform
			,IPresentationParameters presentation
			,IGraphicsAdapter adapter
			,IGraphicsCapabilities capabilities
			,IWeakReferenceCollection resources

			// IMPLEMENTATION BELOW
			,BaseOpenTKGameWindow window
			,IGraphicsDevicePreferences devicePreferences
			,IGraphicsDeviceLogger logger
			,IGLFramebufferHelper framebufferHelper
			,IGLDevicePlatform glPlatform
		) : base(platform, presentation, adapter, resources)
		{
			mWindow = window;
			mDevicePreferences = devicePreferences;
			mCapabilities = capabilities;
			mLogger = logger;
			mFramebufferHelper = framebufferHelper;
			mGLPlatform = glPlatform;
		}

		internal IGraphicsContext Context { get; private set; }
		private void PlatformSetup()
		{
			GraphicsMode mode;
			var wnd = mWindow.GetWindowInfo();

			// Create an OpenGL compatibility context
			var flags = GraphicsContextFlags.Default;
			int major = 1;
			int minor = 0;

			if (Context == null || Context.IsDisposed)
			{
				var color = PresentationParameters.BackBufferFormat.GetColorFormat();
				var depth =
					PresentationParameters.DepthStencilFormat == DepthFormat.None ? 0 :
					PresentationParameters.DepthStencilFormat == DepthFormat.Depth16 ? 16 :
					24;
				var stencil =
					PresentationParameters.DepthStencilFormat == DepthFormat.Depth24Stencil8 ? 8 :
					0;

				var samples = 0;
				if (mDevicePreferences.PreferMultiSampling)
				{
					// Use a default of 4x samples if PreferMultiSampling is enabled
					// without explicitly setting the desired MultiSampleCount.
					if (PresentationParameters.MultiSampleCount == 0)
					{
						PresentationParameters.MultiSampleCount = 4;
					}

					samples = PresentationParameters.MultiSampleCount;
				}

				mode = new GraphicsMode(color, depth, stencil, samples);
				try
				{
					Context = new GraphicsContext(mode, wnd, major, minor, flags);
				}
				catch (Exception e)
				{
					mLogger.Log (string.Format ("Failed to create OpenGL context, retrying. Error: {0}", e));
					major = 1;
					minor = 0;
					flags = GraphicsContextFlags.Default;
					Context = new GraphicsContext(mode, wnd, major, minor, flags);
				}
			}
			Context.MakeCurrent(wnd);
			(Context as IGraphicsContextInternal).LoadAll();
			Context.SwapInterval = PresentationParameters.PresentationInterval.GetSwapInterval();

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
			Context.MakeCurrent(wnd);

			// TODO : initialise extension stuff

			mGLPlatform.Initialise ();
			MaxTextureSlots = mGLPlatform.MaxTextureSlots;
			MaxVertexAttributes = mGLPlatform.MaxVertexAttributes;
			_maxTextureSize = mGLPlatform.MaxTextureSize;
		}

		internal int _maxTextureSize = 0;

		/// TODO : refactor this
		/// <summary>
		/// Gets the max texture slots.
		/// CURRENTLY SITTING IN BASE GRAPHICS DEVICE
		/// </summary>
		/// <value>The max texture slots.</value>
		public int MaxTextureSlots { get; private set; }

		/// TODO : refactor this
		/// <summary>
		/// Gets the max vertex attributes.
		/// CURRENTLY SITTING IN BASE GRAPHICS DEVICE
		/// </summary>
		/// <value>The max vertex attributes.</value>
		public int MaxVertexAttributes { get; private set; }

		private void PlatformInitialize()
		{
			// Ensure the vertex attributes are reset

			// Free all the cached shader programs. 

			// Force reseting states
		}


		#region implemented abstract members of BaseGraphicsDevice

		protected override void ReleaseUnmanagedResources ()
		{
			throw new NotImplementedException ();
		}

		protected override void ReleaseManagedResources ()
		{
			throw new NotImplementedException ();
		}

		#endregion
	}
}

