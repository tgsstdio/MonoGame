using System.IO;
using BirdNest.MonoGame.Blocks;
using BirdNest.MonoGame.Graphics;

namespace BirdNest.MonoGame
{
	public class AssetLocator : IAssetLocator
	{
		public IBlockFileSerializer Serializer { get; private set; }
		private readonly ITexturePageLookup mPageLookup;
		public AssetLocator (IBlockFileSerializer serializer, ITexturePageLookup pageLookup)
		{
			Serializer = serializer;
			mPageLookup = pageLookup;
		}

		public BlockFile Scan(Stream s)
		{
			var block = Serializer.Read (s);
			foreach (var chapter in block.Chapters)
			{
				mPageLookup.Scan (block.Identifier, chapter);
			}

			return block;
		}
	}
}

