using System;

namespace MonoGame.Graphics
{
	using VkImage = MonoGame.Graphics.Image;

	public class ImageMemoryBarrier
	{
		public AccessFlagBits SrcAccessMask { get; set; }
		public AccessFlagBits DstAccessMask { get; set; }
		public ImageLayout OldLayout { get; set; }
		public ImageLayout NewLayout { get; set; }
		public UInt32 SrcQueueFamilyIndex { get; set; }
		public UInt32 DstQueueFamilyIndex { get; set; }
		public VkImage Image { get; set; }
		public ImageSubresourceRange SubresourceRange { get; set; }
	}
}

