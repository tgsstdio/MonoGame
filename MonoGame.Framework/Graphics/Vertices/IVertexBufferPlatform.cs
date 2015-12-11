// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.


namespace Microsoft.Xna.Framework.Graphics
{
	public interface IVertexBufferPlatform
	{
		void SetDataInternal<T> (int offsetInBytes, T[] data, int startIndex, int elementCount, int vertexStride, SetDataOptions options, int bufferSize, int elementSizeInBytes);

		void GetData<T> (int offsetInBytes, T[] data, int startIndex, int elementCount, int vertexStride);

		void GraphicsDeviceResetting ();

		void Construct ();
	}
}
