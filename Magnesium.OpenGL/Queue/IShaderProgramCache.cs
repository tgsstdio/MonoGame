namespace Magnesium.OpenGL
{
	public interface IShaderProgramCache
	{
		byte ProgramIndex { get; }
		void SetProgram (ushort programIndex);
		IShaderProgram GetActiveProgram ();
	}

}

