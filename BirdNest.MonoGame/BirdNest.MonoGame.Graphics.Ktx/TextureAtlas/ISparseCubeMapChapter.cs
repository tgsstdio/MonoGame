using BirdNest.MonoGame.Blocks;

namespace BirdNest.MonoGame.Graphics.Ktx
{
	public interface ISparseCubeMapChapter
	{
		void Initialise(TextureCatalog catalog, GLImageType imageType, ImageDimensions dims, int arrayTextureId, long textureHandle, int maxNoOfPages);		
	}
}

