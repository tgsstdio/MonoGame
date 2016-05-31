using Magnesium.OpenGL;


namespace HelloMagnesium
{
	public class MockShaderProgramCache : IShaderProgramCache
	{
		#region IShaderProgramCache implementation
		public void SetProgram (ushort programIndex)
		{
			throw new System.NotImplementedException ();
		}
		public IShaderProgram GetActiveProgram ()
		{
			throw new System.NotImplementedException ();
		}
		public byte ProgramIndex {
			get;
		}
		#endregion
	}

}
