using System;
using System.Runtime.InteropServices;

namespace MonoGame.Graphics
{
	[StructLayout(LayoutKind.Sequential)]
	public struct RasterizerState
	{
		//public RasterizerStateBitFlags Flags { get; set;}
		public float DepthBias {get;set;}
		public float SlopeScaleDepthBias {get;set;}
	}
}

