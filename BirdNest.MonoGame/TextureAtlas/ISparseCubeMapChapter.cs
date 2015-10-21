using BirdNest.MonoGame.Blocks;
using BirdNest.MonoGame.Graphics;

namespace BirdNest.MonoGame
{
	public interface ISparseCubeMapChapter
	{
		void Initialise(TextureCatalog catalog, AtlasTextureType imageType, ImageDimensions dims, int arrayTextureId, long textureHandle, int maxNoOfPages);		
	}
}
