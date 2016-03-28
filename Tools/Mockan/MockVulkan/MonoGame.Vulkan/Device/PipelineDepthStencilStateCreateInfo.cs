using System;

namespace MonoGame.Graphics.Vk
{
	public class PipelineDepthStencilStateCreateInfo
	{
		public UInt32 Flags { get; set; }
		public bool DepthTestEnable { get; set; }
		public bool DepthWriteEnable { get; set; }
		public CompareOp DepthCompareOp { get; set; }
		public bool DepthBoundsTestEnable { get; set; }
		public bool StencilTestEnable { get; set; }
		public StencilOpState Front { get; set; }
		public StencilOpState Back { get; set; }
		public float MinDepthBounds { get; set; }
		public float MaxDepthBounds { get; set; }
	}
}

