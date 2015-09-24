using OpenTK.Graphics.OpenGL;
using BirdNest.MonoGame.Blocks;
using BirdNest.MonoGame.Core;

namespace BirdNest.MonoGame.Graphics.Ktx
{
	public interface ITextureChapterAllocator
	{
		ITextureChapter Generate(TextureCatalog catalog, GLImageType imageType, ImageDimensions dims, TextureTarget glTarget);
	}

}

