using System;
using Magnesium;
using Magnesium.OpenGL;

namespace HelloMagnesium.Android
{
	class MockGLCmdBlendEntrypoint : IGLCmdBlendEntrypoint
	{
		public void ApplyBlendSeparateFunction(uint index, MgBlendFactor colorSource, MgBlendFactor colorDest, MgBlendFactor alphaSource, MgBlendFactor alphaDest)
		{
			throw new NotImplementedException();
		}

		public void EnableBlending(uint index, bool value)
		{
			throw new NotImplementedException();
		}

		public void EnableLogicOp(bool logicOpEnable)
		{
			throw new NotImplementedException();
		}

		public GLGraphicsPipelineBlendColorState Initialize(uint noOfAttachments)
		{
			throw new NotImplementedException();
		}

		public bool IsEnabled(uint index)
		{
			throw new NotImplementedException();
		}

		public void LogicOp(MgLogicOp logicOp)
		{
			throw new NotImplementedException();
		}

		public void SetColorMask(uint index, MgColorComponentFlagBits colorMask)
		{
			throw new NotImplementedException();
		}
	}
}