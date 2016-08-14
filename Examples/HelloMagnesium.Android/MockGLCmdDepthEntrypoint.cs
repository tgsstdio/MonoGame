using System;
using Magnesium;
using Magnesium.OpenGL;

namespace HelloMagnesium.Android
{
	class MockGLCmdDepthEntrypoint : IGLCmdDepthEntrypoint
	{
		public bool IsDepthBufferEnabled
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public void DisableDepthBuffer()
		{
			throw new NotImplementedException();
		}

		public void EnableDepthBuffer()
		{
			throw new NotImplementedException();
		}

		public GLGraphicsPipelineDepthState GetDefaultEnums()
		{
			throw new NotImplementedException();
		}

		public GLGraphicsPipelineDepthState Initialize()
		{
			throw new NotImplementedException();
		}

		public void SetClipControl(bool usingLowerLeftCorner, bool zeroToOneRange)
		{
			throw new NotImplementedException();
		}

		public void SetDepthBufferFunc(MgCompareOp func)
		{
			throw new NotImplementedException();
		}

		public void SetDepthMask(bool isMaskOn)
		{
			throw new NotImplementedException();
		}
	}
}