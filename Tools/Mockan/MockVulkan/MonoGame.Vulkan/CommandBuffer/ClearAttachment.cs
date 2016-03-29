using System;

namespace MonoGame.Graphics
{
	public class ClearAttachment
	{
		public ImageAspectFlagBits AspectMask { get; set; }
		public UInt32 ColorAttachment { get; set; }
		public ClearValue ClearValue { get; set; }
	}
}

