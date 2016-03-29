using System;

namespace MonoGame.Graphics
{
	public class MgPipelineLayoutCreateInfo
	{
		public UInt32 Flags { get; set; }
		public MgDescriptorSetLayout[] SetLayouts { get; set; }
		public MgPushConstantRange[] PushConstantRanges { get; set; }
	}
}

