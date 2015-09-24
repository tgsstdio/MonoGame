using BirdNest.MonoGame.Blocks;

namespace BirdNest.MonoGame.Graphics.Ktx
{
	public interface ISparseTextureChapter
	{
		void Initialise(TextureCatalog catalog, GLImageType imageType, ImageDimensions dims, int arrayTextureId, long textureHandle, int maxNoOfPages);
	}
}

