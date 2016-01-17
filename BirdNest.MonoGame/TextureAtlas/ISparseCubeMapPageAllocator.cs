using MonoGame.Content.Blocks;
using MonoGame.Textures;

namespace BirdNest.MonoGame
{
	public interface ISparseCubeMapPageAllocator
	{
		SparseCubeMapPage Generate(ITextureChapter chapter, int arrayTextureId, long textureHandle, TexturePageInfo info);			
	}

}

