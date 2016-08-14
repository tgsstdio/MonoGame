using System;
using Magnesium.OpenGL;

namespace HelloMagnesium.Android
{
	class MockGLCmdScissorsEntrypoint : IGLCmdScissorsEntrypoint
	{
		public void ApplyScissors(GLCmdScissorParameter scissors)
		{
			throw new NotImplementedException();
		}

		public void ApplyViewports(GLCmdViewportParameter viewports)
		{
			throw new NotImplementedException();
		}
	}
}