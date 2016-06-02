using System;
using Magnesium;

namespace HelloMagnesium
{
	public interface IMgThreadPartition : IDisposable
	{
		IMgCommandPool CommandPool { get; }
		IMgDescriptorPool DescriptorPool { get; }
		IMgCommandBuffer[] CommandBuffers { get; }
		IMgQueue Queue { get; }
		IMgDevice Device { get; }
		IMgPhysicalDevice PhysicalDevice { get; }
	}
}

