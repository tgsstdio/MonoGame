using MonoGame.Content.Blocks;
using MonoGame.Textures;

namespace BirdNest.MonoGame
{
	public interface ISingleTextureChapter
	{
		void Initialise(TextureCatalog catalog, AtlasTextureType imageType, ImageDimensions dims, int textureId, long textureHandle);		
	}

}

