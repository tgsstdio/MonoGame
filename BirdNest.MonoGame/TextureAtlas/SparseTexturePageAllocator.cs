using MonoGame.Content.Blocks;
using MonoGame.Textures;

namespace BirdNest.MonoGame
{
	public class SparseTexturePageAllocator : ISparseTexturePageAllocator
	{
		private readonly ITextureLookup mLookup;
		public SparseTexturePageAllocator (ITextureLookup lookup)
		{
			this.mLookup = lookup;
		}

		#region ISparseTexturePageAllocator implementation

		public SparseTexturePage Generate (ITextureChapter chapter, int arrayTextureId, long textureHandle, TexturePageInfo info)
		{		
			// TODO : Texture 3d stuff
			float slice = chapter.NoOfPages;
			var page = new SparseTexturePage (chapter, slice);

			mLookup.Add (info.Asset, new ArrayTextureLocation{Handle = textureHandle, Slice=slice});
			return page;
		}

		#endregion
	}
}

