using MonoGame.Content;

namespace MonoGame.Graphics
{
	public interface IMgTextureLoader
	{
        void Initialize();
		IMgTexture Load(AssetIdentifier assetId);
	}
}

