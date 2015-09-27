using BirdNest.MonoGame.Blocks;

namespace BirdNest.MonoGame.Graphics
{
	public interface ITextureChapter
	{
		void Clear();

		TextureCatalog Catalog {get;}
		AtlasTextureType ImageType {get;}
		ImageDimensions Dimensions { get; }

		int NoOfPages { get; }
		int MaxNoOfPages {get;}
		int TextureId {get;}

		bool IsFull();
		ITexturePage GeneratePage(TexturePageInfo pageInfo);
	}

}

