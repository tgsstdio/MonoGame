using System.Collections.Generic;

namespace KTXArchiver
{
	public interface IMipmapEncoder
	{
		string ProgramFile { get; }
		string[] GenerateArguments (List<BlockImageInfo> images);
	}

}

