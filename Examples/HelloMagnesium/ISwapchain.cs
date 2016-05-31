using Magnesium;
using System;

namespace HelloMagnesium
{

	// CODE taken from vulkanswapchain.hpp by Sascha Willems 2016 (licensed under the MIT license)	
	// 
	public interface ISwapchain : IDisposable
	{
		void Setup(IMgInstance instance, IMgPhysicalDevice physicalDevice, IMgDevice device);
		uint Initialise();
		void Create(IMgCommandBuffer cmd, UInt32 width, UInt32 height);
	}
}

