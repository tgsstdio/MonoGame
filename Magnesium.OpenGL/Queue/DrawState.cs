using System;

namespace Magnesium.OpenGL
{
	[Flags]
	public enum DrawState : byte
	{
		Enabled = 1,
		UsingLowerLeftOrigin = 2,
		ZeroToOneDepthRange = 4,
	}
}

