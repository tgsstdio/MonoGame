using System;

namespace MonoGame.Graphics
{
	public class PipelineColorBlendAttachmentState
	{
		public bool BlendEnable { get; set; }
		public BlendFactor SrcColorBlendFactor { get; set; }
		public BlendFactor DstColorBlendFactor { get; set; }
		public BlendOp ColorBlendOp { get; set; }
		public BlendFactor SrcAlphaBlendFactor { get; set; }
		public BlendFactor DstAlphaBlendFactor { get; set; }
		public BlendOp AlphaBlendOp { get; set; }
		public UInt32 ColorWriteMask { get; set; }
	}
}

