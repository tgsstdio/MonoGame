using System;
using System.Runtime.InteropServices;

namespace MonoGame.Graphics
{
	[StructLayout(LayoutKind.Sequential)]	
	public struct Extent2D
	{
		public UInt32 Width { get; set; }
		public UInt32 Height { get; set; }
	}
}

