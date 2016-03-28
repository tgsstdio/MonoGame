using System;

namespace MonoGame.Graphics.Vk
{
	using VkBuffer = MonoGame.Graphics.Vk.Buffer;

	public class BufferViewCreateInfo
	{
		public UInt32 Flags { get; set; }
		public VkBuffer Buffer { get; set; }
		public Format Format { get; set; }
		public UInt64 Offset { get; set; }
		public UInt64 Range { get; set; }
	}
}

