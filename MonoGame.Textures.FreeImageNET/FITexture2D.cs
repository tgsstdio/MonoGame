using System;
using MonoGame.Core;

namespace MonoGame.Textures.FreeImageNET
{
	public class FITexture2D : ITexture2D
	{
		public FITexture2D (int sortingKey)
		{
			SortingKey = sortingKey;
		}

		#region ITexture2D implementation

		public void Reload (System.IO.Stream textureStream)
		{
			throw new NotImplementedException ();
		}

		public void SaveAsPng (System.IO.Stream stream, int width, int height)
		{
			throw new NotImplementedException ();
		}

		public void SaveAsJpeg (System.IO.Stream stream, int width, int height)
		{
			throw new NotImplementedException ();
		}

		public void SetData<T> (T[] data) where T : struct
		{
			throw new NotImplementedException ();
		}

		public void SetData<T> (T[] data, int startIndex, int elementCount) where T : struct
		{
			throw new NotImplementedException ();
		}

		public void SetData<T> (int level, Rectangle? rect, T[] data, int startIndex, int elementCount) where T : struct
		{
			throw new NotImplementedException ();
		}

		public void SetData<T> (int level, int arraySlice, Rectangle? rect, T[] data, int startIndex, int elementCount) where T : struct
		{
			throw new NotImplementedException ();
		}

		public void GetData<T> (T[] data) where T : struct
		{
			throw new NotImplementedException ();
		}

		public void GetData<T> (T[] data, int startIndex, int elementCount) where T : struct
		{
			throw new NotImplementedException ();
		}

		public void GetData<T> (int level, Rectangle? rect, T[] data, int startIndex, int elementCount) where T : struct
		{
			throw new NotImplementedException ();
		}

		public void GetData<T> (int level, int arraySlice, Rectangle? rect, T[] data, int startIndex, int elementCount) where T : struct
		{
			throw new NotImplementedException ();
		}

		public int Width {
			get;
			internal set;
		}

		public int Height {
			get;
			internal set;
		}

		public Rectangle Bounds {
			get;
			internal set;
		}

		#endregion

		#region ITexture implementation

		public void GraphicsDeviceResetting ()
		{
			
		}

		public int SortingKey {
			get;
			private set;
		}

		public Microsoft.Xna.Framework.Graphics.SurfaceFormat Format {
			get;
			internal set;
		}

		public int LevelCount {
			get;
			internal set;
		}

		#endregion
	}
}

