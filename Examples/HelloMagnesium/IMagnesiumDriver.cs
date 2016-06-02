using System;
using Magnesium;

namespace HelloMagnesium
{
	public interface IMagnesiumDriver : IDisposable
	{
		IMgInstance Instance { get; }
		void Initialize (MgApplicationInfo appInfo);
		void Initialize (MgApplicationInfo appInfo, string[] extensionLayers, string[] extensionNames);

		IMgLogicalDevice CreateGraphicsDevice ();
		IMgLogicalDevice CreateGraphicsDevice(IMgSurfaceKHR presentationSurface);
		IMgLogicalDevice CreateDevice(uint physicalDevice, IMgSurfaceKHR presentationSurface, MgQueueAllocation requestCount, MgQueueFlagBits requestedQueueType);
		IMgLogicalDevice CreateDevice(IMgPhysicalDevice gpu, MgDeviceQueueCreateInfo queueCreateInfo);
	}
}

