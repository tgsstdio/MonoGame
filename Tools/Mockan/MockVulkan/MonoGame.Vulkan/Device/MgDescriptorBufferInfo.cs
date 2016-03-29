using System;

namespace MonoGame.Graphics
{
	using VkBuffer = MonoGame.Graphics.MgBuffer;

	// Device
	public class MgDescriptorBufferInfo
	{
		public VkBuffer Buffer { get; set; }
		public UInt64 Offset { get; set; }
		public UInt64 Range { get; set; }
	}

}

