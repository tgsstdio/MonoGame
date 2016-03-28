using System;

namespace MonoGame.Graphics.Vk
{
	[Flags] 
	public enum PipelineCreateFlagBits : byte
	{
		DISABLE_OPTIMIZATION_BIT = 1 << 0,
		ALLOW_DERIVATIVES_BIT = 1 << 1,
		DERIVATIVE_BIT = 1 << 2,
	}
}

