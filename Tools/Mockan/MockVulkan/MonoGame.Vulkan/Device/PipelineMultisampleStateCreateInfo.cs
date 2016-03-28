using System;

namespace MonoGame.Graphics.Vk
{
	public class PipelineMultisampleStateCreateInfo
	{
		public UInt32 Flags { get; set; }
		public SampleCountFlagBits RasterizationSamples { get; set; }
		public bool SampleShadingEnable { get; set; }
		public float MinSampleShading { get; set; }
		public UInt32[] SampleMask { get; set; }
		public bool AlphaToCoverageEnable { get; set; }
		public bool AlphaToOneEnable { get; set; }
	}
}

