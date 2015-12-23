// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.


namespace Microsoft.Xna.Framework.Graphics
{
	public interface ITexture3DPlatform
	{
		void GetData<T> (int level, int left, int top, int right, int bottom, int front, int back, T[] data, int startIndex, int elementCount);

		void SetData<T> (int level, int left, int top, int right, int bottom, int front, int back, T[] data, int startIndex, int elementCount, int width, int height, int depth);

		void Construct (IGraphicsDevice graphicsDevice, int width, int height, int depth, bool mipMap, SurfaceFormat format, bool renderTarget);
		
	}
}

