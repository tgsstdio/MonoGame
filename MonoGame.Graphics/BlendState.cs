using System.Runtime.InteropServices;
using System;
using Magnesium;

namespace MonoGame.Graphics
{
	[StructLayout(LayoutKind.Sequential)]
	public struct BlendState : IEquatable<BlendState>, IComparable<BlendState>
	{
		//	public BlendStateBitFlags Flags {get;set;}
		public MgBlendOp ColorBlendFunction {get;set;}
		public MgBlendOp AlphaBlendFunction {get;set;}

		public MgBlendFactor ColorSourceBlend {get;set;}
		public MgBlendFactor ColorDestinationBlend {get;set;}
		public MgBlendFactor AlphaSourceBlend {get;set;}
		public MgBlendFactor AlphaDestinationBlend {get;set;}

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

		#region IComparable implementation

		public int CompareTo (BlendState other)
		{
			if (ColorBlendFunction < other.ColorBlendFunction)
			{
				return -1;
			}
			else if (ColorBlendFunction > other.ColorBlendFunction)
			{
				return 1;
			}

			if (AlphaBlendFunction < other.AlphaBlendFunction)
			{
				return -1;
			}
			else if (AlphaBlendFunction > other.AlphaBlendFunction)
			{
				return 1;
			}

			if (ColorSourceBlend < other.ColorSourceBlend)
			{
				return -1;
			}
			else if (ColorSourceBlend > other.ColorSourceBlend)
			{
				return 1;
			}

			if (ColorDestinationBlend < other.ColorDestinationBlend)
			{
				return -1;
			}
			else if (ColorDestinationBlend > other.ColorDestinationBlend)
			{
				return 1;
			}

			if (AlphaSourceBlend < other.AlphaSourceBlend)
			{
				return -1;
			}
			else if (AlphaSourceBlend > other.AlphaSourceBlend)
			{
				return 1;
			}

			if (AlphaDestinationBlend < other.AlphaDestinationBlend)
			{
				return -1;
			}
			else if (AlphaDestinationBlend > other.AlphaDestinationBlend)
			{
				return 1;
			}

			return 0;	
		}

		#endregion
	}
}

