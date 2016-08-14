using System;
using Magnesium.OpenGL;

namespace HelloMagnesium.Android
{
	class MockGLImageDescriptorEntrypoint : IGLImageDescriptorEntrypoint
	{
		public ulong CreateHandle(int textureId, int samplerId)
		{
			throw new NotImplementedException();
		}

		public void ReleaseHandle(ulong handle)
		{
			throw new NotImplementedException();
		}
	}
}