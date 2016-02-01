using System;
using System.Runtime.InteropServices;

namespace MonoGame.Graphics
{
	[StructLayout(LayoutKind.Sequential)]
	public struct DepthState : IEquatable<DepthState>, IComparable<DepthState>
	{
		public CompareFunction DepthBufferFunction { get; set; }
	//	public ClipControl DepthRange { get; set; }

		#region IEquatable implementation

		public bool Equals (DepthState other)
		{
			return	this.DepthBufferFunction == other.DepthBufferFunction;
		}

		#endregion

		#region IComparable implementation

		public int CompareTo (DepthState other)
		{
			if (DepthBufferFunction < other.DepthBufferFunction)
			{
				return -1;
			}
			else if (DepthBufferFunction > other.DepthBufferFunction)
			{
				return 1;
			}
			else
			{
				return 0;
			}				
		}

		#endregion
	}
}

