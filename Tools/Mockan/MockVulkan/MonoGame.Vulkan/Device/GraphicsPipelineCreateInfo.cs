using System;

namespace MonoGame.Graphics.Vk
{
	public class GraphicsPipelineCreateInfo
	{
		public PipelineCreateFlagBits Flags { get; set; }
		public PipelineShaderStageCreateInfo[] Stages { get; set; }
		public PipelineVertexInputStateCreateInfo VertexInputState { get; set; }
		public PipelineInputAssemblyStateCreateInfo InputAssemblyState { get; set; }
		public PipelineTessellationStateCreateInfo TessellationState { get; set; }
		public PipelineViewportStateCreateInfo ViewportState { get; set; }
		public PipelineRasterizationStateCreateInfo RasterizationState { get; set; }
		public PipelineMultisampleStateCreateInfo MultisampleState { get; set; }
		public PipelineDepthStencilStateCreateInfo DepthStencilState { get; set; }
		public PipelineColorBlendStateCreateInfo ColorBlendState { get; set; }
		public PipelineDynamicStateCreateInfo DynamicState { get; set; }
		public PipelineLayout Layout { get; set; }
		public RenderPass RenderPass { get; set; }
		public UInt32 Subpass { get; set; }
		public Pipeline BasePipelineHandle { get; set; }
		public Int32 BasePipelineIndex { get; set; }
	}
}

