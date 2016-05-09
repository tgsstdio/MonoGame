using System.IO;

namespace MonoGame.Content
{
	public interface IContentStreamer
	{
		Stream LoadContent (AssetIdentifier assetId, string extension);
	}
}

