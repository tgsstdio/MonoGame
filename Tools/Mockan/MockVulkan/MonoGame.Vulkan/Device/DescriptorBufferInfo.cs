using System;

namespace MonoGame.Graphics.Vk
{
	using VkBuffer = MonoGame.Graphics.Vk.Buffer;

	// Device
	public class DescriptorBufferInfo
	{
		public VkBuffer Buffer { get; set; }
		public UInt64 Offset { get; set; }
		public UInt64 Range { get; set; }
	}

}

