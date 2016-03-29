using System;

namespace MonoGame.Graphics
{
	[Flags] 
	public enum CullModeFlagBits : byte
	{
		NONE = 0,
		FRONT_BIT = 1 << 0,
		BACK_BIT = 1 << 1,
		FRONT_AND_BACK = 0x3,
	}
}

