using OpenTK.Graphics.OpenGL;

namespace Magnesium.OpenGL
{
	public class GLVertexInputAttribute
	{
		public GLVertexAttribFunction Function { get; private set; }
		public uint Binding { get; private set; }
		public int Location { get; private set; }
		public int Size { get; private set; }
		public GLVertexAttributeType PointerType { get; private set; }
		public bool IsNormalized { get; private set; }
		public int Stride { get; private set; }
		public int Offset {get; private set; }

		public int Divisor { get; private set; }

		public GLVertexInputAttribute (uint binding, uint location, uint stride, uint offset, int divisor, GLVertexAttributeInfo attribute)
		{
			Binding = binding;
			Location = (int) location;
			Stride = (int) stride;
			Divisor = divisor;
			Offset = (int) offset;
			Size = attribute.Size;
			PointerType = attribute.PointerType;
			IsNormalized = attribute.IsNormalized;
			Function = attribute.Function;
			//GL.VertexAttribPointer (Location, Elements, VertexAttribPointerType.Float, false, Stride, Offset);
		}
	}
}

