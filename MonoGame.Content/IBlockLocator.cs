using System;

namespace MonoGame.Content
{
	public interface IBlockLocator
	{
		string GetBlockPath(AssetIdentifier assetId);
		string GetLocalPath(AssetIdentifier assetId);
	}
}

