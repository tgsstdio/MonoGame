using System;
using Magnesium;
using Magnesium.OpenGL;

namespace HelloMagnesium.Android
{
	class MockGLImageViewEntrypoint : IGLImageViewEntrypoint
	{
		public int CreateImageView(GLImage originalImage, MgImageViewCreateInfo pCreateInfo)
		{
			throw new NotImplementedException();
		}

		public void DeleteImageView(int texture)
		{
			throw new NotImplementedException();
		}
	}
}