using System;
using System.Runtime.InteropServices;

namespace Magnesium.OpenGL
{
	[StructLayout(LayoutKind.Sequential)]
	public struct GLQueueRasterizerState : IEquatable<GLQueueRasterizerState>, IComparable<GLQueueRasterizerState>
	{
		//public RasterizerStateBitFlags Flags { get; set;}
		public float DepthBiasConstantFactor {get;set;}
		public float DepthBiasSlopeFactor { get; set;}
		public float LineWidth { get; set; }

		#region IEquatable implementation

		public bool Equals (GLQueueRasterizerState other)
		{
			return Math.Abs (this.DepthBiasConstantFactor - other.DepthBiasConstantFactor) <= float.Epsilon
				&& Math.Abs (this.DepthBiasSlopeFactor - other.DepthBiasSlopeFactor) <= float.Epsilon;
		}

		#endregion

		#region IComparable implementation

		public int CompareTo (GLQueueRasterizerState other)
		{
			if (DepthBiasConstantFactor < other.DepthBiasConstantFactor)
			{
				return -1;
			}
			else if (DepthBiasConstantFactor > other.DepthBiasConstantFactor)
			{
				return 1;
			}

			if (DepthBiasSlopeFactor < other.DepthBiasSlopeFactor)
			{
				return -1;
			}
			else if (DepthBiasSlopeFactor > other.DepthBiasSlopeFactor)
			{
				return 1;
			}

			if (LineWidth < other.LineWidth)
			{
				return -1;
			}
			else if (LineWidth > other.LineWidth)
			{
				return 1;
			}

			return 0;
		}

		#endregion
	}
}

