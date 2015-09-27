using BirdNest.MonoGame.Blocks;
using BirdNest.MonoGame.Graphics;

namespace BirdNest.MonoGame
{
	public class SinglePageAllocator : ISinglePageAllocator
	{
		private readonly ITextureHandleLookup mLookup;
		public SinglePageAllocator (ITextureHandleLookup lookup)
		{
			mLookup = lookup;
		}

		#region ISinglePageAllocator implementation

		public SingleTexturePage Generate (ITextureChapter chapter, int arrayTextureId, long textureHandle, TexturePageInfo info)
		{
			var page = new SingleTexturePage (chapter);

			mLookup.Add (info.Asset, textureHandle);
			return page;
		}

		#endregion
	}
}

