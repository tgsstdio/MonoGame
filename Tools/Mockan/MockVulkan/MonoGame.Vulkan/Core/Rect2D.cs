using System.Runtime.InteropServices;

namespace MonoGame.Graphics
{
	[StructLayout(LayoutKind.Sequential)]	
	public struct Rect2D
	{
		public Offset2D Offset { get; set; }
		public Extent2D Extent { get; set; }
	}
}

