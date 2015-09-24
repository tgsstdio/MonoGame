using System;
using System.Collections.Generic;

namespace KTXArchiver
{
	public interface IKTXPacker
	{
		void Initialise (byte[] buffer);
		List<string> PackImages(List<BlockImageInfo> images);		
	}
}

