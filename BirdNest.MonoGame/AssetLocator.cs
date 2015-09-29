using System.IO;
using BirdNest.MonoGame.Blocks;
using BirdNest.MonoGame.Graphics;

namespace BirdNest.MonoGame
{
	public class AssetLocator : IAssetLocator
	{
		public IBlockFileSerializer Serializer { get; private set; }
		private readonly ITexturePageLookup mPageLookup;
		private readonly IShaderInfoLookup mShaderLookup;
		public AssetLocator (IBlockFileSerializer serializer, ITexturePageLookup pageLookup, IShaderInfoLookup shaderLookup)
		{
			Serializer = serializer;
			mPageLookup = pageLookup;
			mShaderLookup = shaderLookup;
		}

		void ScanTextures (BlockFile block)
		{
			if (block.Chapters != null)
			{
				foreach (var chapter in block.Chapters)
				{
					mPageLookup.Scan (block.Identifier, chapter);
				}
			}
		}

		void ScanShaders (BlockFile block)
		{
			if (block.Shaders != null)
			{
				foreach (var shader in block.Shaders)
				{
					mShaderLookup.Scan (block.Identifier, shader);
				}
			}
		}


		public BlockFile Scan(Stream s)
		{
			var block = Serializer.Read (s);
			ScanTextures (block);
			ScanShaders (block);
			return block;
		}
	}
}

