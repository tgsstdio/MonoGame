using System;

namespace StrawHat
{
	[Flags]
	public enum DepthStencilBitFlags : byte
	{
		Off = 0,
		DepthBufferEnabled = 0x1,
		DepthBufferWriteEnabled = 0x2,
		StencilEnabled = 0x4,
		StencilWriteEnabled = 0x8,
	}
}

