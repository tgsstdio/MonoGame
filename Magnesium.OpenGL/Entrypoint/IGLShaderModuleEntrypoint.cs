namespace Magnesium.OpenGL
{
	public interface IGLShaderModuleEntrypoint
	{
		bool CheckUniformLocation (int programId, int location);

		/// int CompileProgram (MgGraphicsPipelineCreateInfo info);

		int GetActiveUniforms (int programId);

		int CreateShaderModule(MgShaderStageFlagBits stage);
		void CompileShaderModule(int module, string sourceCode);

		void DeleteShaderModule (int module);
}
}

