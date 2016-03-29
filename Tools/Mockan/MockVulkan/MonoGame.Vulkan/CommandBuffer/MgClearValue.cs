using System.Runtime.InteropServices;

namespace MonoGame.Graphics
{
	[StructLayout(LayoutKind.Explicit)]
	public struct MgClearValue
	{
		[FieldOffset(0)]
		MgClearColorValue Color; 
		[FieldOffset(0)]
		MgClearDepthStencilValue DepthStencil; 
	}
}

