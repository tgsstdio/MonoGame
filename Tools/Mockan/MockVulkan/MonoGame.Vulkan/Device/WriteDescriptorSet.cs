using System;

namespace MonoGame.Graphics.Vk
{
	// Device
	public class WriteDescriptorSet
	{
		public DescriptorSet DstSet { get; set; }
		public UInt32 DstBinding { get; set; }
		public UInt32 DstArrayElement { get; set; }
		public UInt32 DescriptorCount { get; set; }
		public DescriptorType DescriptorType { get; set; }
		public DescriptorImageInfo[] ImageInfo { get; set; }
		public DescriptorBufferInfo[] BufferInfo { get; set; }
		public BufferView[] TexelBufferView { get; set; }
	}
}

