using System;
using NUnit.Framework;

namespace Magnesium.OpenGL.UnitTests
{
	[TestFixture]
	public class ViewportMergeUnitTests
	{
		[TestCase]
		public void CalculateFinalLength0()
		{
			Assert.AreEqual (4, GLCmdArraySlice<float>.GetAdjustedLength (4, 1, 3));

			Assert.AreEqual (2, GLCmdArraySlice<float>.GetAdjustedLength (2, 1, 1));

			Assert.AreEqual (3, GLCmdArraySlice<float>.GetAdjustedLength (3, 1, 1));

			Assert.AreEqual (6, GLCmdArraySlice<float>.GetAdjustedLength (3, 1, 5));

			Assert.AreEqual (8, GLCmdArraySlice<float>.GetAdjustedLength (5, 0, 8));

			Assert.AreEqual (7, GLCmdArraySlice<float>.GetAdjustedLength (7, 0, 0));

			Assert.AreEqual (0, GLCmdArraySlice<float>.GetAdjustedLength (0, 0, 0));
		}

		[TestCase]
		public void Merge()
		{
			const int FACTOR = 4;
			var basis = new GLCmdArraySlice<float> (
				values: new float [] { 
					0, 1, 2, 3,
					4, 5, 6, 7,
					8, 9, 10, 11 },
				factor : FACTOR,
				first : 0,
				count : 3
			);

			var delta = new GLCmdArraySlice<float> (
				values : new float [] { 50, 51, 52, 54, },
				factor : FACTOR,
				first : 1,
				count : 1
			);

			var output = basis.Merge (delta);

			Assert.AreEqual (basis.Values[0], output.Values [0]);
			Assert.AreEqual (basis.Values[1], output.Values [1]);
			Assert.AreEqual (basis.Values[2], output.Values [2]);
			Assert.AreEqual (basis.Values[3], output.Values [3]);

			Assert.AreEqual (delta.Values[0], output.Values [4]);
			Assert.AreEqual (delta.Values[1], output.Values [5]);
			Assert.AreEqual (delta.Values[2], output.Values [6]);
			Assert.AreEqual (delta.Values[3], output.Values [7]);

			Assert.AreEqual (basis.Values[8],  output.Values [8]);
			Assert.AreEqual (basis.Values[9],  output.Values [9]);
			Assert.AreEqual (basis.Values[10], output.Values [10]);
			Assert.AreEqual (basis.Values[11], output.Values [11]);
		}

		[TestCase]
		public void Extend()
		{
			const int FACTOR = 4;
			var basis = new GLCmdArraySlice<double> (
				values : new double [] { 
					0, 1, 2, 3,
					4, 5, 6, 7,
				},
				factor : FACTOR,
				first : 0,
				count : 2
			);

			var delta = new GLCmdArraySlice<double> (
				values : new double [] { 
					50, 51, 52, 53, 
					60, 61, 62, 63,
				},
				factor : FACTOR,
				first : 2,
				count : 2
			);

			var output = GLCmdArraySlice<double>.MergeData (FACTOR, basis, delta);

			Assert.AreEqual (basis.Values[0], output.Values [0]);
			Assert.AreEqual (basis.Values[1], output.Values [1]);
			Assert.AreEqual (basis.Values[2], output.Values [2]);
			Assert.AreEqual (basis.Values[3], output.Values [3]);

			Assert.AreEqual (basis.Values[4], output.Values [4]);
			Assert.AreEqual (basis.Values[5], output.Values [5]);
			Assert.AreEqual (basis.Values[6], output.Values [6]);
			Assert.AreEqual (basis.Values[7], output.Values [7]);

			Assert.AreEqual (delta.Values[0],  output.Values [8]);
			Assert.AreEqual (delta.Values[1],  output.Values [9]);
			Assert.AreEqual (delta.Values[2],  output.Values [10]);
			Assert.AreEqual (delta.Values[3],  output.Values [11]);

			Assert.AreEqual (delta.Values[4],  output.Values [12]);
			Assert.AreEqual (delta.Values[5],  output.Values [13]);
			Assert.AreEqual (delta.Values[6],  output.Values [14]);
			Assert.AreEqual (delta.Values[7],  output.Values [15]);
		}

		[TestCase]
		public void CopyValuesTest()
		{
			Func<float[], uint, MgViewport, uint> copyFn = (dst, offset, src) => {
				dst[offset] = src.X;
				dst [1 + offset] = src.Y;
				dst [2 + offset] = src.Width;
				dst [3 + offset] = src.Height;
				return 4;
			};

			var viewports_0 = new [] { 
				new MgViewport {
					X = 100,
					Y = 200, 
					Width = 400,
					Height = 600,
				}
			};

			var dest0 = new float[4];
			GLCmdArraySlice<float>.CopyValues(dest0, 0, viewports_0, copyFn);

			Assert.AreEqual (viewports_0 [0].X, dest0 [0]);
			Assert.AreEqual (viewports_0 [0].Y, dest0 [1]);
			Assert.AreEqual (viewports_0 [0].Width, dest0 [2]);
			Assert.AreEqual (viewports_0 [0].Height, dest0 [3]);


			var viewports_1 = new [] { 
				new MgViewport {
					X = 100,
					Y = 200, 
					Width = 400,
					Height = 600,
				},
				new MgViewport {
					X = 300,
					Y = 444, 
					Width = 666,
					Height = 777,
				}
			};

			const float CONSTANT_VALUE_0 = 1000f;
			const float CONSTANT_VALUE_1 = 2000f;
			const float CONSTANT_VALUE_2 = 3000f;
			const float CONSTANT_VALUE_3 = 4000f;

			var dest1 = new float[12];
			dest1 [0] = CONSTANT_VALUE_0;
			dest1 [1] = CONSTANT_VALUE_1;
			dest1 [2] = CONSTANT_VALUE_2;
			dest1 [3] = CONSTANT_VALUE_3;

			GLCmdArraySlice<float>.CopyValues(dest1, 4, viewports_1, copyFn);

			Assert.AreEqual (CONSTANT_VALUE_0, dest1 [0]);
			Assert.AreEqual (CONSTANT_VALUE_1, dest1 [1]);
			Assert.AreEqual (CONSTANT_VALUE_2, dest1 [2]);
			Assert.AreEqual (CONSTANT_VALUE_3, dest1 [3]);

			Assert.AreEqual (viewports_1 [0].X, dest1 [4]);
			Assert.AreEqual (viewports_1 [0].Y, dest1 [5]);
			Assert.AreEqual (viewports_1 [0].Width, dest1 [6]);
			Assert.AreEqual (viewports_1 [0].Height, dest1 [7]);

			Assert.AreEqual (viewports_1 [1].X, dest1 [8]);
			Assert.AreEqual (viewports_1 [1].Y, dest1 [9]);
			Assert.AreEqual (viewports_1 [1].Width, dest1 [10]);
			Assert.AreEqual (viewports_1 [1].Height, dest1 [11]);
		}
	}
}

