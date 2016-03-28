using System.Runtime.InteropServices;

namespace MonoGame.Graphics.Vk
{
	[StructLayout(LayoutKind.Sequential)]	
	public class ComponentMapping
	{
		public ComponentSwizzle R { get; set; }
		public ComponentSwizzle G { get; set; }
		public ComponentSwizzle B { get; set; }
		public ComponentSwizzle A { get; set; }
	}
}

