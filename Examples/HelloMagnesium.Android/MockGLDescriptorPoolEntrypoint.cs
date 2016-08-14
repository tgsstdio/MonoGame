using System;
using Magnesium;
using Magnesium.OpenGL;

namespace HelloMagnesium.Android
{
	class MockGLDescriptorPoolEntrypoint : IGLDescriptorPoolEntrypoint
	{
		public IGLDescriptorPool CreatePool(MgDescriptorPoolCreateInfo createInfo)
		{
			throw new NotImplementedException();
		}
	}
}