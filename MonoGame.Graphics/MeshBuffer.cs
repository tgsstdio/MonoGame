namespace MonoGame.Graphics
{
	/// <summary>
	///  Graphics implementation of index and vertex buffer object
	/// and fence
	/// </summary>
	public class MeshBuffer : IMeshBuffer
	{
		public int BufferId {
			get {
				throw new System.NotImplementedException ();
			}
		}

		public int VBO { get; set;}
		public int IndexObject {get;set;}
		public int VertexObject {get;set;}
		public SyncObject Fence {get;set;}
		public float Factor {get;set;}
	}
}

