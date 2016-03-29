using System;

namespace MonoGame.Graphics.Vk
{
	public class ImageSubresourceLayers
	{
		public ImageAspectFlagBits AspectMask { get; set; }
		public UInt32 MipLevel { get; set; }
		public UInt32 BaseArrayLayer { get; set; }
		public UInt32 LayerCount { get; set; }
	}
}

