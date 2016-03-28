using System;
using System.Runtime.InteropServices;

namespace MonoGame.Graphics.Vk
{
	[StructLayout(LayoutKind.Sequential)]	
	public struct Extent3D
	{
		public UInt32 Width { get; set; }
		public UInt32 Height { get; set; }
		public UInt32 Depth { get; set; }
	}
}

