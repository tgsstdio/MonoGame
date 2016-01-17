using MonoGame.Content.Blocks;

namespace MonoGame.Textures
{
	public interface ITextureAtlas
	{
		void Initialize();
		ITextureChapter Add(TextureCatalog catalog, AtlasTextureType key, AtlasTextureTarget glTarget, ImageDimensions dims);
		//void Add(TextureChapterInfo chapter, GLImageType key);
		void Clear();
	}
}

