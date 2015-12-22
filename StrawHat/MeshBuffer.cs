using System;

namespace StrawHat
{
	/// <summary>
	///  Graphics implementation of index and vertex buffer object
	/// and fence
	/// </summary>
	public class MeshBuffer
	{
		public int VBO { get; set;}
		public int IndexObject {get;set;}
		public int VertexObject {get;set;}
		public SyncObject Fence {get;set;}
		public float Factor {get;set;}
	}
}

