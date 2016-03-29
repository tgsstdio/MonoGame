using System;

namespace MonoGame.Graphics
{
	public class PipelineCacheCreateInfo
	{
		public UInt32 Flags { get; set; }
		public UIntPtr InitialDataSize { get; set; }
		public IntPtr InitialData { get; set; }
	}
}

