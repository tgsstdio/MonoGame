using NUnit.Framework;

namespace Magnesium.OpenGL.UnitTests
{
	[TestFixture ()]
	public class Queue
	{
		public class MockQueueRenderer : IGLQueueRenderer
		{
			#region IGLQueueRenderer implementation

			public void Render (CmdBufferInstructionSet[] items)
			{
				throw new System.NotImplementedException ();
			}
			public void SetDefault ()
			{
				throw new System.NotImplementedException ();
			}
			public void CheckProgram (GLQueueDrawItem nextState)
			{
				throw new System.NotImplementedException ();
			}
			public void Render (GLQueueDrawItem[] items)
			{
				
			}
			#endregion
		}

		public class MockSemaphoreGenerator : IGLSemaphoreGenerator
		{
			private readonly MockGLSemaphore mMainSemaphore;
			public uint NoOfFunctionCalls;
			public MockSemaphoreGenerator (MockGLSemaphore main)
			{
				mMainSemaphore = main;
				NoOfFunctionCalls = 0;
			}

			#region IGLSemaphoreGenerator implementation
			public ISyncObject Generate ()
			{
				++NoOfFunctionCalls;
				return mMainSemaphore;
			}
			#endregion
		}

		[Test ()]
		public void EmptyParameter ()
		{
			IGLQueueRenderer renderer = new MockQueueRenderer ();
			IGLSemaphoreGenerator generator = new MockSemaphoreGenerator (null);
			IGLCmdImageCapabilities imageOps = new MockGLCmdImageCapabilities ();

			IGLQueue queue = new GLQueue (renderer, generator, imageOps);
			var actual = queue.QueueSubmit (null, null);
			Assert.AreEqual (Result.SUCCESS, actual);
		}

		public class MockGLQueueFence : IGLQueueFence
		{
			public MockGLQueueFence ()
			{
				Count = 0;
			}

			public uint Count { get; set; }

			public void DestroyFence (IMgDevice device, MgAllocationCallbacks allocator)
			{
				throw new System.NotImplementedException ();
			}

			#region IGLQueueFence implementation
			public void Signal ()
			{
				++Count;
			}
			#endregion
		}

		[Test()]
		public void NullWithFence()
		{
			IGLQueueRenderer renderer = new MockQueueRenderer ();
			IGLSemaphoreGenerator generator = new MockSemaphoreGenerator (null);
			IGLCmdImageCapabilities imageOps = new MockGLCmdImageCapabilities ();

			IGLQueue queue = new GLQueue (renderer, generator, imageOps);
			var fence = new MockGLQueueFence ();

			var actual = queue.QueueSubmit (null, fence);
			Assert.AreEqual (Result.SUCCESS, actual);
			Assert.AreEqual (1, fence.Count);
		}

		public class MockGLSemaphore : IGLSemaphore
		{
			public MockGLSemaphore (bool initialState)
			{
				ReadyState = initialState;
			}

			#region ISyncObject implementation
			public void Reset ()
			{
				
			}
			public void BeginSync ()
			{
				
			}

			public bool ReadyState { get; set; }
			public bool IsReady ()
			{
				return ReadyState;
			}

			public bool IsWaiting {
				get;
				set;
			}
			public long Duration {
				get {
					throw new System.NotImplementedException ();
				}
				set {
					throw new System.NotImplementedException ();
				}
			}
			public int Factor {
				get {
					throw new System.NotImplementedException ();
				}
				set {
					throw new System.NotImplementedException ();
				}
			}
			public uint TotalBlockingWaits {
				get {
					throw new System.NotImplementedException ();
				}
			}
			public uint TotalFailures {
				get {
					throw new System.NotImplementedException ();
				}
			}
			public uint BlockingRetries {
				get {
					throw new System.NotImplementedException ();
				}
				set {
					throw new System.NotImplementedException ();
				}
			}
			public uint NonBlockingRetries {
				get {
					throw new System.NotImplementedException ();
				}
				set {
					throw new System.NotImplementedException ();
				}
			}
			#endregion
			#region IMgSemaphore implementation
			public void DestroySemaphore (IMgDevice device, MgAllocationCallbacks allocator)
			{
				throw new System.NotImplementedException ();
			}
			#endregion
		}

		[Test()]
		public void PreviousWorkCompleted()
		{
			IGLQueueRenderer renderer = new MockQueueRenderer ();
			IGLCmdImageCapabilities imageOps = new MockGLCmdImageCapabilities ();

			var internalSema = new MockGLSemaphore (true);

			var generator = new MockSemaphoreGenerator (internalSema);

			IGLQueue queue = new GLQueue (renderer, generator, imageOps);
			var before = new MockGLSemaphore (false);
			var submits = new [] { 
				new MgSubmitInfo
				{
					WaitSemaphores = new []
					{
						new MgSubmitInfoWaitSemaphoreInfo{ 
							WaitSemaphore = before
						},
					},
				}
			};

			Assert.AreEqual (0, generator.NoOfFunctionCalls);

			var actual = queue.QueueSubmit (submits, null);
			Assert.AreEqual (Result.SUCCESS, actual);
			Assert.False (queue.IsEmpty ());

			Assert.AreEqual (1, generator.NoOfFunctionCalls);

			before.ReadyState = true;

			// COMPLETE ALL PREVIOUS WORK
			var fence = new MockGLQueueFence ();
			actual = queue.QueueSubmit (null, fence);
			Assert.AreEqual (Result.SUCCESS, actual);
			Assert.AreEqual (1, fence.Count);
			Assert.IsTrue (queue.IsEmpty ());

			Assert.AreEqual (1, generator.NoOfFunctionCalls);
		}

		public class FakeSemaphoreGenerator : IGLSemaphoreGenerator
		{
			public MockGLSemaphore Semaphore {get; set;}
			public uint NoOfFunctionCalls;
			public FakeSemaphoreGenerator ()
			{
				NoOfFunctionCalls = 0;
			}

			#region IGLSemaphoreGenerator implementation
			public ISyncObject Generate ()
			{
				++NoOfFunctionCalls;
				return Semaphore;
			}
			#endregion
		}

		[Test()]
		public void SemaphoreIdsClash()
		{
			IGLQueueRenderer renderer = new MockQueueRenderer ();
			IGLCmdImageCapabilities imageOps = new MockGLCmdImageCapabilities ();

			var generator = new FakeSemaphoreGenerator ();

			IGLQueue queue = new GLQueue (renderer, generator, imageOps);
			Assert.AreEqual (0, generator.NoOfFunctionCalls);

			var first = new MockGLSemaphore (false);
			var semaphore_0 = new MockGLSemaphore (false);
			generator.Semaphore = semaphore_0;
			var actual = queue.QueueSubmit (new [] { 
				new MgSubmitInfo
				{
					WaitSemaphores = new []
					{
						new MgSubmitInfoWaitSemaphoreInfo{ 
							WaitSemaphore = first
						},
					},
				},
			}, null);
			Assert.AreEqual (Result.SUCCESS, actual);
			Assert.False (queue.IsEmpty ());

			Assert.AreEqual (1, generator.NoOfFunctionCalls);

			first.ReadyState = true;

			var second = new MockGLSemaphore (false);
			var semaphore_1 = new MockGLSemaphore (false);
			generator.Semaphore = semaphore_1;
			actual = queue.QueueSubmit (new [] { 
				new MgSubmitInfo
				{
					WaitSemaphores = new []
					{
						new MgSubmitInfoWaitSemaphoreInfo{ 
							WaitSemaphore = second
						},
					},
				},
			}, null);
			Assert.AreEqual (Result.SUCCESS, actual);
			Assert.False (queue.IsEmpty ());

			semaphore_0.ReadyState = true;
			queue.QueueSubmit (null, null);
			Assert.False (queue.IsEmpty ());

			var semaphore_2 = new MockGLSemaphore (true);
			generator.Semaphore = semaphore_2;
			actual = queue.QueueSubmit (new [] { 
				new MgSubmitInfo (),
			}, null);
			Assert.AreEqual (Result.SUCCESS, actual);
			Assert.False (queue.IsEmpty ());

			Assert.AreEqual (3, generator.NoOfFunctionCalls);
		}
	}
}

