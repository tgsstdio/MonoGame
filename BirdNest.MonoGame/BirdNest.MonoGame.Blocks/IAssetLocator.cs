using System.IO;
using BirdNest.MonoGame.Blocks;

namespace BirdNest.MonoGame
{
	public interface IAssetLocator
	{
		IBlockFileSerializer Serializer { get; }
		BlockFile Scan(Stream s);
	}
}

