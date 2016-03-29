using System;

namespace MonoGame.Graphics
{
	public class MappedMemoryRange
	{
		public DeviceMemory Memory { get; set; }
		public UInt64 Offset { get; set; }
		public UInt64 Size { get; set; }
	}
}

