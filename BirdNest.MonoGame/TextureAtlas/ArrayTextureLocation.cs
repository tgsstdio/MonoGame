using System.Runtime.InteropServices;

namespace BirdNest.MonoGame
{
	[StructLayout(LayoutKind.Sequential)]	
	public struct ArrayTextureLocation
	{
		public long Handle;
		public float Slice;
	}
}

