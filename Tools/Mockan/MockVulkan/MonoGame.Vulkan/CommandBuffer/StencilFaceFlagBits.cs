using System;

namespace MonoGame.Graphics.Vk
{
	[Flags] 
	public enum StencilFaceFlagBits : byte
	{
		// Front face
		FRONT_BIT = 1 << 0,
		// Back face
		BACK_BIT = 1 << 1,
		// Front and back faces
		FRONT_AND_BACK = 0x3,
	}
}

