using NUnit.Framework;

namespace NewFences.UnitTests
{
	[TestFixture ()]
	public class UpdateTest
	{
		[Test ()]
		public void UpdateTestNoBuffers ()
		{
			var indexer = new MockFenceIndexer ();
			IMeshBufferUpdater r = new MeshBufferUpdater (indexer);
			r.Buffers = null;
			r.Update ();
		}


	}
}

