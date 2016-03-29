using System;

namespace MonoGame.Graphics
{
	public class ComputePipelineCreateInfo
	{
		public PipelineCreateFlagBits Flags { get; set; }
		public PipelineShaderStageCreateInfo Stage { get; set; }
		public PipelineLayout Layout { get; set; }
		public Pipeline BasePipelineHandle { get; set; }
		public Int32 BasePipelineIndex { get; set; }
	}
}

