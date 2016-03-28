using System;

namespace MonoGame.Graphics.Vk
{
	public class SpecializationMapEntry
	{
		public UInt32 ConstantID { get; set; }
		public UInt32 Offset { get; set; }
		public UIntPtr Size { get; set; }
	}
}

