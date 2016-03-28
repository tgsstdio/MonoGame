using System;

namespace MonoGame.Graphics.Vk
{
	public class DisplaySurfaceCreateInfoKHR
	{
		public UInt32 Flags { get; set; }
		public DisplayModeKHR DisplayMode { get; set; }
		public UInt32 PlaneIndex { get; set; }
		public UInt32 PlaneStackIndex { get; set; }
		public SurfaceTransformFlagBitsKHR Transform { get; set; }
		public float GlobalAlpha { get; set; }
		public DisplayPlaneAlphaFlagBitsKHR AlphaMode { get; set; }
		public Extent2D ImageExtent { get; set; }
	}
}

