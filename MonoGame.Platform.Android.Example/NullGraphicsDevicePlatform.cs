using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Platform.AndroidGL.Example
{
	public class NullGraphicsDevicePlatform : IGraphicsDevicePlatform
	{
		public NullGraphicsDevicePlatform ()
		{
		}

		#region IGraphicsDevicePlatform implementation

		public GraphicsProfile GetHighestSupportedGraphicsProfile (Microsoft.Xna.Framework.IGraphicsDevice graphicsDevice)
		{
			throw new System.NotImplementedException ();
		}

		public void SetViewport (ref Viewport value)
		{
			throw new System.NotImplementedException ();
		}

		public void DrawIndexedPrimitives (PrimitiveType primitiveType, int baseVertex, int startIndex, int primitiveCount)
		{
			throw new System.NotImplementedException ();
		}

		public IRenderTarget ApplyRenderTargets ()
		{
			throw new System.NotImplementedException ();
		}

		public void ApplyDefaultRenderTarget ()
		{
			throw new System.NotImplementedException ();
		}

		public void ResolveRenderTargets ()
		{
			throw new System.NotImplementedException ();
		}

		public void ApplyState (bool applyShaders)
		{
			throw new System.NotImplementedException ();
		}

		public void Present ()
		{
			throw new System.NotImplementedException ();
		}

		public void DrawUserIndexedPrimitives<T> (PrimitiveType primitiveType, T[] vertexData, int vertexOffset, int numVertices, short[] indexData, int indexOffset, int primitiveCount, IVertexDeclaration vertexDeclaration) where T : struct
		{
			throw new System.NotImplementedException ();
		}

		public void DrawUserIndexedPrimitives<T> (PrimitiveType primitiveType, T[] vertexData, int vertexOffset, int numVertices, int[] indexData, int indexOffset, int primitiveCount, IVertexDeclaration vertexDeclaration) where T : struct
		{
			throw new System.NotImplementedException ();
		}

		public void DrawUserIndexedPrimitives<T> (PrimitiveType primitiveType, T[] vertexData, int vertexOffset, int numVertices, int[] indexData, int indexOffset, int primitiveCount) where T : struct
		{
			throw new System.NotImplementedException ();
		}

		public void DrawUserIndexedPrimitives<T> (PrimitiveType primitiveType, T[] vertexData, int vertexOffset, int numVertices, short[] indexData, int indexOffset, int primitiveCount) where T : struct
		{
			throw new System.NotImplementedException ();
		}

		public void DrawPrimitives (PrimitiveType primitiveType, int vertexStart, int vertexCount)
		{
			throw new System.NotImplementedException ();
		}

		public void DrawUserPrimitives<T> (PrimitiveType primitiveType, T[] vertexData, int vertexOffset, IVertexDeclaration vertexDeclaration, int vertexCount) where T : struct
		{
			throw new System.NotImplementedException ();
		}

		public void Clear (ClearOptions options, Microsoft.Xna.Framework.Vector4 vector4, float maxDepth, int i)
		{
			throw new System.NotImplementedException ();
		}

		public void BeginApplyState ()
		{
			throw new System.NotImplementedException ();
		}

		public void Initialize ()
		{
			throw new System.NotImplementedException ();
		}

		public void Setup ()
		{
			throw new System.NotImplementedException ();
		}

		#endregion

		#region IDisposable implementation

		public void Dispose ()
		{
			throw new System.NotImplementedException ();
		}

		#endregion
	}
}

