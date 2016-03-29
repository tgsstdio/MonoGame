using System;

namespace MonoGame.Graphics
{
	public class CopyDescriptorSet
	{
		public DescriptorSet SrcSet { get; set; }
		public UInt32 SrcBinding { get; set; }
		public UInt32 SrcArrayElement { get; set; }
		public DescriptorSet DstSet { get; set; }
		public UInt32 DstBinding { get; set; }
		public UInt32 DstArrayElement { get; set; }
		public UInt32 DescriptorCount { get; set; }
	}
}

