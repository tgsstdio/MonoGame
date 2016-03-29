using System;

namespace MonoGame.Graphics
{
	public class MgMappedMemoryRange
	{
		public MgDeviceMemory Memory { get; set; }
		public UInt64 Offset { get; set; }
		public UInt64 Size { get; set; }
	}
}

