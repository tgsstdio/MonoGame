using System;
using OpenTK.Graphics.OpenGL;

namespace MonoGame.Graphics.AZDO
{
	/// <summary>
	/// Graphic implementation of sync object.
	/// </summary>
	public class FullSyncObject : ISyncObject, IDisposable
	{
		public int Index {get;set;}
		public IntPtr ObjectPtr {get;set;}
		public float Timeout {get;set;}

		public FullSyncObject ()
		{
			TotalBlockingWaits = 0;
			ObjectPtr = IntPtr.Zero;
		}

		public bool IsWaiting { get; private set; }
		public void Reset()
		{
			IsWaiting = false;
			if (ObjectPtr != IntPtr.Zero)
			{
				GL.DeleteSync (ObjectPtr);
			}
		}

		public void BeginSync()
		{
			ObjectPtr = GL.FenceSync (SyncCondition.SyncGpuCommandsComplete, 0);
			IsWaiting = true;
		}

		private bool NonBlockingWait ()
		{
			// only on the first time
			ClientWaitSyncFlags waitOption = ClientWaitSyncFlags.SyncFlushCommandsBit;

			WaitSyncStatus result = GL.ClientWaitSync (ObjectPtr, waitOption, 0);
			waitOption = ClientWaitSyncFlags.None;
			if (result == WaitSyncStatus.WaitFailed)
			{
				throw new InvalidOperationException ("GPU NonBlockingWait sync failed - surplus actions incomplete");
			}
			return !(result == WaitSyncStatus.ConditionSatisfied || result == WaitSyncStatus.AlreadySignaled);
		}

		private bool BlockingWait ()
		{
			int times = 0;
			++TotalBlockingWaits;
			do
			{
				WaitSyncStatus status = GL.ClientWaitSync (ObjectPtr, ClientWaitSyncFlags.None, Duration);
				// BLOCKING WAITING 
				if (status == WaitSyncStatus.WaitFailed)
				{
					throw new InvalidOperationException ("GPU BlockingWait sync failed - surplus actions completed");
				} 
				else if (status == WaitSyncStatus.ConditionSatisfied || status == WaitSyncStatus.AlreadySignaled)
				{
					return false;
				}
				++times;
			}
			while (times < NoOfRetries);

			++TotalFailures;
			// still waiting
			return true;
		}

		#region IDisposable implementation

		public void Dispose ()
		{
			if (ObjectPtr != IntPtr.Zero)
			{
				GL.DeleteSync (ObjectPtr);
			}
		}

		#endregion

		#region ISyncObject implementation

		public uint TotalFailures {
			get;
			private set;
		}

		public int NoOfRetries {
			get;
			set;
		}

		public bool IsReady ()
		{
			if (IsWaiting)
			{
				bool needBlocking = NonBlockingWait ();

				IsWaiting = needBlocking && BlockingWait ();
			}

			return !(IsWaiting);
		}



		public long Duration {
			get;
			set;
		}

		public int Factor {
			get;
			set;
		}

		public uint TotalBlockingWaits {
			get;
			private set;
		}

		#endregion
	}
}

