using System;

namespace MonoGame.Graphics
{
	public class SpecializationInfo
	{
		public SpecializationMapEntry[] MapEntries { get; set; }
		public UIntPtr DataSize { get; set; }
		public IntPtr Data { get; set; }
	}
}

