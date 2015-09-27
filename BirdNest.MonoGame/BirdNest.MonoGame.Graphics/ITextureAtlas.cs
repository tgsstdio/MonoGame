using BirdNest.MonoGame.Blocks;

namespace BirdNest.MonoGame.Graphics
{
	public interface ITextureAtlas
	{
		void Initialize();
		ITextureChapter Add(TextureCatalog catalog, AtlasTextureType key, AtlasTextureTarget glTarget, ImageDimensions dims);
		//void Add(TextureChapterInfo chapter, GLImageType key);
		void Clear();
	}
}

