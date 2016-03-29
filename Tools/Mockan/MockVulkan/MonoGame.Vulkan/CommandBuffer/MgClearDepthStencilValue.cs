using System;
using System.Runtime.InteropServices;

namespace MonoGame.Graphics
{
	[StructLayout(LayoutKind.Sequential)]	
	public struct MgClearDepthStencilValue
	{
		public float Depth;
		public UInt32 Stencil;
	}
}

