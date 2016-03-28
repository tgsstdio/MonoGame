using System;

namespace MonoGame.Graphics.Vk
{
	public class DescriptorPoolCreateInfo
	{
		public DescriptorPoolCreateFlagBits Flags { get; set; }
		public UInt32 MaxSets { get; set; }
		public DescriptorPoolSize[] PoolSizes { get; set; }
	}
}

