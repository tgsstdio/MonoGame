namespace Magnesium.OpenGL
{
	public interface IShaderProgramCache
	{
		byte ProgramIndex { get; }
		void SetProgram (int programIndex);
		IShaderProgram GetActiveProgram ();
	}

}

