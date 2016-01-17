namespace MonoGame.Graphics
{
	public class RenderPass
	{
		public uint InstanceID {get;set;}
		public SyncObject[] Requirements { get; set; }
		public DrawItem[] Items {get;set;}
		public ImageTarget[] Outputs {get;set;}
		public SyncObject Fence {get;set;}
	}
}

