using System;

namespace MonoGame.Graphics
{
	public class DisplayPropertiesKHR
	{
		public DisplayKHR Display { get; set; }
		public String DisplayName { get; set; }
		public Extent2D PhysicalDimensions { get; set; }
		public Extent2D PhysicalResolution { get; set; }
		public SurfaceTransformFlagBitsKHR SupportedTransforms { get; set; }
		public bool PlaneReorderPossible { get; set; }
		public bool PersistentContent { get; set; }
	}
}

