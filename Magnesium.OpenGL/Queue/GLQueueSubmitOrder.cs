using System.Collections.Generic;

namespace Magnesium.OpenGL
{
	public class GLQueueSubmitOrder
	{
		public uint Key;
		public Dictionary<uint, ISyncObject> Submissions { get;set; }
		public GLQueueFence Fence { get; set; }
	}
}

