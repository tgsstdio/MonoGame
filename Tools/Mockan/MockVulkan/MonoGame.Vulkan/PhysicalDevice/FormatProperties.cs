using System;

namespace MonoGame.Graphics.Vk
{
	public class FormatProperties
	{
		public FormatFeatureFlagBits LinearTilingFeatures { get; set; }
		public FormatFeatureFlagBits OptimalTilingFeatures { get; set; }
		public FormatFeatureFlagBits BufferFeatures { get; set; }
	}
}

