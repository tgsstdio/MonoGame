using System;

namespace NewFences
{
	public class RenderFrame
	{
		public IFrameSyncObject Fence {
			get;
			set;
		}

		public IRenderPass[] Passes {
			get;
			set;
		}
	}

}
