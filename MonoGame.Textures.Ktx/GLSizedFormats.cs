using System;

namespace MonoGame.Textures.Ktx
{
	[Flags]
	public enum GLSizedFormats
	{
		None = 0x0,
		NonLegacy = 0x1,
		Legacy = 0x2,
		All = NonLegacy | Legacy
	};
}

