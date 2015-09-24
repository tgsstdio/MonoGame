using System;
using OpenTK.Graphics.OpenGL;

namespace BirdNest.MonoGame.Graphics.Ktx
{
	public class SparseTexturePage : ITexturePage
	{
		private int mNoOfMipmapsUsed;
		public SparseTexturePage (ITextureChapter chapter, float slice) 
		{
			this.Chapter = chapter;
			this.mNoOfMipmapsUsed = 0;
			this.Slice = slice;
		}

		public float Slice { get; private set;}
		//public Asset Asset { get; private set;}
		public void Initialise(MipmapData mipmap)
		{
			var status = GL.GetError ();
			if (status != ErrorCode.NoError)
			{
				throw new Exception (status.ToString ());
			}

			if (mNoOfMipmapsUsed < Chapter.ImageType.NoOfMipmapLevels)
			{
				var format = (All)Chapter.ImageType.GlInternalFormat;
				var pixelType = (All)Chapter.ImageType.GlBaseInternalFormat;

				switch (mipmap.TextureDimensions)
				{
				case 1:
					if (mipmap.NumberOfFaces <= 1)
					{
						mipmap.Target = TextureTarget.Texture1DArray;
					}
					if (mipmap.IsCompressed)
					{
						GL.Ext.CompressedTextureSubImage2D<byte> (Chapter.TextureId, mipmap.Target, mipmap.Level, 0, (int)Slice, mipmap.PixelWidth, 1, (PixelFormat)format, mipmap.Size, mipmap.Data);
						status = GL.GetError ();
						if (status != ErrorCode.NoError)
						{
							throw new Exception (status.ToString ());
						}
					}
					else
					{
						GL.Ext.TextureSubImage2D<byte> (Chapter.TextureId, mipmap.Target, mipmap.Level, 0, (int)Slice, mipmap.PixelWidth, 1, (PixelFormat)format, (PixelType)pixelType, mipmap.Data);
						status = GL.GetError ();
						if (status != ErrorCode.NoError)
						{
							throw new Exception (status.ToString ());
						}
					}
					break;
				case 2:
					if (mipmap.NumberOfFaces <= 1)
					{
						mipmap.Target = TextureTarget.Texture2DArray;
					}
					if (mipmap.IsCompressed)
					{
						// It is simpler to just attempt to load the format, rather than divine which
						// formats are supported by the implementation. In the event of an error,
						// software unpacking can be attempted.

						GL.Ext.CompressedTextureSubImage3D<byte> (
							Chapter.TextureId
							,mipmap.Target
							,mipmap.Level
							,0
							,0
							,(int)Slice
							,mipmap.PixelWidth
							,mipmap.PixelHeight
							,1
							,(PixelFormat)format
							,mipmap.Size
							,mipmap.Data);

						status = GL.GetError ();
						if (status != ErrorCode.NoError)
						{
							throw new Exception (status.ToString ());
						}
					}
					else
					{
						GL.Ext.TextureSubImage3D<byte> (Chapter.TextureId, mipmap.Target, mipmap.Level, 0, 0, (int)Slice, mipmap.PixelWidth, mipmap.PixelHeight, 1, (PixelFormat)format, (PixelType)pixelType, mipmap.Data);
						status = GL.GetError ();
						if (status != ErrorCode.NoError)
						{
							throw new Exception (status.ToString ());
						}
					}
					break;
				case 3:
					throw new NotSupportedException ();
				}				

				// TODO: Texture 3d mipmaps
				++mNoOfMipmapsUsed;
			}
			else
			{
			}
		}

		#region ITexturePage implementation

		public ITextureChapter Chapter {
			get;
			private set;
		}

		public void Load ()
		{
			for (int i = 0; i < mNoOfMipmapsUsed; ++i)
			{
				// TODO :  Texture 3d mipmaps
			}

			throw new NotImplementedException ();
		}

		public void Unload ()
		{
			for (int i = 0; i < mNoOfMipmapsUsed; ++i)
			{
				// TODO :  Texture 3d mipmaps
			}

			throw new NotImplementedException ();
		}

		public void Clear ()
		{
			for (int i = 0; i < mNoOfMipmapsUsed; ++i)
			{
				// TODO :  Texture 3d mipmaps
			}

			throw new NotImplementedException ();
		}

		#endregion
	}
}

