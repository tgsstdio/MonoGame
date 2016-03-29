namespace MonoGame.Graphics
{
	public class RenderPassBeginInfo
	{
		public RenderPass RenderPass { get; set; }
		public Framebuffer Framebuffer { get; set; }
		public Rect2D RenderArea { get; set; }
		public ClearValue[] ClearValues { get; set; }
	}
}

