namespace MonoGame.Graphics.AZDO
{
	public interface IShaderProgramCache
	{
		byte ProgramIndex { get; }
		void SetProgram (byte programIndex);
		IShaderProgram GetActiveProgram ();
	}

}

