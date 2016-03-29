using System;

namespace MonoGame.Graphics
{
	public class ImageCopy
	{
		public ImageSubresourceLayers SrcSubresource { get; set; }
		public Offset3D SrcOffset { get; set; }
		public ImageSubresourceLayers DstSubresource { get; set; }
		public Offset3D DstOffset { get; set; }
		public Extent3D Extent { get; set; }
	}
}

