using System.Collections.Generic;

namespace BirdNest.Rendering.UnitTests
{
	public class NestedLoop<TLeftData, TRightData>
	{
		private readonly IList<TLeftData> mOuter;
		private readonly IList<TRightData> mInner;

		public NestedLoop (IList<TLeftData> left, IList<TRightData> right)
		{
			mOuter = left;
			mInner = right;
			OuterIndex = 0;
			InnerIndex = 0;
		}

		public int OuterIndex {
			get;
			private set;
		}

		public int InnerIndex {
			get;
			private set;
		}

		public TLeftData OuterItem
		{
			get { 
				return mOuter[OuterIndex];
			}
		}

		public TRightData InnerItem
		{
			get { 
				return mInner[InnerIndex];
			}
		}

		public void ResetLoop ()
		{
			InnerIndex = 0;
			OuterIndex = 0;
		}

		public bool WorkToDo ()
		{
			return (OuterIndex < mOuter.Count);
		}

		public void Next ()
		{
			++InnerIndex;
			if (InnerIndex >= mInner.Count)
			{
				++OuterIndex;
				InnerIndex = 0;
			}
		}
	}
}

