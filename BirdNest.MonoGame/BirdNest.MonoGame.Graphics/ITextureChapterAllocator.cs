using BirdNest.MonoGame.Blocks;

namespace BirdNest.MonoGame.Graphics
{
	public interface ITextureChapterAllocator
	{
		ITextureChapter Generate(TextureCatalog catalog, AtlasTextureType imageType, ImageDimensions dims, AtlasTextureTarget glTarget);
	}

}

