using System;
using MonoGame.Content.Blocks;
using MonoGame.Textures;

namespace BirdNest.MonoGame
{
	public class SingleTextureChapter : ITextureChapter, ISingleTextureChapter
	{
		private ITextureLookup mLookup;
		private ISinglePageAllocator mAllocator;
		public SingleTextureChapter (ITextureLookup lookup, ISinglePageAllocator pageAllocator)
		{
			mLookup = lookup;
			mAllocator = pageAllocator;
		}

		#region ISingleTextureChapter implementation
		private long mTextureHandle;
		public void Initialise (TextureCatalog catalog, AtlasTextureType imageType, ImageDimensions dims, int textureId, long textureHandle)
		{
			Catalog = catalog;
			ImageType = imageType;
			Dimensions = dims;
			TextureId = textureId;
			mTextureHandle = textureHandle;
		}

		#endregion

		#region ITextureChapter implementation

		public void Clear ()
		{
			throw new NotImplementedException ();
		}

		public bool IsFull ()
		{
			return true;
		}

		public ITexturePage GeneratePage (TexturePageInfo pageInfo)
		{
			if (IsFull())
			{
				throw new Exception ("Too full");
			}

			if (mLookup.Contains (pageInfo.Asset.Identifier))
			{
				throw new Exception ("Texture is already established"); 
			}

			var page = mAllocator.Generate (this, TextureId, mTextureHandle, pageInfo);
			return page;
		}

		public TextureCatalog Catalog {
			get;
			private set;
		}

		public AtlasTextureType ImageType {
			get;
			private set;
		}

		public ImageDimensions Dimensions {
			get;
			private set;
		}

		public int NoOfPages {
			get {
				return 1;
			}
		}

		public int MaxNoOfPages {
			get {
				return 1;
			}
		}

		public int TextureId {
			get;
			private set;
		}

		#endregion
	}
}

