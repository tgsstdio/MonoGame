using System;

namespace MonoGame.Graphics
{
	public class AttachmentDescription
	{
		public AttachmentDescriptionFlagBits Flags { get; set; }
		public Format Format { get; set; }
		public SampleCountFlagBits Samples { get; set; }
		public AttachmentLoadOp LoadOp { get; set; }
		public AttachmentStoreOp StoreOp { get; set; }
		public AttachmentLoadOp StencilLoadOp { get; set; }
		public AttachmentStoreOp StencilStoreOp { get; set; }
		public ImageLayout InitialLayout { get; set; }
		public ImageLayout FinalLayout { get; set; }
	}
}

