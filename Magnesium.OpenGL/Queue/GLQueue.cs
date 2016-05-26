using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace Magnesium.OpenGL
{
	public class GLQueue : IMgQueue
	{
		private IGLQueueRenderer mRenderer;
		public GLQueue (IGLQueueRenderer renderer)
		{
			mRenderer = renderer;
		}

		#region IMgQueue implementation
		private Dictionary<uint, GLQueueSubmission> mRequests = new Dictionary<uint, GLQueueSubmission>();
		private Dictionary<uint, GLQueueSubmitOrder> mOrders = new Dictionary<uint, GLQueueSubmitOrder>();

		public Result QueueSubmit (MgSubmitInfo[] pSubmits, MgFence fence)
		{
			if (pSubmits == null)
				return Result.SUCCESS;

			var submissions = new List<GLQueueSubmission> ();

			uint key = (uint)mRequests.Keys.Count;
			foreach (var sub in pSubmits)
			{
				submissions.Add (new GLQueueSubmission (key, sub));
				++key;
			}

			foreach (var sub in submissions)
			{
				mRequests.Add (sub.Key, sub);
			}

			if (fence != null)
			{
				var order = new GLQueueSubmitOrder ();
				order.Key = (uint)mOrders.Keys.Count;
				order.Submissions = new Dictionary<uint, ISyncObject> ();
				order.Fence = fence as GLQueueFence;
				foreach (var sub in submissions)
				{
					order.Submissions.Add (key, sub.OrderFence);
					mRequests.Add (sub.Key, sub);
				}
			}

			return Result.SUCCESS;
		}

		void PerformRequests (uint key)
		{
			GLQueueSubmission request;
			if (mRequests.TryGetValue (key, out request))
			{
				int requirements = request.Waits.Count;
				int checks = 0;
				foreach (var signal in request.Waits)
				{
					if (signal.IsReady ())
					{
						++checks;
					}
				}
				// render
				if (checks >= requirements)
				{
					mRenderer.Render (null);
					foreach (var signal in request.Signals)
					{
						signal.Reset ();
						signal.BeginSync ();
					}
					if (request.OrderFence != null)
					{
						request.OrderFence.Reset ();
						request.OrderFence.BeginSync ();
					}
					mRequests.Remove (key);
				}
			}
		}

		public Result QueueWaitIdle ()
		{
			do
			{
				var requestKeys = mRequests.Keys;
				foreach(var key in requestKeys)
				{
					PerformRequests (key);
				}

				var orderKeys = mOrders.Keys;
				foreach (var orderKey in orderKeys)
				{
					GLQueueSubmitOrder order;
					if (mOrders.TryGetValue(orderKey, out order))
					{
						var submissionKeys = order.Submissions.Keys;
						foreach (uint key in submissionKeys)
						{
							ISyncObject signal;
							if (order.Submissions.TryGetValue (key, out signal))
							{
								if (signal.IsReady ())
								{
									signal.Reset ();
									order.Submissions.Remove (key);
								}
							}
						}

						if (order.Submissions.Count <= 0)
						{
							order.Fence.Signal ();
							mOrders.Remove (orderKey);
						}
					}
				}

			} while (mRequests.Keys.Count > 0 || mOrders.Keys.Count > 0);

			return Result.SUCCESS;
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

