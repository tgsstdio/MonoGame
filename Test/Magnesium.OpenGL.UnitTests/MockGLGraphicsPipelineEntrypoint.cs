
using System;

namespace Magnesium.OpenGL.UnitTests
{
	public class MockGLGraphicsPipelineEntrypoint : IGLGraphicsPipelineEntrypoint
	{
		public void AttachShaderToProgram(int programID, int shader)
		{
			throw new NotImplementedException();
		}

		public bool CheckUniformLocation(int programId, int location)
		{
			throw new NotImplementedException();
		}

		public void CompileProgram(int programID)
		{
			throw new NotImplementedException();
		}

		public int CreateProgram()
		{
			throw new NotImplementedException();
		}
		#region IGLGraphicsPipelineEntrypoint implementation
		public void DeleteProgram (int programID)
		{
			
		}

		public int GetActiveUniforms(int programId)
		{
			throw new NotImplementedException();
		}

		public string GetCompilerMessages(int programID)
		{
			throw new NotImplementedException();
		}

		public bool HasCompilerMessages(int programID)
		{
			throw new NotImplementedException();
		}

		public bool IsCompiled(int programID)
		{
			throw new NotImplementedException();
		}
		#endregion
	}
}

