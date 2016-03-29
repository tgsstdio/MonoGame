using System;

namespace MonoGame.Graphics
{
	public class MgDescriptorSetLayoutCreateInfo
	{
		public UInt32 Flags { get; set; }
		public MgDescriptorSetLayoutBinding[] Bindings { get; set; }
	}
}

