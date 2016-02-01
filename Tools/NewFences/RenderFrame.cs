using System;

namespace NewFences
{
	public class RenderFrame
	{
		public IFrameSyncObject Fence {
			get;
			set;
		}

		public RenderPass[] Passes {
			get;
			set;
		}
	}

}
