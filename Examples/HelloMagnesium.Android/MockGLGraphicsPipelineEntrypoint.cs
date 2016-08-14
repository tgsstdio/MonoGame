using System;
using Magnesium.OpenGL;

namespace HelloMagnesium.Android
{
	class MockGLGraphicsPipelineEntrypoint : IGLGraphicsPipelineEntrypoint
	{
		public void DeleteProgram(int programID)
		{
			throw new NotImplementedException();
		}
	}
}