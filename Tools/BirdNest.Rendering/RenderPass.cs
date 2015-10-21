using BirdNest.MonoGame.Core;

namespace BirdNest.Rendering
{
	public class RenderPass
	{
		public SceneNode Origin { get; set; }
		public AssetIdentifier Program {get;set;}
		public AssetIdentifier Model { get; set; }
		public BlockIdentifier Block { get; set; }
		public uint MeshIndex { get; set; }
		public ModelBufferUsage Usage { get; set; }
		public ModelUserFormat Format { get; set; }
		public DrawState State {get;set;}
	}
}

