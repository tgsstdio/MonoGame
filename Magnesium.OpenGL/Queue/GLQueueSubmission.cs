using System.Collections.Generic;

namespace Magnesium.OpenGL
{
	class GLQueueSubmission
	{
		public uint Key;
		public GLQueueSubmission (uint key, MgSubmitInfo sub)
		{
			Key = key;
			Waits = new List<ISyncObject> ();
			if (sub.WaitSemaphores != null)
			{
				foreach (var signal in sub.WaitSemaphores)
				{
					var semaphore = signal.WaitSemaphore as ISyncObject;
					if (semaphore != null)
					{
						Waits.Add (semaphore);
					}
				}
			}

			Signals = new List<ISyncObject> ();
			if (sub.SignalSemaphores != null)
			{
				foreach (var signal in sub.SignalSemaphores)
				{
					var semaphore = signal as ISyncObject;
					if (semaphore != null)
					{
						Signals.Add (semaphore);
					}
				}
			}
		}
		public List<ISyncObject> Waits { get;set; }

		public GLCommandBuffer[] CommandBuffers { get; set; }

		public List<ISyncObject> Signals { get; set; }
		public ISyncObject OrderFence { get; set; }
	}
}

