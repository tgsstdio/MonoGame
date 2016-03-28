using System;

namespace MonoGame.Graphics.Vk
{
	// Device
	public class DescriptorSetAllocateInfo
	{
		public DescriptorPool DescriptorPool { get; set; }
		public DescriptorSetLayout[] SetLayouts { get; set; }
	}
}

