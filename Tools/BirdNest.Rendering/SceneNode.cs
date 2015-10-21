using BirdNest.MonoGame.Core;
using BirdNest.MonoGame.Graphics;

namespace BirdNest.Rendering
{
	public class SceneNode
	{
		public InstanceIdentifier Id { get; set; }
		public AssetIdentifier ModelId { get; set; }
		public ObjectModelInfo ObjectModel { get; set; }

		public MeshSceneNode[] Children {get;set;}
		// Add as many shader buffer objects info as components
		public LocationInfo Location {get;set;}
	}
}

