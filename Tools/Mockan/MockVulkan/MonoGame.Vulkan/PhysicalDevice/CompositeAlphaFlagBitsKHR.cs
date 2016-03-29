using System;

namespace MonoGame.Graphics
{
	[Flags] 
	public enum CompositeAlphaFlagBitsKHR : byte
	{
		OPAQUE_BIT_KHR = 1 << 0,
		PRE_MULTIPLIED_BIT_KHR = 1 << 1,
		POST_MULTIPLIED_BIT_KHR = 1 << 2,
		INHERIT_BIT_KHR = 1 << 3,
	}
}

