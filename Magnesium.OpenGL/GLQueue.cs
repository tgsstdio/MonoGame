using System;

namespace Magnesium.OpenGL
{
	public class GLQueue : IMgQueue
	{
		public GLQueue ()
		{
		}

		#region IMgQueue implementation

		public Result QueueSubmit (MgSubmitInfo[] pSubmits, MgFence fence)
		{
			throw new NotImplementedException ();
		}

		public Result QueueWaitIdle ()
		{
			throw new NotImplementedException ();
		}

		public Result QueueBindSparse (MgBindSparseInfo[] pBindInfo, MgFence fence)
		{
			throw new NotImplementedException ();
		}

		public Result QueuePresentKHR (MgPresentInfoKHR pPresentInfo)
		{
			throw new NotImplementedException ();
		}

		#endregion
	}
}

