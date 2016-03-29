using System;

namespace MonoGame.Graphics
{
	public class MgImageViewCreateInfo
	{
		public UInt32 Flags { get; set; }
		public MgImage Image { get; set; }
		public MgImageViewType ViewType { get; set; }
		public MgFormat Format { get; set; }
		public MgComponentMapping Components { get; set; }
		public MgImageSubresourceRange SubresourceRange { get; set; }
	}
}

