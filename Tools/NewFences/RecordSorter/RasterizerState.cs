using System;
using System.Runtime.InteropServices;

namespace MonoGame.Graphics
{
	[StructLayout(LayoutKind.Sequential)]
	public struct RasterizerState : IEquatable<RasterizerState>, IComparable<RasterizerState>
	{
		//public RasterizerStateBitFlags Flags { get; set;}
		public float DepthBias {get;set;}
		public float SlopeScaleDepthBias {get;set;}

		#region IEquatable implementation

		public bool Equals (RasterizerState other)
		{
			return Math.Abs (this.DepthBias - other.DepthBias) <= float.Epsilon
				&& Math.Abs (this.SlopeScaleDepthBias - other.SlopeScaleDepthBias) <= float.Epsilon;
		}

		#endregion

		#region IComparable implementation

		public int CompareTo (RasterizerState other)
		{
			if (DepthBias < other.DepthBias)
			{
				return -1;
			}
			else if (DepthBias > other.DepthBias)
			{
				return 1;
			}

			if (SlopeScaleDepthBias < other.SlopeScaleDepthBias)
			{
				return -1;
			}
			else if (SlopeScaleDepthBias > other.SlopeScaleDepthBias)
			{
				return 1;
			}

			return 0;
		}

		#endregion
	}
}

