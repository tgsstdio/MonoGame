using MonoGame.Content;

namespace BirdNest.MonoGame
{
	public interface ITextureHandleLookup
	{
		bool Add (AssetInfo asset, long handle);
		bool Contains (AssetIdentifier key);
		bool TryGetValue (AssetIdentifier key, out TextureHandle result);
	}
}

