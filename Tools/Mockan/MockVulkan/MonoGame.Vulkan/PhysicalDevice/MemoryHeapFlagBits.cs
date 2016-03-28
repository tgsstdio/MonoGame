using System;

namespace MonoGame.Graphics.Vk
{
	[Flags] 
	public enum MemoryHeapFlagBits : byte
	{
		// If set, heap represents device memory
		DEVICE_LOCAL_BIT = 1 << 0,
	};
}

