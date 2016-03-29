using System;

namespace MonoGame.Graphics
{
	public class MgCommandBufferAllocateInfo
	{
		public MgCommandPool CommandPool { get; set; }
		public MgCommandBufferLevel Level { get; set; }
		public UInt32 CommandBufferCount { get; set; }
	}

}

