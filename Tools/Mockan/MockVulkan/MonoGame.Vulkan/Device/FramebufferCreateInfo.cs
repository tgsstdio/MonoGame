using System;

namespace MonoGame.Graphics.Vk
{
	public class FramebufferCreateInfo
	{
		public UInt32 Flags { get; set; }
		public RenderPass RenderPass { get; set; }
		public ImageView[] Attachments { get; set; }
		public UInt32 Width { get; set; }
		public UInt32 Height { get; set; }
		public UInt32 Layers { get; set; }
	}

}

