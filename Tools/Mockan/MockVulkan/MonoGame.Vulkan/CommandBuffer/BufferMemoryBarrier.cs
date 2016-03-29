using System;

namespace MonoGame.Graphics.Vk
{
	using VkBuffer = MonoGame.Graphics.Vk.Buffer;

	public class BufferMemoryBarrier
	{
		public AccessFlagBits SrcAccessMask { get; set; }
		public AccessFlagBits DstAccessMask { get; set; }
		public UInt32 SrcQueueFamilyIndex { get; set; }
		public UInt32 DstQueueFamilyIndex { get; set; }
		public VkBuffer Buffer { get; set; }
		public UInt64 Offset { get; set; }
		public UInt64 Size { get; set; }
	}
}

