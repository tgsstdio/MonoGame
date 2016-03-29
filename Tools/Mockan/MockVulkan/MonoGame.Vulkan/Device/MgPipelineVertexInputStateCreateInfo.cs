using System;

namespace MonoGame.Graphics
{
	public class MgPipelineVertexInputStateCreateInfo
	{
		public UInt32 Flags { get; set; }
		public MgVertexInputBindingDescription[] VertexBindingDescriptions { get; set; }
		public MgVertexInputAttributeDescription[] VertexAttributeDescriptions { get; set; }
	}
}

