using System;

namespace KTXArchiver
{
	public class EncoderStartInfo
	{
		public uint MipPadding {
			get;
			set;
		}

		public ulong Offset { get; set; }
		public string InputFile;
		public string OutputFile;
		public UInt32 ImageSize;
	}
}

