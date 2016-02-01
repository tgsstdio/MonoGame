namespace MonoGame.Shaders
{
	public interface IBinaryShaderProgramWriter
	{
		void Initialise(int bufSize);
		void WriteBinary(ShaderProgram program);
	}
}

