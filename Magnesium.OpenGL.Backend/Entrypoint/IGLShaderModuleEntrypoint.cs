namespace Magnesium.OpenGL
{
	public interface IGLShaderModuleEntrypoint
	{
		bool CheckUniformLocation (int programId, int location);

		int CompileProgram (MgGraphicsPipelineCreateInfo info);

		int GetActiveUniforms (int programId);

		void DeleteShaderModule (int module);
	}
}

