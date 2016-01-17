namespace MonoGame.Content.Blocks
{
	public interface IAssetManager
	{
		bool Contains(AssetIdentifier key);
		bool Add(AssetInfo key);
		bool Remove(AssetIdentifier key);
	}
}

