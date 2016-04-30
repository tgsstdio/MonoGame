using System.Runtime.InteropServices;
using System;
using Magnesium;

namespace MonoGame.Graphics
{
	[StructLayout(LayoutKind.Sequential)]
	public struct StencilState : IEquatable<StencilState>, IComparable<StencilState>
	{
		//public CompareFunction DepthBufferFunction { get; set; }
		public int StencilMask {get;set;}
		public int ReferenceStencil { get; set; }
		public int StencilWriteMask {get;set;}
		public MgCompareOp StencilFunction { get; set; }
		public MgStencilOp StencilPass { get; set; }
		public MgStencilOp StencilFail {get;set;}
		public MgStencilOp StencilDepthBufferFail { get; set; }

		public MgCompareOp CounterClockwiseStencilFunction { get; set; }
		public MgStencilOp CounterClockwiseStencilPass { get; set; }
		public MgStencilOp CounterClockwiseStencilFail {get;set;}
		public MgStencilOp CounterClockwiseStencilDepthBufferFail { get; set; }

		//	public DepthStencilBitFlags Flags { get; set; }


		#region IEquatable implementation

		public bool Equals (StencilState other)
		{
			return 
				this.StencilFunction == other.StencilFunction
				//&& this.DepthBufferFunction == other.DepthBufferFunction
				&& this.StencilPass == other.StencilPass
				&& this.StencilFail == other.StencilFail
				&& this.StencilDepthBufferFail == other.StencilDepthBufferFail
				&& this.CounterClockwiseStencilFunction == other.CounterClockwiseStencilFunction
				&& this.CounterClockwiseStencilPass == other.CounterClockwiseStencilPass
				&& this.CounterClockwiseStencilFail == other.CounterClockwiseStencilFail
				&& this.CounterClockwiseStencilDepthBufferFail == other.CounterClockwiseStencilDepthBufferFail
				&& this.StencilMask == other.StencilMask
				&& this.ReferenceStencil == other.ReferenceStencil
				&& this.StencilWriteMask == other.StencilWriteMask;
		}

		#endregion

		#region IComparable implementation

		public int CompareTo (StencilState other)
		{
			if (StencilFunction < other.StencilFunction)
			{
				return -1;
			}
			else if (StencilFunction > other.StencilFunction)
			{
				return 1;
			}

			if (StencilPass < other.StencilPass)
			{
				return -1;
			}
			else if (StencilPass > other.StencilPass)
			{
				return 1;
			}

			if (StencilFail < other.StencilFail)
			{
				return -1;
			}
			else if (StencilFail > other.StencilFail)
			{
				return 1;
			}

			if (StencilDepthBufferFail < other.StencilDepthBufferFail)
			{
				return -1;
			}
			else if (StencilDepthBufferFail > other.StencilDepthBufferFail)
			{
				return 1;
			}

			if (CounterClockwiseStencilFunction < other.CounterClockwiseStencilFunction)
			{
				return -1;
			}
			else if (CounterClockwiseStencilFunction > other.CounterClockwiseStencilFunction)
			{
				return 1;
			}

			if (CounterClockwiseStencilPass < other.CounterClockwiseStencilPass)
			{
				return -1;
			}
			else if (CounterClockwiseStencilPass > other.CounterClockwiseStencilPass)
			{
				return 1;
			}

			if (CounterClockwiseStencilFail < other.CounterClockwiseStencilFail)
			{
				return -1;
			}
			else if (CounterClockwiseStencilFail > other.CounterClockwiseStencilFail)
			{
				return 1;
			}

			if (CounterClockwiseStencilDepthBufferFail < other.CounterClockwiseStencilDepthBufferFail)
			{
				return -1;
			}
			else if (CounterClockwiseStencilDepthBufferFail > other.CounterClockwiseStencilDepthBufferFail)
			{
				return 1;
			}

			if (StencilMask < other.StencilMask)
			{
				return -1;
			}
			else if (StencilMask > other.StencilMask)
			{
				return 1;
			}

			if (ReferenceStencil < other.ReferenceStencil)
			{
				return -1;
			}
			else if (ReferenceStencil > other.ReferenceStencil)
			{
				return 1;
			}

			if (StencilWriteMask < other.StencilWriteMask)
			{
				return -1;
			}
			else if (StencilWriteMask > other.StencilWriteMask)
			{
				return 1;
			}

			return 0;
		}

		#endregion
	}
}

