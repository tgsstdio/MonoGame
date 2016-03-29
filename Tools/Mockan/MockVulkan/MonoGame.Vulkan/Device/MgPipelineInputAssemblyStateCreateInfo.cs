using System;

namespace MonoGame.Graphics
{
	public class MgPipelineInputAssemblyStateCreateInfo
	{
		public UInt32 Flags { get; set; }
		public MgPrimitiveTopology Topology { get; set; }
		public bool PrimitiveRestartEnable { get; set; }
	}
}

