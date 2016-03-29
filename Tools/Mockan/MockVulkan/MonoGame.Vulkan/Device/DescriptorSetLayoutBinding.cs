using System;

namespace MonoGame.Graphics
{
	public class DescriptorSetLayoutBinding
	{
		public UInt32 Binding { get; set; }
		public DescriptorType DescriptorType { get; set; }
		public UInt32 DescriptorCount { get; set; }
		public UInt32 StageFlags { get; set; }
		public Sampler[] ImmutableSamplers { get; set; }
	}
}

