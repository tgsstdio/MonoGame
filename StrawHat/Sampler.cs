using System;

namespace StrawHat
{
	public class Sampler
	{
		public struct Key
		{
			public long Handle;
			public int Index;
			public float Slice;
		}

		public int Index {get;set;}
		public int TextureId { get; set;}
		public Key Slot {get;set;}
	}
}

