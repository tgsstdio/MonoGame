using System.Runtime.InteropServices;

namespace MonoGame.Graphics
{
	[StructLayout(LayoutKind.Explicit)]
	public struct ClearValue
	{
		[FieldOffset(0)]
		ClearColorValue Color; 
		[FieldOffset(0)]
		ClearDepthStencilValue DepthStencil; 
	}
}

