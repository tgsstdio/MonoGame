using System;

namespace MonoGame.Graphics.Vk
{
	public class DescriptorSetLayoutCreateInfo
	{
		public UInt32 Flags { get; set; }
		public DescriptorSetLayoutBinding[] Bindings { get; set; }
	}
}

