using MonoGame.Content.Blocks;

namespace MonoGame.Textures
{
	public interface ITextureChapterAllocator
	{
		ITextureChapter Generate(TextureCatalog catalog, AtlasTextureType imageType, ImageDimensions dims, AtlasTextureTarget glTarget);
	}

}

