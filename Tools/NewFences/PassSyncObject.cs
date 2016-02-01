using System;
using MonoGame.Graphics;

namespace NewFences
{
	public class PassSyncObject : IPassSyncObject
	{
		#region IPassSyncObject implementation

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
