using MonoGame.Content;

namespace MonoGame.Graphics.AZDO
{
	public class Mesh
	{
		public int MeshIndex { get; private set; }
		public ObjectModel Parent { get; set; }		
		public BlockIdentifier Block { get; set; }
		public VertexLayout InternalFormat { get; set; }
	}
}

