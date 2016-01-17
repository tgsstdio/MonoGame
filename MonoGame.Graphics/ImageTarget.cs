namespace MonoGame.Graphics
{
	public class ImageTarget
	{
		public byte TargetIndex { get; set; }
		public RenderPass Origin { get; set; }
		public SyncObject Fence { get; set; }
	}
}

