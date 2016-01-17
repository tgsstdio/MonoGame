using System;

namespace MonoGame.Graphics
{
	[Flags]
	public enum DrawState : byte
	{
		Enabled = 1,
		UsingLowerLeftOrigin = 2,
	}
}

