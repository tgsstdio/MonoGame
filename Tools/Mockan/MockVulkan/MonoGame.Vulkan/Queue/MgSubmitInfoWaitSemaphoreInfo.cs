using System;

namespace MonoGame.Graphics
{
	public class MgSubmitInfoWaitSemaphoreInfo
	{
		public MgSemaphore WaitSemaphore { get; set; }
		public MgPipelineStageFlagBits WaitDstStageMask { get; set;}
	}
}

