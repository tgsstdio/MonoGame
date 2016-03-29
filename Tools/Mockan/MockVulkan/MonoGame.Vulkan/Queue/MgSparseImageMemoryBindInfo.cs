using System;

namespace MonoGame.Graphics
{
	public class MgSparseImageMemoryBindInfo
	{
		public MgImage Image { get; set; }
		public UInt32 BindCount { get; set; }
		public MgSparseImageMemoryBind[] Binds { get; set; }
	}
}

