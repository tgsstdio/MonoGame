namespace Magnesium.OpenGL
{
	public interface IGLGraphicsPipelineEntrypoint
	{
		void DeleteProgram (int programID);

		void CompileProgram(int programID);

		int CreateProgram();

		bool HasCompilerMessages(int programID);

		void AttachShaderToProgram(int programID, int shader);

		bool AreShadersLinkedCorrectly(int programID);
	}
}

