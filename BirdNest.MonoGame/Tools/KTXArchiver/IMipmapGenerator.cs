using System.Collections.Generic;
using BirdNest.Core;

namespace KTXArchiver
{
	public interface IMipmapGenerator
	{
		string MipmapExtension { get;  }
		void Initialise (string extension);
		List<BlockImageInfo> GenerateMipmaps(BlockFile[] blocks);
	}
}

