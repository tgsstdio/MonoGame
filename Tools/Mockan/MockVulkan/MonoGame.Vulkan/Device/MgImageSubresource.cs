using System;

namespace MonoGame.Graphics
{
	public class MgImageSubresource
	{
		public MgImageAspectFlagBits AspectMask { get; set; }
		public UInt32 MipLevel { get; set; }
		public UInt32 ArrayLayer { get; set; }
	}
}

