using System;
using Magnesium;
using Magnesium.OpenGL;

namespace HelloMagnesium.Android
{
	class MockGLDeviceMemoryEntrypoint : IGLDeviceMemoryEntrypoint
	{
		public IGLDeviceMemory CreateDeviceMemory(MgMemoryAllocateInfo createInfo)
		{
			throw new NotImplementedException();
		}
	}
}