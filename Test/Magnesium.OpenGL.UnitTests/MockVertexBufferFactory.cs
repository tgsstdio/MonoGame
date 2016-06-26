
namespace Magnesium.OpenGL.UnitTests
{
	public class MockVertexBufferFactory : ICmdVBOCapabilities
	{
		#region IGLVertexBufferFactory implementation

		public void DeleteVBO (int vbo)
		{

		}

		public void BindIndexBuffer (int vbo, int bufferId)
		{
			
		}
		public void BindDoubleVertexAttribute (int vbo, int location, int size, GLVertexAttributeType pointerType, int offset)
		{
			
		}
		public void BindIntVertexAttribute (int vbo, int location, int size, GLVertexAttributeType pointerType, int offset)
		{
			
		}
		public void BindFloatVertexAttribute (int vbo, int location, int size, GLVertexAttributeType pointerType, bool isNormalized, int offset)
		{
			
		}
		public void SetupVertexAttributeDivisor (int vbo, int location, int divisor)
		{
			
		}
		public int GenerateVBO ()
		{
			return 0;
		}
		public void AssociateBufferToLocation (int vbo, int location, int bufferId, long i, uint stride)
		{
			
		}
		#endregion
	}

}

