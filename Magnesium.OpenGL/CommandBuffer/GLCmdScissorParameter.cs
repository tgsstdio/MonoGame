using System;

namespace Magnesium.OpenGL
{
	public class GLCmdScissorParameter : IEquatable<GLCmdScissorParameter>, IMergeable<GLCmdScissorParameter>
	{
		public GLCmdScissorParameter (uint first, MgRect2D[] scissors)
		{
			const int factor = 4;
			var count = scissors.Length;
			var values = new float[factor * count];

			Func<float[], uint, MgRect2D, uint> copyFn = (dest, offset, sc) => {
				dest [0 + offset] = sc.Offset.X;
				dest [1 + offset] = sc.Offset.Y;
				dest [2 + offset] = sc.Extent.Width;
				dest [3 + offset] = sc.Extent.Height;
				return 4;
			};

			GLCmdArraySlice<float>.CopyValues<MgRect2D>(values, 0, scissors, copyFn);

			Parameters = new GLCmdArraySlice<float> (values, factor, first, count);
		}

		private GLCmdScissorParameter(GLCmdArraySlice<float> scissors)
		{
			Parameters = scissors;
		}

		public GLCmdArraySlice<float> Parameters { get; private set; }

		#region IMergeable implementation

		public GLCmdScissorParameter Merge (GLCmdScissorParameter delta)
		{
			var combined = this.Parameters.Merge (delta.Parameters);
			return new GLCmdScissorParameter (combined);
		}

		#endregion

		#region IEquatable implementation

		public bool Equals (GLCmdScissorParameter other)
		{
			return this.Parameters.Matches(other.Parameters,
				(a,b) => Math.Abs(a -b) <= float.Epsilon );
		}

		#endregion
	}
}

