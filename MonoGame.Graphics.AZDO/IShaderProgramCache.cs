namespace MonoGame.Graphics.AZDO
{
	public interface IShaderProgramCache
	{
		byte ProgramIndex { get; }
		void SetProgram (ushort programIndex);
		IShaderProgram GetActiveProgram ();
	}

}

