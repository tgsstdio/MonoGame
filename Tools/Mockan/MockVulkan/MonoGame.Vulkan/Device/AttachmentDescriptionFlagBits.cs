using System;

namespace MonoGame.Graphics
{
	[Flags] 
	public enum AttachmentDescriptionFlagBits : byte
	{
		// The attachment may alias physical memory of another attachment in the same render pass
		ATTACHMENT_DESCRIPTION_MAY_ALIAS_BIT = 1 << 0,
	}
}

