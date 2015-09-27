using BirdNest.MonoGame.Core;

namespace BirdNest.MonoGame
{
	public interface ITextureLookup
	{
		bool Add(AssetInfo asset, ArrayTextureLocation location);
		bool TryGetValue(AssetIdentifier key, out ArrayTextureLocation result);
		bool Contains(AssetIdentifier key);
	}
}

