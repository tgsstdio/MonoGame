using System;

namespace BirdNest.MonoGame.Graphics.Ktx
{
	[Flags]
	public enum GLR16Formats
	{
		None = 0x0,
		Norm = 0x1,
		SNorm = 0x2,
		All = Norm | SNorm
	};	
}

