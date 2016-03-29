using System.Runtime.InteropServices;

namespace MonoGame.Graphics
{
	[StructLayout(LayoutKind.Sequential)]	
	public class MgComponentMapping
	{
		public MgComponentSwizzle R { get; set; }
		public MgComponentSwizzle G { get; set; }
		public MgComponentSwizzle B { get; set; }
		public MgComponentSwizzle A { get; set; }
	}
}

