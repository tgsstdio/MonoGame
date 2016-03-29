using System;

namespace MonoGame.Graphics
{
	public class SwapchainCreateInfoKHR
	{
		public UInt32 Flags { get; set; }
		public SurfaceKHR Surface { get; set; }
		public UInt32 MinImageCount { get; set; }
		public Format ImageFormat { get; set; }
		public ColorSpaceKHR ImageColorSpace { get; set; }
		public Extent2D ImageExtent { get; set; }
		public UInt32 ImageArrayLayers { get; set; }
		public ImageUsageFlagBits ImageUsage { get; set; }
		public SharingMode ImageSharingMode { get; set; }
		public UInt32[] QueueFamilyIndices { get; set; }
		public SurfaceTransformFlagBitsKHR PreTransform { get; set; }
		public CompositeAlphaFlagBitsKHR CompositeAlpha { get; set; }
		public PresentModeKHR PresentMode { get; set; }
		public bool Clipped { get; set; }
		public SwapchainKHR OldSwapchain { get; set; }
	}

}

