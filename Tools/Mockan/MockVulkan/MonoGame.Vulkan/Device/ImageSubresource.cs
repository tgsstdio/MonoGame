using System;

namespace MonoGame.Graphics
{
	public class ImageSubresource
	{
		public ImageAspectFlagBits AspectMask { get; set; }
		public UInt32 MipLevel { get; set; }
		public UInt32 ArrayLayer { get; set; }
	}
}

