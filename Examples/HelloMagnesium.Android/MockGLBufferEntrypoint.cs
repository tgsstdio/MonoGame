using System;
using Magnesium;
using Magnesium.OpenGL;

namespace HelloMagnesium.Android
{
	class MockGLBufferEntrypoint : IGLBufferEntrypoint
	{
		public IGLBuffer CreateBuffer(MgBufferCreateInfo createInfo)
		{
			throw new NotImplementedException();
		}
	}
}