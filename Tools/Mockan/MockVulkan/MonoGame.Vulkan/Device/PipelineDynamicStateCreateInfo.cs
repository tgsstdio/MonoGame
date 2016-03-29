using System;

namespace MonoGame.Graphics
{
	public class PipelineDynamicStateCreateInfo
	{
		public UInt32 Flags { get; set; }
		public UInt32 DynamicStateCount { get; set; }
		public DynamicState[] DynamicStates { get; set; }
	}
}

