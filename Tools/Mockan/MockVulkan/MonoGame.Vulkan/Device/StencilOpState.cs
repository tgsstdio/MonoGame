using System;

namespace MonoGame.Graphics.Vk
{
	public class StencilOpState
	{
		public StencilOp FailOp { get; set; }
		public StencilOp PassOp { get; set; }
		public StencilOp DepthFailOp { get; set; }
		public CompareOp CompareOp { get; set; }
		public UInt32 CompareMask { get; set; }
		public UInt32 WriteMask { get; set; }
		public UInt32 Reference { get; set; }
	}
}

