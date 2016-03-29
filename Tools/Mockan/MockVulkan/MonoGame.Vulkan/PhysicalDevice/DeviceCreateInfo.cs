using System;

namespace MonoGame.Graphics
{
	public class DeviceCreateInfo
	{
		public UInt32 Flags { get; set; }
		public DeviceQueueCreateInfo[] QueueCreateInfos { get; set; }
		public String[] EnabledLayerNames { get; set; }
		public String[] EnabledExtensionNames { get; set; }
		public PhysicalDeviceFeatures EnabledFeatures { get; set; }
	}
}

