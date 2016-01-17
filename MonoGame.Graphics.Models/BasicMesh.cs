using Microsoft.Xna.Framework;

namespace MonoGame.Models
{
	public class BasicMesh
	{
		public class MeshVertex
		{
			public Vector3 Position { get; set; }
			public Vector3 Normal { get; set; }
			public Vector2 UV { get; set; }
		}

		public class MeshFace
		{
			public uint[] Indices { get; set; }
		}

		public MeshVertex[] Vertices { get; set; }

		public MeshFace[] Faces { get; set; }
	}
}

