using System;

namespace MonoGame.Graphics
{
	public class MgClearAttachment
	{
		public MgImageAspectFlagBits AspectMask { get; set; }
		public UInt32 ColorAttachment { get; set; }
		public MgClearValue ClearValue { get; set; }
	}
}

