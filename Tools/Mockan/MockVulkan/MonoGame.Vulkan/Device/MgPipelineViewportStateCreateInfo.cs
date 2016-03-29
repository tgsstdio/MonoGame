using System;

namespace MonoGame.Graphics
{
	public class MgPipelineViewportStateCreateInfo
	{
		public UInt32 Flags { get; set; }
		public MgViewport[] Viewports { get; set; }
		public MgRect2D[] Scissors { get; set; }
	}
}

