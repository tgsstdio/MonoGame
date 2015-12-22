using System;

namespace StrawHat
{
	public class ResourceList
	{
		public struct Key
		{
			public long Handle;
			public int Index;
			public float Slice;
		}

		public Key[] Resources {get;set;}
	}
}

