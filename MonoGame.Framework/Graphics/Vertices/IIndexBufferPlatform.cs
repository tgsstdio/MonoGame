// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.


namespace Microsoft.Xna.Framework.Graphics
{
	public interface IIndexBufferPlatform
	{
		void SetDataInternal<T> (int offsetInBytes, T[] data, int startIndex, int elementCount, SetDataOptions options);

		void GetData<T> (int offsetInBytes, T[] data, int startIndex, int elementCount);

		void GraphicsDeviceResetting ();

		void Construct (IndexElementSize indexElementSize, int indexCount);
	}
}
