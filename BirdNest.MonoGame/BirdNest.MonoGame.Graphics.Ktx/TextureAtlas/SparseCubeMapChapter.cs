using System;
using BirdNest.MonoGame.Blocks;
using BirdNest.MonoGame.Core;

namespace BirdNest.MonoGame.Graphics.Ktx
{
	public class SparseCubeMapChapter : SparseArrayTextureChapter<SparseCubeMapPage>, ISparseCubeMapChapter
	{
		private readonly ISparseCubeMapPageAllocator mAllocator;
		private readonly ITextureLookup mLookup;
		public SparseCubeMapChapter (ITextureLookup lookup, ISparseCubeMapPageAllocator allocator)
		{
			mLookup = lookup;
			mAllocator = allocator;
		}

		#region ISparseCubeMapChapter implementation

		public void Initialise (TextureCatalog catalog, GLImageType imageType, ImageDimensions dims, int textureId, long textureHandle, int maxNoOfPages)
		{
			Setup (catalog, imageType, dims, textureId, textureHandle, maxNoOfPages / 6);
		}

		#endregion

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
	}
}

