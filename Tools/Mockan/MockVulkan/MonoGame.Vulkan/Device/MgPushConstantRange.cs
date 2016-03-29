using System;

namespace MonoGame.Graphics
{
	public class MgPushConstantRange
	{
		public MgShaderStageFlagBits StageFlags { get; set; }
		public UInt32 Offset { get; set; }
		public UInt32 Size { get; set; }
	}
}

