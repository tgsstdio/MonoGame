using System;
using MonoGame.Content.Blocks;
using MonoGame.Textures;

namespace BirdNest.MonoGame
{
	public class SparseTextureChapter :  SparseArrayTextureChapter<SparseTexturePage>, ISparseTextureChapter
	{
		private readonly ITextureLookup mLookup;
		private readonly ISparseTexturePageAllocator mAllocator;
		public SparseTextureChapter (ITextureLookup manager, ISparseTexturePageAllocator allocator)
		{
			this.mLookup = manager;
			this.mAllocator = allocator;
		}

		#region ITextureChapter implementation

		public override ITexturePage GeneratePage (TexturePageInfo pageInfo)
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
			mPages.Add (page);

			return page;
		}

		public void Initialise (TextureCatalog catalog, AtlasTextureType imageType, ImageDimensions dims, int arrayTextureId, long textureHandle, int maxNoOfPages)
		{
			Setup (catalog, imageType, dims, arrayTextureId, textureHandle, maxNoOfPages);
		}

		#endregion
	}
}

