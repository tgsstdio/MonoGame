using System;
using Magnesium.OpenGL;

namespace HelloMagnesium.Android
{
	class MockGLCmdVBOEntrypoint : IGLCmdVBOEntrypoint
	{
		public void AssociateBufferToLocation(int vbo, int location, int bufferId, long offsets, uint stride)
		{
			throw new NotImplementedException();
		}

		public void BindDoubleVertexAttribute(int vbo, int location, int size, GLVertexAttributeType pointerType, int offset)
		{
			throw new NotImplementedException();
		}

		public void BindFloatVertexAttribute(int vbo, int location, int size, GLVertexAttributeType pointerType, bool isNormalized, int offset)
		{
			throw new NotImplementedException();
		}

		public void BindIndexBuffer(int vbo, int bufferId)
		{
			throw new NotImplementedException();
		}

		public void BindIntVertexAttribute(int vbo, int location, int size, GLVertexAttributeType pointerType, int offset)
		{
			throw new NotImplementedException();
		}

		public void DeleteVBO(int vbo)
		{
			throw new NotImplementedException();
		}

		public int GenerateVBO()
		{
			throw new NotImplementedException();
		}

		public void SetupVertexAttributeDivisor(int vbo, int location, int divisor)
		{
			throw new NotImplementedException();
		}
	}
}