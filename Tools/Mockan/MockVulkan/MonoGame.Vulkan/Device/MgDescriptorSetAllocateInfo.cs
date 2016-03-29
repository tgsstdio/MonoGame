using System;

namespace MonoGame.Graphics
{
	// Device
	public class MgDescriptorSetAllocateInfo
	{
		public MgDescriptorPool DescriptorPool { get; set; }
		public MgDescriptorSetLayout[] SetLayouts { get; set; }
	}
}

