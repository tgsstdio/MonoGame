using System;

namespace MonoGame.Graphics.Vk
{
	public class PipelineLayoutCreateInfo
	{
		public UInt32 Flags { get; set; }
		public DescriptorSetLayout[] SetLayouts { get; set; }
		public PushConstantRange[] PushConstantRanges { get; set; }
	}
}

