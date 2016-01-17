using System.Runtime.InteropServices;
using System;

namespace MonoGame.Graphics
{
	[StructLayout(LayoutKind.Sequential)]
	public struct BlendState : IEquatable<BlendState>
	{
		//	public BlendStateBitFlags Flags {get;set;}
		public BlendFunction ColorBlendFunction {get;set;}
		public BlendFunction AlphaBlendFunction {get;set;}

		public Blend ColorSourceBlend {get;set;}
		public Blend ColorDestinationBlend {get;set;}
		public Blend AlphaSourceBlend {get;set;}
		public Blend AlphaDestinationBlend {get;set;}

		#region IEquatable implementation
		public bool Equals (BlendState other)
		{
			return this.ColorBlendFunction == other.ColorBlendFunction
				&& this.AlphaBlendFunction == other.AlphaBlendFunction
				&& this.ColorSourceBlend == other.ColorSourceBlend
				&& this.ColorDestinationBlend == other.ColorDestinationBlend
				&& this.AlphaSourceBlend == other.AlphaSourceBlend
				&& this.AlphaDestinationBlend == other.AlphaDestinationBlend;
		}
		#endregion
	}
}

