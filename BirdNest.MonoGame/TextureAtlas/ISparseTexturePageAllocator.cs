using MonoGame.Content.Blocks;
using MonoGame.Textures;

namespace BirdNest.MonoGame
{
	public interface ISparseTexturePageAllocator
	{
		SparseTexturePage Generate(ITextureChapter chapter, int arrayTextureId, long textureHandle, TexturePageInfo info);
	}
}

