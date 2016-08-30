using System;
using OpenTK.Graphics.OpenGL;

namespace Magnesium.OpenGL.DesktopGL
{
	public class FullGLGraphicsPipelineEntrypoint : IGLGraphicsPipelineEntrypoint
	{
		public bool AreShadersLinkedCorrectly(int programID)
		{
			int linkStatus = 0;
			GL.GetProgram(programID, GetProgramParameterName.LinkStatus, out linkStatus);
			return (linkStatus == (int)All.True);
		}

		public void AttachShaderToProgram(int programID, int shader)
		{
			GL.AttachShader (programID, shader);
		}

		public void CompileProgram(int programID)
		{
			GL.LinkProgram(programID);
		}

		public int CreateProgram()
		{
			return GL.CreateProgram();
		}

		public void DeleteProgram(int programID)
		{
			GL.DeleteProgram(programID);
		}

		public bool HasCompilerMessages(int programID)
		{
			int glinfoLogLength = 0;
			GL.GetProgram(programID, GetProgramParameterName.InfoLogLength, out glinfoLogLength);
			return (glinfoLogLength > 1);
		}
	}
}

