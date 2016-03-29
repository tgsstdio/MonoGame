using System;

namespace MonoGame.Graphics
{
	[Flags] 
	public enum CommandPoolResetFlagBits : byte
	{
		// Release resources owned by the pool
		RELEASE_RESOURCES_BIT = 1 << 0,
	}
}

