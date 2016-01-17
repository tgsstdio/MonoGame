using System.Runtime.InteropServices;

namespace MonoGame.Graphics
{
	[StructLayout(LayoutKind.Sequential)]
	public struct DrawCommand
	{
		public DrawPrimitive Primitive {get;set;}
		public uint Count { get; set;}
	}
}

