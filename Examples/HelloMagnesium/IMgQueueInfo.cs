﻿using Magnesium;

namespace HelloMagnesium
{
	public interface IMgQueueInfo
	{
		uint QueueIndex { get; }
		uint QueueFamilyIndex { get; }
		IMgDevice Device { get; }
		IMgQueue Queue { get; }
		IMgThreadPartition CreatePartition();
		IMgThreadPartition CreatePartition (MgCommandPoolCreateFlagBits flags, MgDescriptorPoolCreateInfo descPoolCreateInfo);
	}
}

