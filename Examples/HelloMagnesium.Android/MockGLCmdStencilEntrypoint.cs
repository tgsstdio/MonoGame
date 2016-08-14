using System;
using Magnesium;
using Magnesium.OpenGL;

namespace HelloMagnesium.Android
{
	class MockGLCmdStencilEntrypoint : IGLCmdStencilEntrypoint
	{
		public bool IsStencilBufferEnabled
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public void DisableStencilBuffer()
		{
			throw new NotImplementedException();
		}

		public void EnableStencilBuffer()
		{
			throw new NotImplementedException();
		}

		public GLGraphicsPipelineStencilState GetDefaultEnums()
		{
			throw new NotImplementedException();
		}

		public GLQueueRendererStencilState Initialize()
		{
			throw new NotImplementedException();
		}

		public void SetBackFaceCullStencilFunction(MgCompareOp func, int referenceStencil, int stencilMask)
		{
			throw new NotImplementedException();
		}

		public void SetBackFaceStencilOperation(MgStencilOp stencilFail, MgStencilOp stencilDepthBufferFail, MgStencilOp stencilPass)
		{
			throw new NotImplementedException();
		}

		public void SetFrontFaceCullStencilFunction(MgCompareOp func, int referenceStencil, int stencilMask)
		{
			throw new NotImplementedException();
		}

		public void SetFrontFaceStencilOperation(MgStencilOp stencilFail, MgStencilOp stencilDepthBufferFail, MgStencilOp stencilPass)
		{
			throw new NotImplementedException();
		}

		public void SetStencilFunction(MgCompareOp stencilFunction, int referenceStencil, int stencilMask)
		{
			throw new NotImplementedException();
		}

		public void SetStencilOperation(MgStencilOp stencilFail, MgStencilOp stencilDepthBufferFail, MgStencilOp stencilPass)
		{
			throw new NotImplementedException();
		}

		public void SetStencilWriteMask(int mask)
		{
			throw new NotImplementedException();
		}
	}
}