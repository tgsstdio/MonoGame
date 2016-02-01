using System;
using MonoGame.Graphics;

namespace NewFences
{
	public class BufferSyncObject : IBufferSyncObject
	{
		#region IBufferSyncObject implementation

		public int LastPass {
			get;
			set;
		}

		public void Bind (int passId)
		{
			if (LastPass > passId)
			{
				LastPass = passId;
			}
		}

		public void Lock (int index)
		{
			// only lock when last pass 
			// ASSUMPTION: pass id is equivalent to order
		}

		public void WaitForGPU (int index)
		{
			throw new NotImplementedException ();
		}

		#endregion
	}

}
