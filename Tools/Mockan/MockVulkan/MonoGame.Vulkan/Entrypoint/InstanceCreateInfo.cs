using System;

namespace MonoGame.Graphics.Vk
{
	public class InstanceCreateInfo
	{
		public UInt32 Flags { get; set; }
		public ApplicationInfo ApplicationInfo { get; set; }
		public String[] EnabledLayerNames { get; set; }
		public String[] EnabledExtensionNames { get; set; }
	}
}

