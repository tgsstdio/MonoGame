using System;
using System.Runtime.InteropServices;

namespace MonoGame.Graphics
{
	[StructLayout(LayoutKind.Sequential)]
	public struct DepthState : IEquatable<DepthState>
	{
		public CompareFunction DepthBufferFunction { get; set; }
		public ClipControl DepthRange { get; set; }

		#region IEquatable implementation

		public bool Equals (DepthState other)
		{
			return	this.DepthBufferFunction == other.DepthBufferFunction
				&& 	this.DepthRange == other.DepthRange;
		}

		#endregion
	}
}

