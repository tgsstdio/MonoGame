using System;

namespace MonoGame.Graphics
{
	public class SubpassDescription
	{
		public UInt32 Flags { get; set; }
		public PipelineBindPoint PipelineBindPoint { get; set; }
		public UInt32 InputAttachmentCount { get; set; }
		public AttachmentReference[] InputAttachments { get; set; }
		public UInt32 ColorAttachmentCount { get; set; }
		public AttachmentReference[] ColorAttachments { get; set; }
		public AttachmentReference[] ResolveAttachments { get; set; }
		public AttachmentReference DepthStencilAttachment { get; set; }
		public UInt32 PreserveAttachmentCount { get; set; }
		public UInt32[] PreserveAttachments { get; set; }
	}
}

