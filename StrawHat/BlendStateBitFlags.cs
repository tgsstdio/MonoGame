using System;

namespace StrawHat
{
	[Flags]
	public enum BlendStateBitFlags : byte
	{
		Off = 0,
		BlendEnabled = 1,
		RedColorWriteChannel = 2,
		BlueColorWriteChannel = 4,
		GreenColorWriteChannel = 8,
		AlphaColorWriteChannel = 16,
	}
}

