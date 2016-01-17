using System.Collections.Generic;
using MonoGame.Content.Blocks;
using MonoGame.Textures;

namespace BirdNest.MonoGame
{
	public abstract class SparseArrayTextureChapter<T> : ITextureChapter where T : ITexturePage
	{
		protected List<T> mPages;
		protected long mTextureHandle;
		protected void Setup(TextureCatalog catalog, AtlasTextureType imageType, ImageDimensions dims, int arrayTextureId, long textureHandle, int maxNoOfPages)
		{
			mPages = new List<T> ();
			Catalog = catalog;
			ImageType = imageType;
			Dimensions = dims;
			TextureId = arrayTextureId;
			mTextureHandle = textureHandle;
			MaxNoOfPages = maxNoOfPages;
		}

		public abstract ITexturePage GeneratePage (TexturePageInfo pageInfo);

		#region ITextureChapter implementation

		public void Clear ()
		{
			foreach (var page in mPages)
			{
				page.Clear ();
			}

			mPages.Clear ();
		}

		public bool IsFull ()
		{
			return NoOfPages >= MaxNoOfPages;
		}

		public TextureCatalog Catalog {
			get;
			private set;
		}

		public AtlasTextureType ImageType {
			get;
			private set;
		}

		public ImageDimensions Dimensions {
			get;
			private set;
		}

		public int NoOfPages {
			get {
				return mPages.Count;
			}
		}

		public int MaxNoOfPages {
			get;
			private set;
		}

		public int TextureId {
			get;
			private set;
		}

		#endregion
	}
}

