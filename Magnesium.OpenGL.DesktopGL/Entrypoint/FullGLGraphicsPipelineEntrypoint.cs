using OpenTK.Graphics.OpenGL;

namespace Magnesium.OpenGL.DesktopGL
{
	public class FullGLGraphicsPipelineEntrypoint : IGLGraphicsPipelineEntrypoint
	{
		public void DeleteProgram(int programID)
		{
			GL.DeleteProgram(programID);
		}
	}
}

