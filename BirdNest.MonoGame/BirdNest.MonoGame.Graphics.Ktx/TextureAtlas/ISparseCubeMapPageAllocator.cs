using BirdNest.MonoGame.Blocks;

namespace BirdNest.MonoGame.Graphics.Ktx
{
	public interface ISparseCubeMapPageAllocator
	{
		SparseCubeMapPage Generate(ITextureChapter chapter, int arrayTextureId, long textureHandle, TexturePageInfo info);			
	}

}

