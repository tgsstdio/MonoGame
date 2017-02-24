using MonoGame.Content;

namespace MonoGame.Graphics
{
	public interface IMgTexture2DLoader
	{
		IMgTexture2D Load(AssetIdentifier assetId);
	}
}

