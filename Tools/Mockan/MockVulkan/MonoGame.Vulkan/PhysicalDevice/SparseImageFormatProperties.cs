using System;

namespace MonoGame.Graphics.Vk
{
	public class SparseImageFormatProperties
	{
		public ImageAspectFlagBits AspectMask { get; set; }
		public Extent3D ImageGranularity { get; set; }
		public SparseImageFormatFlagBits Flags { get; set; }
	}
}

