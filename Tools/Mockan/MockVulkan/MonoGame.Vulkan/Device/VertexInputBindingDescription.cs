using System;

namespace MonoGame.Graphics.Vk
{
	public class VertexInputBindingDescription
	{
		public UInt32 Binding { get; set; }
		public UInt32 Stride { get; set; }
		public VertexInputRate InputRate { get; set; }
	}
}

