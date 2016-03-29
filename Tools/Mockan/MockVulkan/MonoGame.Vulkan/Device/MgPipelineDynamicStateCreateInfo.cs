﻿using System;

namespace MonoGame.Graphics
{
	public class MgPipelineDynamicStateCreateInfo
	{
		public UInt32 Flags { get; set; }
		public UInt32 DynamicStateCount { get; set; }
		public MgDynamicState[] DynamicStates { get; set; }
	}
}

