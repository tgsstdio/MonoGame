using System;

namespace MonoGame.Graphics.Vk
{
	public class PipelineViewportStateCreateInfo
	{
		public UInt32 Flags { get; set; }
		public Viewport[] Viewports { get; set; }
		public Rect2D[] Scissors { get; set; }
	}
}

