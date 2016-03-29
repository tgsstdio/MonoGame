﻿using System;

namespace MonoGame.Graphics
{
	public class MgImageCopy
	{
		public MgImageSubresourceLayers SrcSubresource { get; set; }
		public MgOffset3D SrcOffset { get; set; }
		public MgImageSubresourceLayers DstSubresource { get; set; }
		public MgOffset3D DstOffset { get; set; }
		public MgExtent3D Extent { get; set; }
	}
}

