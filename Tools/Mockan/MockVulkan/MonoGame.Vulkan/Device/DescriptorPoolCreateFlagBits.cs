using System;

namespace MonoGame.Graphics
{
	[Flags] 
	public enum DescriptorPoolCreateFlagBits : byte
	{
		// Descriptor sets may be freed individually
		DESCRIPTOR_POOL_CREATE_FREE_DESCRIPTOR_SET_BIT = 1 << 0,
	}
}

