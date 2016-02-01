using NUnit.Framework;

namespace NewFences.UnitTests
{
	[TestFixture ()]	
	public class FrameRendererTest
	{
		[Test ()]
		public void NoFrames()
		{
			var indexer = new MockFenceIndexer ();
			var r = new FrameRenderer (indexer);
			r.Frames = null;
			r.Render ();

			Assert.AreEqual (1, indexer.NoOfGetCalls);
			Assert.AreEqual (1, indexer.NoOfIncrementCalls);
		}

		[Test ()]
		public void NoPassesForFrame()
		{
			var indexer = new MockFenceIndexer ();			
			var r = new FrameRenderer (indexer);
			var f = new RenderFrame ();
			f.Passes = null;
			r.Frames = new []{ f };
			r.Render ();

			Assert.AreEqual (1, indexer.NoOfGetCalls);
			Assert.AreEqual (1, indexer.NoOfIncrementCalls);
		}

		[Test ()]
		public void NoFenceForFrame()
		{
			var indexer = new MockFenceIndexer ();				
			var r = new FrameRenderer (indexer);
			var f = new RenderFrame ();
			f.Fence = null;
			r.Frames = new []{ f };
			r.Render ();

			Assert.AreEqual (1, indexer.NoOfGetCalls);
			Assert.AreEqual (1, indexer.NoOfIncrementCalls);
		}

		[Test ()]
		public void RenderThreeTimes()
		{
			const int NO_OF_TIMES = 3;
			var indexer = new MockFenceIndexer ();				
			var r = new FrameRenderer (indexer);
			var f = new RenderFrame ();
			f.Fence = null;
			r.Frames = new []{ f };
			for (int i = 0; i < NO_OF_TIMES; ++i)
			{
				r.Render ();
			}

			Assert.AreEqual (NO_OF_TIMES, indexer.NoOfGetCalls);
			Assert.AreEqual (NO_OF_TIMES, indexer.NoOfIncrementCalls);
		}
	}
}

