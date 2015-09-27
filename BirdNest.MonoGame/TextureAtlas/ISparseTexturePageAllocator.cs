using BirdNest.MonoGame.Blocks;
using BirdNest.MonoGame.Graphics;

namespace BirdNest.MonoGame
{
	public interface ISparseTexturePageAllocator
	{
		SparseTexturePage Generate(ITextureChapter chapter, int arrayTextureId, long textureHandle, TexturePageInfo info);
	}
}

