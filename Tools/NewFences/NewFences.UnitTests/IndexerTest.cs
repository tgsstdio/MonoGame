using System;
using NUnit.Framework;

namespace NewFences.UnitTests
{
	[TestFixture]
	public class IndexerTest
	{
		[Test ()]
		public void DefaultIndex()
		{
			const int MAX = 10;
			var r = new DefaultFenceIndexVariable (MAX);
			Assert.AreEqual (0, r.Index);
		}

		[Test ()]
		public void DefaultUpperLimit()
		{
			const int MAX = 13;
			var r = new DefaultFenceIndexVariable (MAX);
			Assert.AreEqual (MAX, r.IndexMaximum);
		}

		[Test ()]
		public void SingleStep()
		{
			const int MAX = 11;
			var r = new DefaultFenceIndexVariable (MAX);
			Assert.AreEqual (0, r.Index);
			r.Increment ();
			Assert.AreEqual (1, r.Index);
		}

		[Test ()]
		public void Resetting()
		{
			const int MAX = 17;
			var r = new DefaultFenceIndexVariable (MAX);
			Assert.AreEqual (0, r.Index);
			for (int i = 0; i < MAX - 1; ++i)
			{
				r.Increment ();
			}
			Assert.AreEqual (MAX - 1, r.Index);
			r.Increment ();
			Assert.AreEqual (0, r.Index);
		}
	}
}

