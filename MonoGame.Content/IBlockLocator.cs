using System;

namespace MonoGame.Content
{
	public interface IBlockLocator
	{
		BlockIdentifier GetSource (AssetIdentifier assetId);
		string GetLocalPath(AssetIdentifier assetId);
	}
}

