using System;

namespace MonoGame.Graphics.Vk
{
	// Device
	public class DescriptorImageInfo
	{
		public Sampler Sampler { get; set; }
		public ImageView ImageView { get; set; }
		public ImageLayout ImageLayout { get; set; }
	}
}

