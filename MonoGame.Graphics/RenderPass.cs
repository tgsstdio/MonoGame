using System.Collections.Generic;

namespace MonoGame.Graphics
{
	public class RenderPass
	{
		public uint InstanceID {get;set;}
		public byte Effect {get;set;}
		public int Pass { get; set; }
		public ISyncObject[] Requirements { get; set; }
		public IList<DrawItem> Items {get;set;}
		public ImageTarget[] Outputs {get;set;}
		public ISyncObject Fence {get;set;}
	}
}

