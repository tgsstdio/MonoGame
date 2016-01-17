using System.Runtime.InteropServices;

namespace MonoGame.Graphics
{
	[StructLayout(LayoutKind.Sequential)]
	public struct ResourceListKey
	{
		public byte ListIndex { get; set; }
		public uint ItemIndex { get; set; }		
	}
}

