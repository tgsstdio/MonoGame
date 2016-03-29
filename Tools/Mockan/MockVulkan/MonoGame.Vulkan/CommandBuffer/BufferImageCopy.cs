using System;

namespace MonoGame.Graphics.Vk
{
	public class BufferImageCopy
	{
		public UInt64 BufferOffset { get; set; }
		public UInt32 BufferRowLength { get; set; }
		public UInt32 BufferImageHeight { get; set; }
		public ImageSubresourceLayers ImageSubresource { get; set; }
		public Offset3D ImageOffset { get; set; }
		public Extent3D ImageExtent { get; set; }
	}
}

