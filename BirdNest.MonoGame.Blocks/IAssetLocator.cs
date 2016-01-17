using System.IO;

namespace MonoGame.Content.Blocks
{
	public interface IAssetLocator
	{
		IBlockFileSerializer Serializer { get; }
		BlockFile Scan(Stream s);
	}
}

