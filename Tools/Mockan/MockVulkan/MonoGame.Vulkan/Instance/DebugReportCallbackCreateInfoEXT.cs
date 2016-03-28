using System;

namespace MonoGame.Graphics.Vk
{
	public class DebugReportCallbackCreateInfoEXT
	{
		public DebugReportFlagBitsEXT Flags { get; set; }
		public PFN_vkDebugReportCallbackEXT PfnCallback { get; set; }
		public IntPtr UserData { get; set; }
	}
}

