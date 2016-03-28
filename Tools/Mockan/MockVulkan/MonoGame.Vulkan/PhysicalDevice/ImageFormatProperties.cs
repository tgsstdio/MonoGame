using System;

namespace MonoGame.Graphics.Vk
{
	public class ImageFormatProperties
	{
		public Extent3D MaxExtent { get; set; }
		public UInt32 MaxMipLevels { get; set; }
		public UInt32 MaxArrayLayers { get; set; }
		public SampleCountFlagBits SampleCounts { get; set; }
		public UInt64 MaxResourceSize { get; set; }
	}
}

