using System;

namespace MonoGame.Graphics.Vk
{
	public class ImageCreateInfo
	{
		public ImageCreateFlagBits Flags { get; set; }
		public ImageType ImageType { get; set; }
		public Format Format { get; set; }
		public Extent3D Extent { get; set; }
		public UInt32 MipLevels { get; set; }
		public UInt32 ArrayLayers { get; set; }
		public SampleCountFlagBits Samples { get; set; }
		public ImageTiling Tiling { get; set; }
		public UInt32 Usage { get; set; }
		public SharingMode SharingMode { get; set; }
		public UInt32[] QueueFamilyIndices { get; set; }
		public ImageLayout InitialLayout { get; set; }
	}
}

