using System;

namespace MonoGame.Graphics.Vk
{
	public class PipelineVertexInputStateCreateInfo
	{
		public UInt32 Flags { get; set; }
		public VertexInputBindingDescription[] VertexBindingDescriptions { get; set; }
		public VertexInputAttributeDescription[] VertexAttributeDescriptions { get; set; }
	}
}

