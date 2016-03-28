using System;

namespace MonoGame.Graphics.Vk
{
	public class VertexInputAttributeDescription
	{
		public UInt32 Location { get; set; }
		public UInt32 Binding { get; set; }
		public Format Format { get; set; }
		public UInt32 Offset { get; set; }
	}
}

