using System;
using System.Runtime.InteropServices;

namespace MonoGame.Graphics.Vk
{
	[StructLayout(LayoutKind.Sequential)]	
	public struct Offset2D
	{
		public Int32 X { get; set; }
		public Int32 Y { get; set; }
	}
}

