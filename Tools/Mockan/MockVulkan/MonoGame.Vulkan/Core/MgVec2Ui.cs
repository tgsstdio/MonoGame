using System;
using System.Runtime.InteropServices;

namespace MonoGame.Graphics
{
	[StructLayout(LayoutKind.Sequential)]
	public struct MgVec2Ui
	{
		public UInt32 X { get; set; }
		public UInt32 Y { get; set; }		
	}
}

