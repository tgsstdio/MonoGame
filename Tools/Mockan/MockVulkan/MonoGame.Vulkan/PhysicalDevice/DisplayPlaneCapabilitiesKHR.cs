namespace MonoGame.Graphics.Vk
{
	public class DisplayPlaneCapabilitiesKHR
	{
		public DisplayPlaneAlphaFlagBitsKHR SupportedAlpha { get; set; }
		public Offset2D MinSrcPosition { get; set; }
		public Offset2D MaxSrcPosition { get; set; }
		public Extent2D MinSrcExtent { get; set; }
		public Extent2D MaxSrcExtent { get; set; }
		public Offset2D MinDstPosition { get; set; }
		public Offset2D MaxDstPosition { get; set; }
		public Extent2D MinDstExtent { get; set; }
		public Extent2D MaxDstExtent { get; set; }
	}
}

