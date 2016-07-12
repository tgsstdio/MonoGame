using MonoGame.Content;

namespace MonoGame.Graphics
{
	public interface IMgBaseTextureLoader
	{
		MgBaseTexture Load(AssetIdentifier assetId);
	}
}

