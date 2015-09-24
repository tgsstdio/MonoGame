using BirdNest.MonoGame.Core;

namespace BirdNest.MonoGame.Graphics.Ktx
{
	public interface ITextureHandleLookup
	{
		bool Add (AssetInfo asset, long handle);
		bool Contains (AssetIdentifier key);
		bool TryGetValue (AssetIdentifier key, out TextureHandle result);
	}
}

