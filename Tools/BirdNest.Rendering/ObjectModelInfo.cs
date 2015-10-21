using BirdNest.MonoGame.Core;

namespace BirdNest.Rendering
{
	public class ObjectModelInfo
	{
		public AssetInfo Asset { get; set; }	
		public MeshInfo[] Meshes { get; set; }
		public MeshData InternalData {get; set;}
	}
}

