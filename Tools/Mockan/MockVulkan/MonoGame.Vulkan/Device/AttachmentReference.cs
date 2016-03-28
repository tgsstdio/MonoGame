using System;

namespace MonoGame.Graphics.Vk
{
	public class AttachmentReference
	{
		public UInt32 Attachment { get; set; }
		public ImageLayout Layout { get; set; }
	}
}

