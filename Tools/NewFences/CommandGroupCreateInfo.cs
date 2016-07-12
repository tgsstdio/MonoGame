using System;
using Magnesium;

namespace NewFences
{
	public class CommandGroupCreateInfo
	{
		public CommandGroupCreateInfo (IMgCommandBuffer cb, IMgFramebuffer fb, Action<IMgCommandBuffer, IMgFramebuffer> build)
		{
			CommandBuffer = cb;
			Framebuffer = fb;
			BuildAction = build;
		}

		#region Required fields 
		public IMgCommandBuffer CommandBuffer { get; private set; }
		public IMgFramebuffer Framebuffer { get; private set; }
		public Action<IMgCommandBuffer, IMgFramebuffer> BuildAction { get; private set; }
		#endregion Required fields 

		#region Optional fields 
		public MgSubmitInfoWaitSemaphoreInfo[] Waits { get; set;}
		public IMgSemaphore[] Signals {get; set;}
		public IMgFence Fence {get; set;}
		#endregion
	}
}

