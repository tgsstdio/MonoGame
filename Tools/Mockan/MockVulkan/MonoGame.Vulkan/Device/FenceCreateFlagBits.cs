using System;

namespace MonoGame.Graphics.Vk
{
	[Flags] 
	public enum FenceCreateFlagBits : byte
	{
		SIGNALED_BIT = 1 << 0,
	}
}

