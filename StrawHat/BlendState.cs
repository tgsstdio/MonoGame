using System;

namespace StrawHat
{
	public struct BlendState
	{
		public BlendStateBitFlags Flags {get;set;}
		public BlendFunction ColorBlendFunction {get;set;}
		public BlendFunction AlphaBlendFunction {get;set;}

		public Blend ColorSourceBlend {get;set;}
		public Blend ColorDestinationBlend {get;set;}
		public Blend AlphaSourceBlend {get;set;}
		public Blend AlphaDestinationBlend {get;set;}
	}
}

