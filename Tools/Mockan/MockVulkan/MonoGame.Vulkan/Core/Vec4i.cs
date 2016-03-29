using System;
using System.Runtime.InteropServices;

namespace MonoGame.Graphics
{
	[StructLayout(LayoutKind.Sequential)]
	public struct Vec4i
	{
		public Int32 X { get; set; }
		public Int32 Y { get; set; }
		public Int32 Z { get; set; }
		public Int32 W { get; set; }
	}
}

