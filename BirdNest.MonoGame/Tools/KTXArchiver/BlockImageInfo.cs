using System.Collections.Generic;
using System;

namespace KTXArchiver
{
	public class BlockImageInfo
	{
		public ulong Id;
		public int Width;
		public int Height;
		public List<EncoderStartInfo> Mipmaps;
	}
}

