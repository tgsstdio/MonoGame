using System;

namespace MonoGame.Graphics
{
	public class MgQueryPoolCreateInfo
	{
		public UInt32 Flags { get; set; }
		public MgQueryType QueryType { get; set; }
		public MgQueryControlFlagBits QueryCount { get; set; }
		public MgQueryPipelineStatisticFlagBits PipelineStatistics { get; set; }
	}
}

