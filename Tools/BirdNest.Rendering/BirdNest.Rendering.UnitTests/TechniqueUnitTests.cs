using System;
using NUnit.Framework;

namespace BirdNest.Rendering.UnitTests
{
	[TestFixture ()]
	public class TechniqueUnitTests
	{
		//[TestCase]
		public void ForEach()
		{
			IDataIterator iterator = new DataIterator<LightInfo> ();
			var tech = new Technique ();
			tech.Foreach (iterator);

			tech.BeginLoop ();
			while (!tech.IsDone ())
			{
				tech.BeginStep ();				
				tech.Step ();
				tech.EndStep ();
			}
			tech.EndLoop ();
		}

		[TestCase]
		public void CrossOperator()
		{
			const int LEFT = 5;
			const int RIGHT = 3;
			var left = new int[LEFT];
			var right = new int[RIGHT];

			var loopIter = new NestedLoop<int, int> (left, right);

			int count = 0;
			loopIter.ResetLoop ();
			while (loopIter.WorkToDo ())
			{
				// first
				if (count == 0)
				{
					Assert.AreEqual (0, loopIter.OuterIndex, "count = 0; i=0");
					Assert.AreEqual (0, loopIter.InnerIndex, "count = 0; j=0");
				}
				else if (count == 1)
				{
					Assert.AreEqual (0, loopIter.OuterIndex, "count = 1; i=0");
					Assert.AreEqual (1, loopIter.InnerIndex, "count = 1; j=1");
				}
				else if (count == 2)
				{
					Assert.AreEqual (0, loopIter.OuterIndex, "count = 2; i=0");
					Assert.AreEqual (2, loopIter.InnerIndex, "count = 2; j=2");
				}
				else if (count == 3)
				{
					Assert.AreEqual (1, loopIter.OuterIndex, "count = 3; i=1");
					Assert.AreEqual (0, loopIter.InnerIndex, "count = 3; j=0");
				}
				else if (count == 5)
				{
					Assert.AreEqual (1, loopIter.OuterIndex, "i=1");
					Assert.AreEqual (2, loopIter.InnerIndex, "j=2");
				}

				loopIter.Next ();
				++count;
			}
			Assert.AreEqual (LEFT * RIGHT, count);					
		}
	}
}

