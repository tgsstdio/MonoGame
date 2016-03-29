using System;

namespace MonoGame.Graphics
{
	public class MgSpecializationInfo
	{
		public MgSpecializationMapEntry[] MapEntries { get; set; }
		public UIntPtr DataSize { get; set; }
		public IntPtr Data { get; set; }
	}
}

