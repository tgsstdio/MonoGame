using System;

namespace MonoGame.Graphics
{
	public class RenderPassCreateInfo
	{
		public UInt32 Flags { get; set; }
		public AttachmentDescription[] Attachments { get; set; }
		public SubpassDescription[] Subpasses { get; set; }
		public SubpassDependency[] Dependencies { get; set; }
	}
}

