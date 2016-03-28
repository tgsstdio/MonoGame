using System;

namespace MonoGame.Graphics.Vk
{
	public class PipelineShaderStageCreateInfo
	{
		public UInt32 Flags { get; set; }
		public ShaderStageFlagBits Stage { get; set; }
		public ShaderModule Module { get; set; }
		public String Name { get; set; }
		public SpecializationInfo SpecializationInfo { get; set; }
	}
}

