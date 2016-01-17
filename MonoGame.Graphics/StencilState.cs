using System.Runtime.InteropServices;
using System;

namespace MonoGame.Graphics
{
	[StructLayout(LayoutKind.Sequential)]
	public struct StencilState : IEquatable<StencilState>
	{
		//public CompareFunction DepthBufferFunction { get; set; }
		public int StencilMask {get;set;}
		public int ReferenceStencil { get; set; }
		public int StencilWriteMask {get;set;}
		public CompareFunction StencilFunction { get; set; }
		public StencilOperation StencilPass { get; set; }
		public StencilOperation StencilFail {get;set;}
		public StencilOperation StencilDepthBufferFail { get; set; }

		public CompareFunction CounterClockwiseStencilFunction { get; set; }
		public StencilOperation CounterClockwiseStencilPass { get; set; }
		public StencilOperation CounterClockwiseStencilFail {get;set;}
		public StencilOperation CounterClockwiseStencilDepthBufferFail { get; set; }

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
	}
}

