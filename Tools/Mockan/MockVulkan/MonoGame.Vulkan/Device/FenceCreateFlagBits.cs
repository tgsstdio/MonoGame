using System;

namespace MonoGame.Graphics
{
	[Flags] 
	public enum FenceCreateFlagBits : byte
	{
		SIGNALED_BIT = 1 << 0,
	}
}

