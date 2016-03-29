using System;
using System.Runtime.InteropServices;

namespace MonoGame.Graphics
{
	[StructLayout(LayoutKind.Sequential)]
	public struct MgVec4Ui
	{
		public UInt32 X { get; set; }
		public UInt32 Y { get; set; }
		public UInt32 Z { get; set; }
		public UInt32 W { get; set; }
	}
}

