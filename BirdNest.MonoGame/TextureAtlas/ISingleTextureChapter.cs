using BirdNest.MonoGame.Blocks;
using BirdNest.MonoGame.Graphics;

namespace BirdNest.MonoGame
{
	public interface ISingleTextureChapter
	{
		void Initialise(TextureCatalog catalog, AtlasTextureType imageType, ImageDimensions dims, int textureId, long textureHandle);		
	}

}

