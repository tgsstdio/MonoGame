using BirdNest.MonoGame.Blocks;

namespace BirdNest.MonoGame.Graphics.Ktx
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

