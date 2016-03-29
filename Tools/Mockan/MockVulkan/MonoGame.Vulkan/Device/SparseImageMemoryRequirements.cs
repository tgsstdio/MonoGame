using System;

namespace MonoGame.Graphics
{
	public class SparseImageMemoryRequirements
	{
		public SparseImageFormatProperties FormatProperties { get; set; }
		public UInt32 ImageMipTailFirstLod { get; set; }
		public UInt64 ImageMipTailSize { get; set; }
		public UInt64 ImageMipTailOffset { get; set; }
		public UInt64 ImageMipTailStride { get; set; }
	}
}

