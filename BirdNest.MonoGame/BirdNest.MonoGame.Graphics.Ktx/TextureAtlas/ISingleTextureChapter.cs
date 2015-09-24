using BirdNest.MonoGame.Blocks;

namespace BirdNest.MonoGame.Graphics.Ktx
{
	public interface ISingleTextureChapter
	{
		void Initialise(TextureCatalog catalog, GLImageType imageType, ImageDimensions dims, int textureId, long textureHandle);		
	}

}

