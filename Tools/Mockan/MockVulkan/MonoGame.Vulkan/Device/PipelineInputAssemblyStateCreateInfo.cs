using System;

namespace MonoGame.Graphics
{
	public class PipelineInputAssemblyStateCreateInfo
	{
		public UInt32 Flags { get; set; }
		public PrimitiveTopology Topology { get; set; }
		public bool PrimitiveRestartEnable { get; set; }
	}
}

