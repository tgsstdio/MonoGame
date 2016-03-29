using System;

namespace MonoGame.Graphics
{
	public class BufferCreateInfo
	{
		public BufferCreateFlagBits Flags { get; set; }
		public UInt64 Size { get; set; }
		public BufferUsageFlagBits Usage { get; set; }
		public SharingMode SharingMode { get; set; }
		public UInt32 QueueFamilyIndexCount { get; set; }
		public UInt32[] QueueFamilyIndices { get; set; }
	}
}

