using System;
using Magnesium;
using Magnesium.OpenGL;

namespace HelloMagnesium.Android
{
	class MockGLImageEntrypoint : IGLImageEntrypoint
	{
		public int CreateTextureStorage1D(int levels, MgFormat format, int width)
		{
			throw new NotImplementedException();
		}

		public int CreateTextureStorage2D(int levels, MgFormat format, int width, int height)
		{
			throw new NotImplementedException();
		}

		public int CreateTextureStorage3D(int levels, MgFormat format, int width, int height, int depth)
		{
			throw new NotImplementedException();
		}

		public void DeleteImage(int textureId)
		{
			throw new NotImplementedException();
		}
	}
}