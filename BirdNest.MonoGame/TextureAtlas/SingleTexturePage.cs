using System;
using OpenTK.Graphics.OpenGL;
using BirdNest.MonoGame.Graphics;

namespace BirdNest.MonoGame
{
	public class SingleTexturePage : ITexturePage 
	{
		public SingleTexturePage (ITextureChapter chapter)
		{
			Chapter = chapter;
			mNextMipmap = 0;
		}

		#region ITexturePage implementation
		private int mNextMipmap;
		public void Initialise (MipmapData mipmap)
		{
			if (mNextMipmap >= Chapter.ImageType.NoOfMipmapLevels)
			{
				throw new InvalidOperationException ("Too many mipmaps for this ");
			}

			if (mipmap.Level != mNextMipmap)
			{
				throw new InvalidOperationException ("Missing mipmap");
			}

			if (mipmap.Target == AtlasTextureTarget.Unsupported)
			{
				throw new NotSupportedException ();
			}

			TextureTarget glTarget = TextureTarget.Texture2D;
			switch (mipmap.Target)
			{
			case AtlasTextureTarget.Texture1D:
				glTarget = TextureTarget.Texture1DArray;
				break;
			case AtlasTextureTarget.Texture2D:
				glTarget = TextureTarget.Texture2DArray;
				break;
			case AtlasTextureTarget.TextureCubeMap:
				glTarget = TextureTarget.TextureCubeMapArray;
				break;

			}

			switch (mipmap.TextureDimensions)
			{
			case 1:
				if (mipmap.IsCompressed)
				{
					GL.Ext.CompressedTextureSubImage1D<byte> (Chapter.TextureId, glTarget, mipmap.Level, 0, mipmap.PixelWidth, (PixelFormat)Chapter.ImageType.GlInternalFormat, mipmap.Size, mipmap.Data);
				}
				else
				{
					GL.Ext.TextureSubImage1D<byte> (Chapter.TextureId, glTarget, mipmap.Level, 0, mipmap.PixelWidth, (PixelFormat)Chapter.ImageType.GlInternalFormat, (PixelType)Chapter.ImageType.GlBaseInternalFormat, mipmap.Data);
				}
				break;
			case 2:
				if (mipmap.IsCompressed)
				{
					// It is simpler to just attempt to load the format, rather than divine which
					// formats are supported by the implementation. In the event of an error,
					// software unpacking can be attempted.
					GL.Ext.CompressedTextureSubImage2D<byte> (Chapter.TextureId, glTarget, mipmap.Level, 0, 0, mipmap.PixelWidth, mipmap.PixelHeight, (PixelFormat)Chapter.ImageType.GlInternalFormat, mipmap.Size, mipmap.Data);
				}
				else
				{
					GL.Ext.TextureSubImage2D<byte> (Chapter.TextureId, glTarget, mipmap.Level, 0, 0, mipmap.PixelWidth, mipmap.PixelHeight, (PixelFormat)Chapter.ImageType.GlFormat, (PixelType)Chapter.ImageType.GlBaseInternalFormat, mipmap.Data);
				}
				break;
			case 3:
				if (mipmap.IsCompressed)
				{
					GL.Ext.CompressedTextureSubImage3D<byte> (Chapter.TextureId, glTarget, mipmap.Level, 0, 0, 0, mipmap.PixelWidth, mipmap.PixelHeight, mipmap.PixelDepth, (PixelFormat)Chapter.ImageType.GlInternalFormat, mipmap.Size, mipmap.Data);
				}
				else
				{
					GL.Ext.TextureSubImage3D<byte> (Chapter.TextureId, glTarget, mipmap.Level, 0, 0, 0, mipmap.PixelWidth, mipmap.PixelHeight, mipmap.PixelDepth, (PixelFormat)Chapter.ImageType.GlFormat, (PixelType)Chapter.ImageType.GlBaseInternalFormat, mipmap.Data);
				}
				break;
			}
			++mNextMipmap;
		}

		public void Clear ()
		{
			throw new NotImplementedException ();
		}

		public void Load ()
		{
			throw new NotImplementedException ();
		}

		public void Unload ()
		{
			throw new NotImplementedException ();
		}

		public ITextureChapter Chapter {
			get;
			set;
		}

		#endregion
	}
}

