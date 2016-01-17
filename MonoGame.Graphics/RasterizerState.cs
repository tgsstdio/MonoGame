using System;
using System.Runtime.InteropServices;

namespace MonoGame.Graphics
{
	[StructLayout(LayoutKind.Sequential)]
	public struct RasterizerState : IEquatable<RasterizerState>
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
	}
}

