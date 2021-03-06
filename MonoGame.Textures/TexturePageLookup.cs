﻿using System.Collections.Generic;
using MonoGame.Content;
using MonoGame.Content.Blocks;

namespace MonoGame.Textures
{
	public class TexturePageLookup : ITexturePageLookup
	{
		private readonly Dictionary<ulong, TexturePageInfo> mPageLookup;
		public TexturePageLookup ()
		{
			mPageLookup = new Dictionary<ulong, TexturePageInfo> ();
		}

		public void Scan(BlockIdentifier identifier, TextureChapterInfo chapter)
		{
			foreach (var page in chapter.Pages)
			{
				page.Asset.Block = identifier;
				page.Asset.AssetType = AssetType.Texture;
				page.Catalog = chapter.Catalog;
				mPageLookup.Add (page.Asset.Identifier.AssetId, page);
			}
		}

		public bool TryGetValue(AssetIdentifier identifier, out TexturePageInfo result)
		{
			return mPageLookup.TryGetValue (identifier.AssetId, out result);
		}
	}
}

