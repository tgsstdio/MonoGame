using System;

namespace MonoGame.Graphics.Vk
{
	public class SpecializationInfo
	{
		public SpecializationMapEntry[] MapEntries { get; set; }
		public UIntPtr DataSize { get; set; }
		public IntPtr Data { get; set; }
	}
}

