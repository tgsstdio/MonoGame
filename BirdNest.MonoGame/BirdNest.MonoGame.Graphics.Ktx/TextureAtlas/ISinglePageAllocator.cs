using BirdNest.MonoGame.Blocks;

namespace BirdNest.MonoGame.Graphics.Ktx
{
	public interface ISinglePageAllocator
	{
		SingleTexturePage Generate(ITextureChapter chapter, int arrayTextureId, long textureHandle, TexturePageInfo info);		
	}

}

