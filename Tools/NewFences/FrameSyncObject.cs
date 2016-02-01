using System;

namespace NewFences
{
	public class FrameSyncObject : IFrameSyncObject
	{
		#region IFrameSyncObject implementation

		public void Lock (int index)
		{
			throw new NotImplementedException ();
		}

		public void WaitForGPU (int index)
		{
			throw new NotImplementedException ();
		}

		#endregion
		
	}
}
