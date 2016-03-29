﻿using System;

namespace MonoGame.Graphics.Vk
{
	public class ImageResolve
	{
		public ImageSubresourceLayers SrcSubresource { get; set; }
		public Offset3D SrcOffset { get; set; }
		public ImageSubresourceLayers DstSubresource { get; set; }
		public Offset3D DstOffset { get; set; }
		public Extent3D Extent { get; set; }
	}
}

