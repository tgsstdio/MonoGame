using System;
using System.Runtime.InteropServices;

namespace MonoGame.Graphics.Vk
{
	[StructLayout(LayoutKind.Sequential)]	
	public struct ClearDepthStencilValue
	{
		public float Depth;
		public UInt32 Stencil;
	}
}

