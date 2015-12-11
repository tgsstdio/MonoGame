// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;


namespace Microsoft.Xna.Framework.Graphics
{
	public interface IGraphicsDevicePlatform : IDisposable
	{
		GraphicsProfile GetHighestSupportedGraphicsProfile (GraphicsDevice graphicsDevice);

		void SetViewport (ref Viewport value);

		void DrawIndexedPrimitives (PrimitiveType primitiveType, int baseVertex, int startIndex, int primitiveCount);

		IRenderTarget ApplyRenderTargets ();

		void ApplyDefaultRenderTarget ();

		void ResolveRenderTargets ();

		void ApplyState (bool applyShaders);

		void Present ();

		void DrawUserIndexedPrimitives<T> (
			PrimitiveType primitiveType,
			T[] vertexData, int vertexOffset,
			int numVertices, short[] indexData,
			int indexOffset, int primitiveCount,
			VertexDeclaration vertexDeclaration) where T : struct;

		void DrawUserIndexedPrimitives<T> (
			PrimitiveType primitiveType,
			T[] vertexData, int vertexOffset,
			int numVertices, int[] indexData,
			int indexOffset, int primitiveCount,
			VertexDeclaration vertexDeclaration) where T : struct;

		void DrawUserIndexedPrimitives<T> (
			PrimitiveType primitiveType,
			T[] vertexData,	int vertexOffset, 
			int numVertices, int[] indexData,
			int indexOffset, int primitiveCount) where T : struct;

		void DrawUserIndexedPrimitives<T> (
			PrimitiveType primitiveType,
			T[] vertexData,	int vertexOffset, 
			int numVertices, short[] indexData,
			int indexOffset, int primitiveCount) where T : struct;

		void DrawPrimitives (PrimitiveType primitiveType, int vertexStart, int vertexCount);

		void DrawUserPrimitives<T> (PrimitiveType primitiveType, T[] vertexData, int vertexOffset, VertexDeclaration vertexDeclaration, int vertexCount);

		void Clear (ClearOptions options, Vector4 vector4, float maxDepth, int i);

		void BeginApplyState ();

		void Initialize ();

		void Setup ();
	}
}
