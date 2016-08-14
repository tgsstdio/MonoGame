using System;
using Magnesium;
using Magnesium.OpenGL;

namespace HelloMagnesium.Android
{
	class MockGLShaderModuleEntrypoint : IGLShaderModuleEntrypoint
	{
		public bool CheckUniformLocation(int programId, int location)
		{
			throw new NotImplementedException();
		}

		public int CompileProgram(MgGraphicsPipelineCreateInfo info)
		{
			throw new NotImplementedException();
		}

		public void DeleteShaderModule(int module)
		{
			throw new NotImplementedException();
		}

		public int GetActiveUniforms(int programId)
		{
			throw new NotImplementedException();
		}
	}
}