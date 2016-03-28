using System;

namespace MonoGame.Graphics.Vk
{
	public class CommandBufferAllocateInfo
	{
		public CommandPool CommandPool { get; set; }
		public CommandBufferLevel Level { get; set; }
		public UInt32 CommandBufferCount { get; set; }
	}

}

