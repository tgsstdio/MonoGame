using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using OpenTK.Graphics;
using MonoGame.Graphics;

namespace MonoGame.Platform.AndroidGL.Graphics
{
	public class AndroidGLGraphicsDevicePlatform : IGraphicsDevicePlatform
	{
		private readonly IGLExtensionLookup mExtensions;
		private readonly IGLFramebufferHelperSelector mSelector;
		private readonly IAndroidGLDevicePlatform mGLPlatform;
		private IPresentationParameters mPresentation;

		public AndroidGLGraphicsDevicePlatform 
		(
			// IMPLEMENTATION BELOW
			IGLExtensionLookup extensions
			,IGLFramebufferHelperSelector selector
			,IAndroidGLDevicePlatform glPlatform
			,IPresentationParameters presentation
		)
		{
			mExtensions = extensions;
			mSelector = selector;
			mGLPlatform = glPlatform;
			mPresentation = presentation;
		}

		internal IGraphicsContext Context { get; private set; }

		public void Setup ()
		{
			mExtensions.Initialize();

			mGLPlatform.Initialize ();
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

