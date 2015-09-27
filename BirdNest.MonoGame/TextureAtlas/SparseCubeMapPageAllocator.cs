using BirdNest.MonoGame.Blocks;
using BirdNest.MonoGame.Graphics;

namespace BirdNest.MonoGame
{
	public class SparseCubeMapPageAllocator : ISparseCubeMapPageAllocator
	{
		private readonly ITextureLookup mLookup;
		public SparseCubeMapPageAllocator (ITextureLookup lookup)
		{
			mLookup = lookup;
		}

		#region ISparseCubeMapPageAllocator implementation

		public SparseCubeMapPage Generate (ITextureChapter chapter, int arrayTextureId, long textureHandle, TexturePageInfo info)
		{
			// TODO : Texture 3d stuff
			float slice = chapter.NoOfPages;
			var page = new SparseCubeMapPage (chapter, slice, chapter.NoOfPages * 6);

			mLookup.Add (info.Asset, new ArrayTextureLocation{Handle = textureHandle, Slice=slice});
			return page;
		}

		#endregion
	}
}

