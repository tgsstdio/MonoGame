using System;
using Magnesium;
using Magnesium.OpenGL;

namespace HelloMagnesium.Android
{
	class MockGLCmdClearEntrypoint : IGLCmdClearEntrypoint
	{
		public void ClearBuffers(GLQueueClearBufferMask combinedMask)
		{
			throw new NotImplementedException();
		}

		public GLQueueRendererClearValueState Initialize()
		{
			throw new NotImplementedException();
		}

		public void SetClearColor(MgColor4f clearValue)
		{
			throw new NotImplementedException();
		}

		public void SetClearDepthValue(float value)
		{
			throw new NotImplementedException();
		}

		public void SetClearStencilValue(uint stencil)
		{
			throw new NotImplementedException();
		}
	}
}