using System.Runtime.InteropServices;

namespace MonoGame.Graphics
{
	[StructLayout(LayoutKind.Sequential)]
	public struct Vec4f
	{
		public float X {get;set;}
		public float Y {get;set;}
		public float Z {get;set;}
		public float W {get;set;}
	}
}

