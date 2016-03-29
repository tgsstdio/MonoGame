using System;

namespace MonoGame.Graphics
{
	public class QueryPoolCreateInfo
	{
		public UInt32 Flags { get; set; }
		public QueryType QueryType { get; set; }
		public QueryControlFlagBits QueryCount { get; set; }
		public QueryPipelineStatisticFlagBits PipelineStatistics { get; set; }
	}
}

