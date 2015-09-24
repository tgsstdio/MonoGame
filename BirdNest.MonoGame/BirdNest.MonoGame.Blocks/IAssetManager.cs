using BirdNest.MonoGame.Core;

namespace BirdNest.MonoGame.Blocks
{
	public interface IAssetManager
	{
		bool Contains(AssetIdentifier key);
		bool Add(AssetInfo key);
		bool Remove(AssetIdentifier key);
	}
}

