using System;
using MonoGame.Content;

namespace MonoGame.Core
{
	public interface ITexture2DLoader
	{
		ITexture2D Load(AssetIdentifier assetId);
	}
}

