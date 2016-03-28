using System;

namespace MonoGame.Graphics.Vk
{
	public class ImageViewCreateInfo
	{
		public UInt32 Flags { get; set; }
		public Image Image { get; set; }
		public ImageViewType ViewType { get; set; }
		public Format Format { get; set; }
		public ComponentMapping Components { get; set; }
		public ImageSubresourceRange SubresourceRange { get; set; }
	}
}

