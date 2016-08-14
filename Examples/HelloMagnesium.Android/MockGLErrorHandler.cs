using System;
using Magnesium.OpenGL;

namespace HelloMagnesium.Android
{
	class MockGLErrorHandler : IGLErrorHandler
	{
		public void CheckGLError()
		{
			throw new NotImplementedException();
		}

		public void LogGLError(string location)
		{
			throw new NotImplementedException();
		}
	}
}