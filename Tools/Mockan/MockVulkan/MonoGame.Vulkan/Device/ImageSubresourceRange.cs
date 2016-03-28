using System;

namespace MonoGame.Graphics.Vk
{
	public class ImageSubresourceRange
	{
		public ImageAspectFlagBits AspectMask { get; set; }
		public UInt32 BaseMipLevel { get; set; }
		public UInt32 LevelCount { get; set; }
		public UInt32 BaseArrayLayer { get; set; }
		public UInt32 LayerCount { get; set; }
	}
}

