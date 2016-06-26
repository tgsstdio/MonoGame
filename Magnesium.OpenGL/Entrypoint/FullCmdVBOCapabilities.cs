using System;
using OpenTK.Graphics.OpenGL;

namespace Magnesium.OpenGL
{
	public class FullCmdVBOCapabilities : ICmdVBOCapabilities
	{
		#region ICmdVBOCapabilities implementation

		public void BindIndexBuffer (int vbo, int bufferId)
		{
			GL.VertexArrayElementBuffer (vbo, bufferId);
		}

		public void BindDoubleVertexAttribute (int vbo, int location, int size, GLVertexAttributeType pointerType, int offset)
		{
			GL.VertexArrayAttribLFormat (vbo, location, size, (All)GetVertexAttribType(pointerType), offset);
		}

		public void BindIntVertexAttribute (int vbo, int location, int size, GLVertexAttributeType pointerType, int offset)
		{

			GL.VertexArrayAttribIFormat (vbo, location, size, GetVertexAttribType(pointerType), offset);
		}

		static VertexAttribType GetVertexAttribType (GLVertexAttributeType pointerType)
		{
			switch (pointerType)
			{
			case GLVertexAttributeType.Byte:
				return VertexAttribType.Byte;
			case GLVertexAttributeType.UnsignedByte:
				return VertexAttribType.UnsignedByte;

			case GLVertexAttributeType.Double:
				return VertexAttribType.Double;


			case GLVertexAttributeType.Float:
				return VertexAttribType.Float;
			case GLVertexAttributeType.HalfFloat:
				return VertexAttribType.HalfFloat;

			case GLVertexAttributeType.Int:
				return VertexAttribType.Int;
			case GLVertexAttributeType.UnsignedInt:
				return VertexAttribType.UnsignedInt;

			case GLVertexAttributeType.Int2101010Rev:
				return VertexAttribType.Int2101010Rev;
			case GLVertexAttributeType.UnsignedInt2101010Rev:
				return VertexAttribType.UnsignedInt2101010Rev;

			case GLVertexAttributeType.Short:
				return VertexAttribType.Short;
			case GLVertexAttributeType.UnsignedShort:
				return VertexAttribType.UnsignedShort;

			default:
				throw new NotSupportedException ();
			}
		}

		public void BindFloatVertexAttribute (int vbo, int location, int size, GLVertexAttributeType pointerType, bool isNormalized, int offset)
		{
			GL.VertexArrayAttribFormat (vbo, location, size, GetVertexAttribType(pointerType), isNormalized, offset);
		}

		public void SetupVertexAttributeDivisor (int vbo, int location, int divisor)
		{
			GL.VertexArrayBindingDivisor (vbo, location, divisor);
		}

		public int GenerateVBO ()
		{
			return GL.GenVertexArray ();
		}

		public void DeleteVBO (int vbo)
		{
			GL.DeleteVertexArray (vbo);
		}

		public void AssociateBufferToLocation (int vbo, int location, int bufferId, long offsets, uint stride)
		{
			if (stride >= (uint)int.MaxValue)
			{
				throw new ArgumentOutOfRangeException ("stride", "stride >= int.MaxValue");
			}

			GL.VertexArrayVertexBuffer (vbo, location, bufferId, new IntPtr (offsets), (int)stride);
		}

		#endregion
	}
}

