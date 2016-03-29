using System;

namespace MonoGame.Graphics
{
	public class MgInstanceCreateInfo
	{
		public UInt32 Flags { get; set; }
		public MgApplicationInfo ApplicationInfo { get; set; }
		public String[] EnabledLayerNames { get; set; }
		public String[] EnabledExtensionNames { get; set; }
	}
}

