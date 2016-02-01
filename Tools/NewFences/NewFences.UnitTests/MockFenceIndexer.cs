using System;

namespace NewFences.UnitTests
{
	public class MockFenceIndexer : IFenceIndexVariable
	{
		#region IFenceIndexer implementation
		public MockFenceIndexer ()
		{
			NoOfIncrementCalls = 0;
			NoOfGetCalls = 0;
		}

		public int NoOfIncrementCalls { get; set; }
		public void Increment ()
		{
			++NoOfIncrementCalls;
		}

		public int NoOfGetCalls { get; set; }

		private int mIndex;
		public int Index {
			get {
				++NoOfGetCalls;
				return mIndex;
			}
			set {
				mIndex = value;
			}
		}

		#endregion
	}
}

