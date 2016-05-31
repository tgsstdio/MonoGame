using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using OpenTK.Graphics;
using MonoGame.Platform.DesktopGL.Graphics;
using MonoGame.Graphics;

namespace MonoGame.Platform.DesktopGL
{
	public class DesktopGLGraphicsDevicePlatform : IGraphicsDevicePlatform
	{
		private readonly IOpenTKGameWindow mWindow;
		private readonly IGraphicsDevicePreferences mDevicePreferences;
		private readonly IGraphicsDeviceLogger mLogger;
		private readonly IGLExtensionLookup mExtensions;
		private readonly IGLFramebufferHelperSelector mSelector;
		private readonly IGLDevicePlatform mGLPlatform;
		private IPresentationParameters mPresentation;
		private IOpenTKDeviceQuery mDeviceQuery;

		public DesktopGLGraphicsDevicePlatform 
		(
			// IMPLEMENTATION BELOW
			IOpenTKGameWindow window
			,IGraphicsDevicePreferences devicePreferences
			,IGraphicsDeviceLogger logger
			,IGLExtensionLookup extensions
			,IGLFramebufferHelperSelector selector
			,IGLDevicePlatform glPlatform
			,IOpenTKDeviceQuery deviceQuery
			,IPresentationParameters presentation
		)
		{
			mWindow = window;
			mDevicePreferences = devicePreferences;
			mLogger = logger;
			mExtensions = extensions;
			mSelector = selector;
			mGLPlatform = glPlatform;
			mPresentation = presentation;
			mDeviceQuery = deviceQuery;
		}

		internal IGraphicsContext Context { get; private set; }

		public void Setup ()
		{
			GraphicsMode mode;
			var wnd = mWindow.GetWindowInfo();

			// Create an OpenGL compatibility context
			var flags = GraphicsContextFlags.Default;
			int major = 1;
			int minor = 0;

			if (Context == null || Context.IsDisposed)
			{
				var color = mDeviceQuery.GetColorFormat(mPresentation.BackBufferFormat);
				var depth =
					mPresentation.DepthStencilFormat == DepthFormat.None ? 0 :
					mPresentation.DepthStencilFormat == DepthFormat.Depth16 ? 16 :
					24;
				var stencil =
					mPresentation.DepthStencilFormat == DepthFormat.Depth24Stencil8 ? 8 :
					0;

				var samples = 0;
				if (mDevicePreferences.PreferMultiSampling)
				{
					// Use a default of 4x samples if PreferMultiSampling is enabled
					// without explicitly setting the desired MultiSampleCount.
					if (mPresentation.MultiSampleCount == 0)
					{
						mPresentation.MultiSampleCount = 4;
					}

					samples = mPresentation.MultiSampleCount;
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
			Context.SwapInterval = mDeviceQuery.GetSwapInterval(mPresentation.PresentationInterval);

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

			mExtensions.Initialise();

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

		public void Initialize ()
		{
			// Ensure the vertex attributes are reset

			// Free all the cached shader programs. 

			// Force reseting states
			mSelector.Initialize();
		}

		#region IGraphicsDevicePlatform implementation

		public GraphicsProfile GetHighestSupportedGraphicsProfile (IGraphicsDevice graphicsDevice)
		{
			return GraphicsProfile.HiDef;
		}

		public void SetViewport (ref Viewport value)
		{
			throw new NotImplementedException ();
		}

		public void DrawIndexedPrimitives (PrimitiveType primitiveType, int baseVertex, int startIndex, int primitiveCount)
		{
			throw new NotImplementedException ();
		}

		public IRenderTarget ApplyRenderTargets ()
		{
			throw new NotImplementedException ();
		}

		public void ApplyDefaultRenderTarget ()
		{
			throw new NotImplementedException ();
		}

		public void ResolveRenderTargets ()
		{
			throw new NotImplementedException ();
		}

		public void ApplyState (bool applyShaders)
		{
			throw new NotImplementedException ();
		}

		public void Present ()
		{
			throw new NotImplementedException ();
		}

		public void DrawUserIndexedPrimitives<T> (PrimitiveType primitiveType, T[] vertexData, int vertexOffset, int numVertices, short[] indexData, int indexOffset, int primitiveCount, IVertexDeclaration vertexDeclaration) where T : struct
		{
			throw new NotImplementedException ();
		}

		public void DrawUserIndexedPrimitives<T> (PrimitiveType primitiveType, T[] vertexData, int vertexOffset, int numVertices, int[] indexData, int indexOffset, int primitiveCount, IVertexDeclaration vertexDeclaration) where T : struct
		{
			throw new NotImplementedException ();
		}

		public void DrawUserIndexedPrimitives<T> (PrimitiveType primitiveType, T[] vertexData, int vertexOffset, int numVertices, int[] indexData, int indexOffset, int primitiveCount) where T : struct
		{
			throw new NotImplementedException ();
		}

		public void DrawUserIndexedPrimitives<T> (PrimitiveType primitiveType, T[] vertexData, int vertexOffset, int numVertices, short[] indexData, int indexOffset, int primitiveCount) where T : struct
		{
			throw new NotImplementedException ();
		}

		public void DrawPrimitives (PrimitiveType primitiveType, int vertexStart, int vertexCount)
		{
			throw new NotImplementedException ();
		}

		public void DrawUserPrimitives<T> (PrimitiveType primitiveType, T[] vertexData, int vertexOffset, IVertexDeclaration vertexDeclaration, int vertexCount) where T : struct
		{
			throw new NotImplementedException ();
		}

		public void Clear (ClearOptions options, Vector4 vector4, float maxDepth, int i)
		{
			throw new NotImplementedException ();
		}

		public void BeginApplyState ()
		{
			throw new NotImplementedException ();
		}

		#endregion

		#region IDisposable implementation

		public void Dispose ()
		{

		}

		#endregion
	}
}

