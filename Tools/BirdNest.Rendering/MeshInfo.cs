using BirdNest.MonoGame.Core;


namespace BirdNest.Rendering
{
	public class MeshInfo
	{
		public uint MeshIndex {get;set;}
		public AssetIdentifier Program {get;set;}
		public DrawState State {get;set;}
		public ModelUserFormat Format {get;set;}
		public ModelBufferUsage Usage {get;set;}
	}

}

