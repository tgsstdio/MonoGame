using OpenTK.Graphics.OpenGL;
using BirdNest.MonoGame.Blocks;

namespace BirdNest.MonoGame.Graphics.Ktx
{
	public interface ITextureAtlas
	{
		void Initialize();
		ITextureChapter Add(TextureCatalog catalog, GLImageType key, TextureTarget glTarget, ImageDimensions dims);
		//void Add(TextureChapterInfo chapter, GLImageType key);
		void Clear();
	}
}

