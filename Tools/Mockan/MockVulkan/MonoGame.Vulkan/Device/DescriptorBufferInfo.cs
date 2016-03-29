using System;

namespace MonoGame.Graphics
{
	using VkBuffer = MonoGame.Graphics.Buffer;

	// Device
	public class DescriptorBufferInfo
	{
		public VkBuffer Buffer { get; set; }
		public UInt64 Offset { get; set; }
		public UInt64 Range { get; set; }
	}

}

