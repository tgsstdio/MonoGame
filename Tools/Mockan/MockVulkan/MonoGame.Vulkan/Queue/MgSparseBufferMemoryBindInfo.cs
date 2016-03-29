using System;

namespace MonoGame.Graphics
{
	public class MgSparseBufferMemoryBindInfo
	{
		public MgBuffer Buffer { get; set; }
		public UInt32 BindCount { get; set; }
		public MgSparseMemoryBind[] Binds { get; set; }
	}
}

