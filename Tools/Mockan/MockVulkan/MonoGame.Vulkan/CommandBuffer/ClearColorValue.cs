using System.Runtime.InteropServices;

namespace MonoGame.Graphics
{
	[StructLayout(LayoutKind.Explicit)]
	public struct ClearColorValue
	{
		[FieldOffset(0)]
		public Vec4f Float32; // m4;
		[FieldOffset(0)]
		public Vec4i Int32; // m4;
		[FieldOffset(0)]
		public Vec4Ui Uint32; // m4i	
	}
}

