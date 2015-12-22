using System;

namespace StrawHat
{
	public class MeshMarker
	{
		public int Index {get;set;}
		public DrawPrimitive PrimitiveType {get;set;}
		public VertexLayout Definition { get; set;}
		public int Offset {get;set;}
		public int Count {get;set;}
	}
}

