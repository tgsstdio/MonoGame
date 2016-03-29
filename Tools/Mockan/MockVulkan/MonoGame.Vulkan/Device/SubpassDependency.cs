using System;

namespace MonoGame.Graphics
{
	public class SubpassDependency
	{
		public UInt32 SrcSubpass { get; set; }
		public UInt32 DstSubpass { get; set; }
		public PipelineStageFlagBits SrcStageMask { get; set; }
		public PipelineStageFlagBits DstStageMask { get; set; }
		public AccessFlagBits SrcAccessMask { get; set; }
		public AccessFlagBits DstAccessMask { get; set; }
		public DependencyFlagBits DependencyFlags { get; set; }
	}
}

