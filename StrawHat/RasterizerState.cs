using System;

namespace StrawHat
{
	public struct RasterizerState
	{
		public RasterizerStateBitFlags Flags { get; set;}
		public float DepthBias {get;set;}
		public float SlopeScaleDepthBias {get;set;}
	}
}

