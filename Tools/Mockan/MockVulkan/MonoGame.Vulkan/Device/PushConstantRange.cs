using System;

namespace MonoGame.Graphics
{
	public class PushConstantRange
	{
		public ShaderStageFlagBits StageFlags { get; set; }
		public UInt32 Offset { get; set; }
		public UInt32 Size { get; set; }
	}
}

