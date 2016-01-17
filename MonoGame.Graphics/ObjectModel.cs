using MonoGame.Content;

namespace MonoGame.Graphics
{
	public class ObjectModel
	{
		public AssetIdentifier AssetId { get; set; }
		public Mesh[] Meshes {get;set;}
		public StateGroup[] DefaultStates { get; set; }
	}
}

