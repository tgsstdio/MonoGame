using System;

namespace MonoGame.Graphics.Vk
{
	public class PipelineRasterizationStateCreateInfo
	{
		public UInt32 Flags { get; set; }
		public bool DepthClampEnable { get; set; }
		public bool RasterizerDiscardEnable { get; set; }
		public PolygonMode PolygonMode { get; set; }
		public CullModeFlagBits CullMode { get; set; }
		public FrontFace FrontFace { get; set; }
		public bool DepthBiasEnable { get; set; }
		public float DepthBiasConstantFactor { get; set; }
		public float DepthBiasClamp { get; set; }
		public float DepthBiasSlopeFactor { get; set; }
		public float LineWidth { get; set; }
	}
}

