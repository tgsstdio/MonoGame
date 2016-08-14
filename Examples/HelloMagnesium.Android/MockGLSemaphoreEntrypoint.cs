using System;
using Magnesium.OpenGL;

namespace HelloMagnesium.Android
{
	class MockGLSemaphoreEntrypoint : IGLSemaphoreEntrypoint
	{
		public IGLSemaphore CreateSemaphore()
		{
			throw new NotImplementedException();
		}
	}
}