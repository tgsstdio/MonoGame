using BirdNest.MonoGame.Core;

namespace BirdNest.MonoGame.Graphics
{
	public class ShaderProgramData
	{
		public AssetIdentifier Identifier { get; set; }
		public BlockIdentifier Block {get;set;}
		public bool IsLoaded { get; set; }
		public int ProgramID { get; set; }
	}
}

