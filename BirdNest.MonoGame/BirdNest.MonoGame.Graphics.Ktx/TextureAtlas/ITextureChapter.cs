using BirdNest.MonoGame.Blocks;
using BirdNest.MonoGame.Core;

namespace BirdNest.MonoGame.Graphics.Ktx
{
	public interface ITextureChapter
	{
		void Clear();

		TextureCatalog Catalog {get;}
		GLImageType ImageType {get;}
		ImageDimensions Dimensions { get; }

		int NoOfPages { get; }
		int MaxNoOfPages {get;}
		int TextureId {get;}

		bool IsFull();
		ITexturePage GeneratePage(TexturePageInfo pageInfo);
	}

}

