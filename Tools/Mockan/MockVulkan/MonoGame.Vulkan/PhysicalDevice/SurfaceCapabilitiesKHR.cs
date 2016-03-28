using System;

namespace MonoGame.Graphics.Vk
{
	public class SurfaceCapabilitiesKHR
	{
		public UInt32 MinImageCount { get; set; }
		public UInt32 MaxImageCount { get; set; }
		public Extent2D CurrentExtent { get; set; }
		public Extent2D MinImageExtent { get; set; }
		public Extent2D MaxImageExtent { get; set; }
		public UInt32 MaxImageArrayLayers { get; set; }
		public SurfaceTransformFlagBitsKHR SupportedTransforms { get; set; }
		public SurfaceTransformFlagBitsKHR CurrentTransform { get; set; }
		public CompositeAlphaFlagBitsKHR SupportedCompositeAlpha { get; set; }
		public ImageUsageFlagBits SupportedUsageFlags { get; set; }
	}

}

