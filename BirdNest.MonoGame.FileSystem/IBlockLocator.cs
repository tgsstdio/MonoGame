namespace MonoGame.Content
{
	public interface IBlockLocator
	{
		BlockIdentifier GetSource (AssetIdentifier identifier);
	}
}

