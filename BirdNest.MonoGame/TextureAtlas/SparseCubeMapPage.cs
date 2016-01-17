using System;
using MonoGame.Textures;
using OpenTK.Graphics.OpenGL;

namespace BirdNest.MonoGame
{
	public class SparseCubeMapPage : ITexturePage
	{
		private int mNextMipmap;
		private int mNextFace;
		public float Slice { get; private set;}
		public int Offset { get; private set;}
		public SparseCubeMapPage (ITextureChapter chapter, float slice, int offset)
		{
			Chapter = chapter;
			mNextMipmap = 0;
			mNextFace = 0;
			Slice = slice;
			Offset = offset;
		}

		#region ITexturePage implementation
		public void Initialise (MipmapData mipmap)
		{
			if (mNextMipmap >= Chapter.ImageType.NoOfMipmapLevels)
			{
				throw new InvalidOperationException ("Too many mipmaps for this");
			}

			if (mipmap.Level != mNextMipmap)
			{
				throw new InvalidOperationException ("Missing mipmap");
			}

			if (mipmap.FaceIndex != mNextFace)
			{
				throw new InvalidOperationException ("Missing face");
			}

			TextureTarget glTarget = TextureTarget.TextureCubeMapArray;

			int firstLayer = Offset + mNextFace;
			const int NO_OF_LAYERS_TO_UPDATE = 1;
			if (mipmap.IsCompressed)
			{
				// It is simpler to just attempt to load the format, rather than divine which
				// formats are supported by the implementation. In the event of an error,
				// software unpacking can be attempted.
				GL.Ext.CompressedTextureSubImage3D<byte> 
				(
					Chapter.TextureId
					,glTarget
					,mipmap.Level
					,0
					,0
					,firstLayer
					,mipmap.PixelWidth
					,mipmap.PixelHeight
					,NO_OF_LAYERS_TO_UPDATE
					,(PixelFormat)Chapter.ImageType.GlInternalFormat
					,mipmap.Size
					,mipmap.Data);
			}
			else
			{
				GL.Ext.TextureSubImage3D<byte> (
					Chapter.TextureId
					,glTarget
					,mipmap.Level
					,0
					,0
					,firstLayer
					,mipmap.PixelWidth
					,mipmap.PixelHeight
					,NO_OF_LAYERS_TO_UPDATE
					,(PixelFormat)Chapter.ImageType.GlInternalFormat
					,(PixelType)Chapter.ImageType.GlBaseInternalFormat
					,mipmap.Data);
			}

			if (mNextFace >= 5)
			{
				++mNextMipmap;
			}

			mNextFace = (mNextFace + 1) % 6;
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
			private set;
		}
		#endregion
	}
}

