using System;

namespace MonoGame.Graphics
{
	public class CommandBufferInheritanceInfo
	{
		public RenderPass RenderPass { get; set; }
		public UInt32 Subpass { get; set; }
		public Framebuffer Framebuffer { get; set; }
		public bool OcclusionQueryEnable { get; set; }
		public QueryControlFlagBits QueryFlags { get; set; }
		public QueryPipelineStatisticFlagBits PipelineStatistics { get; set; }
	}
}

