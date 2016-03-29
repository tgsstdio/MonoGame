using System;

namespace MonoGame.Graphics.Vk
{
	public class ClearRect
	{
		public Rect2D Rect { get; set; }
		public UInt32 BaseArrayLayer { get; set; }
		public UInt32 LayerCount { get; set; }
	}
}

