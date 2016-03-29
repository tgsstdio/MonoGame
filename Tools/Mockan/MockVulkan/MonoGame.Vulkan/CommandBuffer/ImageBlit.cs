using System;

namespace MonoGame.Graphics.Vk
{
	public class ImageBlit
	{
		public ImageSubresourceLayers SrcSubresource { get; set; }
		public Offset3D[] SrcOffsets { get; set; } // 2
		public ImageSubresourceLayers DstSubresource { get; set; }
		public Offset3D[] DstOffsets { get; set; } // 2
	}
}

