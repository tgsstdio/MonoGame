// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.
using System.IO;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Core
{
	public interface ITexture2DPlatform
	{
		//Texture2D FromStream(IGraphicsDevice graphicsDevice, Stream stream);

		void SaveAsPng (Stream stream, int width, int height);

		void Reload (Stream textureStream);

		void SaveAsJpeg (Stream stream, int width, int height);

		void GetData<T> (int level, int arraySlice, Rectangle? rect, T[] data, int startIndex, int elementCount)
			where T : struct;

		void SetData<T> (int level, int arraySlice, Rectangle? rect, T[] data, int startIndex, int elementCount)
			where T : struct;

		void Construct (int width, int height, bool mipmap, SurfaceFormat format, SurfaceType type, bool shared);
	}

}

