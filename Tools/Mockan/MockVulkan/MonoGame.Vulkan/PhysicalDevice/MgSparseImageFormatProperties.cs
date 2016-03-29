using System;

namespace MonoGame.Graphics
{
	public class MgSparseImageFormatProperties
	{
		public MgImageAspectFlagBits AspectMask { get; set; }
		public MgExtent3D ImageGranularity { get; set; }
		public MgSparseImageFormatFlagBits Flags { get; set; }
	}
}

