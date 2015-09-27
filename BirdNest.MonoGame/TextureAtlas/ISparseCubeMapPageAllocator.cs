using BirdNest.MonoGame.Blocks;
using BirdNest.MonoGame.Graphics;

namespace BirdNest.MonoGame
{
	public interface ISparseCubeMapPageAllocator
	{
		SparseCubeMapPage Generate(ITextureChapter chapter, int arrayTextureId, long textureHandle, TexturePageInfo info);			
	}

}

