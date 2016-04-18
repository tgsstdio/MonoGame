using System;
using System.IO;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Core
{
	public interface ITexture2D : ITexture
	{
		int Width { get; }
		int Height { get; }
		Rectangle Bounds { get; }

		void Reload(Stream textureStream);
		void SaveAsPng(Stream stream, int width, int height);
		void SaveAsJpeg(Stream stream, int width, int height);

		void SetData<T>(T[] data) where T : struct;
		void SetData<T>(T[] data, int startIndex, int elementCount) where T : struct;
		void SetData<T>(int level, Rectangle? rect, T[] data, int startIndex, int elementCount)	where T : struct;
		void SetData<T>(int level, int arraySlice, Rectangle? rect, T[] data, int startIndex, int elementCount)	where T : struct;

		void GetData<T> (T[] data) where T : struct;
		void GetData<T> (T[] data, int startIndex, int elementCount) where T : struct;
		void GetData<T>(int level, Rectangle? rect, T[] data, int startIndex, int elementCount) where T : struct;
		void GetData<T>(int level, int arraySlice, Rectangle? rect, T[] data, int startIndex, int elementCount)	where T : struct;
	}
}

