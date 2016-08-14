using System;
using OpenTK.Graphics.OpenGL;

namespace Magnesium.OpenGL.DesktopGL
{
	public class FullGLShaderModuleEntrypoint : IGLShaderModuleEntrypoint
	{
		#region IGLShaderModuleEntrypoint implementation

		IGLErrorHandler mErrHandler;

		public FullGLShaderModuleEntrypoint (IGLErrorHandler errHandler)
		{
			mErrHandler = errHandler;
		}

		public int CompileProgram (MgGraphicsPipelineCreateInfo info)
		{
			throw new NotImplementedException ();
		}

		public bool CheckUniformLocation (int programId, int location)
		{
			int locationQuery;
			GL.Ext.GetUniform(programId, location, out locationQuery);
			mErrHandler.LogGLError ("FullGLShaderModuleEntrypoint.CheckUniformLocation");
			return (locationQuery != -1);
		}

		public int GetActiveUniforms (int programId)
		{
			int noOfActiveUniforms;
			GL.GetProgram (programId, GetProgramParameterName.ActiveUniforms, out noOfActiveUniforms);
			mErrHandler.LogGLError ("FullGLShaderModuleEntrypoint.GetActiveUniforms");
			return noOfActiveUniforms;
		}

		public void DeleteShaderModule (int module)
		{
			GL.DeleteShader(module);
		}

		#endregion
	}
}

