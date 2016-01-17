using System.IO;
using System.Threading.Tasks;

namespace MonoGame.Content.Blocks
{
	public interface IBlockFileSerializer
	{
		string GetBlockPath (BlockIdentifier id);
		BlockFile Read (Stream reader);
		Task<BlockFile> ReadAsync(Stream reader);
		void Write(Stream writer, BlockFile block);	
		Task WriteAsync (Stream reader, BlockFile block);
	}
}

