using BirdNest.MonoGame.Blocks;

namespace BirdNest.MonoGame.Graphics.Ktx
{
	public interface ISparseTexturePageAllocator
	{
		SparseTexturePage Generate(ITextureChapter chapter, int arrayTextureId, long textureHandle, TexturePageInfo info);
	}
}

