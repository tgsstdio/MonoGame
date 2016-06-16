using System;
using System.Collections.Generic;

namespace Magnesium.OpenGL
{
	public class GLQueue : IGLQueue
	{
		private IGLQueueRenderer mRenderer;
		private IGLSemaphoreGenerator mSignalModule;

		~GLQueue()
		{
			Dispose (false);
		}

		public void Dispose ()
		{
			Dispose (true);
			GC.SuppressFinalize (this);
		}

		private bool mIsDisposed = false;
		protected virtual void Dispose(bool dispose)
		{
			if (mIsDisposed)
				return;			

			foreach (var order in mOrders.Values)
			{
				foreach (var submission in order.Submissions.Values)
				{
					// RESET ALL FENCES CURRENTLY ATTACHED
					submission.Reset ();
				}
			}

			mIsDisposed = true;
		}

		public GLQueue (IGLQueueRenderer renderer, IGLSemaphoreGenerator generator)
		{
			mRenderer = renderer;
			mSignalModule = generator;
		}

		#region IMgQueue implementation
		private Dictionary<uint, GLQueueSubmission> mSubmissions = new Dictionary<uint, GLQueueSubmission>();
		private Dictionary<uint, GLQueueSubmitOrder> mOrders = new Dictionary<uint, GLQueueSubmitOrder>();

		Result CompleteAllPreviousSubmissions (IMgFence fence)
		{
			var internalFence = fence as IGLQueueFence;
			if (internalFence != null)
			{
				var result = QueueWaitIdle ();
				internalFence.Signal ();
				return result;
			}
			else
			{
				return Result.SUCCESS;
			}
		}

		public Result QueueSubmit (MgSubmitInfo[] pSubmits, IMgFence fence)
		{
			if (pSubmits == null)
			{				
				return CompleteAllPreviousSubmissions (fence);
			} 
			else
			{
				var submissions = new List<GLQueueSubmission> ();

				uint key = (uint)mSubmissions.Keys.Count;
				foreach (var sub in pSubmits)
				{
					var submit = new GLQueueSubmission (key, sub);
					submit.OrderFence = mSignalModule.Generate ();
					submissions.Add (submit);
					++key;
				}

				if (fence != null)
				{
					var order = new GLQueueSubmitOrder ();
					order.Key = (uint)mOrders.Keys.Count;
					order.Submissions = new Dictionary<uint, ISyncObject> ();
					order.Fence = fence as IGLQueueFence;
					foreach (var sub in submissions)
					{
						order.Submissions.Add (key, sub.OrderFence);
					}
					mOrders.Add (order.Key, order);
				}

				return Result.SUCCESS;
			}
		}

		void PerformRequests (uint key)
		{
			GLQueueSubmission request;
			if (mSubmissions.TryGetValue (key, out request))
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
					foreach (var buffer in request.CommandBuffers)
					{
						// TRY TO FIGURE OUT HOW TO STOP CMDBUF EXECUTION WITHOUT CHANGING 
						if (buffer.IsQueueReady)
						{
							mRenderer.Render (null);
						}
					}
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
					mSubmissions.Remove (key);
				}
			}
		}

		public Result QueueWaitIdle ()
		{
			do
			{
				var requestKeys = new uint[mSubmissions.Keys.Count];
				mSubmissions.Keys.CopyTo(requestKeys, 0);

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
						var submissionKeys = new uint[order.Submissions.Keys.Count];
						order.Submissions.Keys.CopyTo(submissionKeys, 0);

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

			} while (!IsEmpty());

			return Result.SUCCESS;
		}

		public bool IsEmpty ()
		{
			return (mSubmissions.Keys.Count == 0 && mOrders.Keys.Count == 0);
		}

		public Result QueueBindSparse (MgBindSparseInfo[] pBindInfo, IMgFence fence)
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

