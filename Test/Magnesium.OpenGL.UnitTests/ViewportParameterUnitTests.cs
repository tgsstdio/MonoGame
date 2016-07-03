using NUnit.Framework;

namespace Magnesium.OpenGL.UnitTests
{
	[TestFixture]
	public class ViewportParameterUnitTests
	{
		[TestCase]
		public void SelfTest ()
		{
			const int FIRST_INDEX = 0;

			const float CONSTANT_X = 3f;
			const float CONSTANT_Y = 5f;
			const float CONSTANT_WIDTH = 7f;
			const float CONSTANT_HEIGHT = 11f;
			const float CONSTANT_MINDEPTH = 13f;
			const float CONSTANT_MAXDEPTH = 17f;
			var vp = new GLCmdViewportParameter (FIRST_INDEX, 
				         new [] {
					new MgViewport {
						X = CONSTANT_X,
						Y = CONSTANT_Y,
						Width = CONSTANT_WIDTH,
						Height = CONSTANT_HEIGHT,
						MinDepth = CONSTANT_MINDEPTH,
						MaxDepth = CONSTANT_MAXDEPTH,
					},
				});

			Assert.IsTrue (vp.Equals (vp));
		}

		[TestCase]
		public void IsAllDifferent ()
		{
			const int FIRST_INDEX = 0;

			const float OVERRIDE_X_0 = 3f;
			const float OVERRIDE_Y_0 = 5f;
			const float OVERRIDE_WIDTH_0 = 7f;
			const float OVERRIDE_HEIGHT_0 = 11f;
			const float OVERRIDE_MINDEPTH_0 = 13f;
			const float OVERRIDE_MAXDEPTH_0 = 17f;

			const float OVERRIDE_X_1 = 101f;
			const float OVERRIDE_Y_1 = 103f;
			const float OVERRIDE_WIDTH_1 = 105f;
			const float OVERRIDE_HEIGHT_1 = 107f;
			const float OVERRIDE_MINDEPTH_1 = 109f;
			const float OVERRIDE_MAXDEPTH_1 = 111f;

			var OVERRIDE_VIEWPORT = new GLCmdViewportParameter (
				FIRST_INDEX, 
				new [] {
					new MgViewport {
						X = OVERRIDE_X_0,
						Y = OVERRIDE_Y_0,
						Width = OVERRIDE_WIDTH_0,
						Height = OVERRIDE_HEIGHT_0,
						MinDepth = OVERRIDE_MINDEPTH_0,
						MaxDepth = OVERRIDE_MAXDEPTH_0,
					},
					new MgViewport {
						X = OVERRIDE_X_1,
						Y = OVERRIDE_Y_1,
						Width = OVERRIDE_WIDTH_1,
						Height = OVERRIDE_HEIGHT_1,
						MinDepth = OVERRIDE_MINDEPTH_1,
						MaxDepth = OVERRIDE_MAXDEPTH_1,
					},
				});

			const float DEFAULT_X_0 = 203f;
			const float DEFAULT_Y_0 = 205f;
			const float DEFAULT_WIDTH_0 = 207f;
			const float DEFAULT_HEIGHT_0 = 211f;
			const float DEFAULT_MINDEPTH_0 = 213f;
			const float DEFAULT_MAXDEPTH_0 = 217f;

			const float DEFAULT_X_1 = 501f;
			const float DEFAULT_Y_1 = 503f;
			const float DEFAULT_WIDTH_1 = 505f;
			const float DEFAULT_HEIGHT_1 = 507f;
			const float DEFAULT_MINDEPTH_1 = 509f;
			const float DEFAULT_MAXDEPTH_1 = 511f;

			var DEFAULT_VIEWPORT = new GLCmdViewportParameter (
                   FIRST_INDEX, 
                   new [] {
					new MgViewport {
						X = DEFAULT_X_0,
						Y = DEFAULT_Y_0,
						Width = DEFAULT_WIDTH_0,
						Height = DEFAULT_HEIGHT_0,
						MinDepth = DEFAULT_MINDEPTH_0,
						MaxDepth = DEFAULT_MAXDEPTH_0,
					},
					new MgViewport {
						X = DEFAULT_X_1,
						Y = DEFAULT_Y_1,
						Width = DEFAULT_WIDTH_1,
						Height = DEFAULT_HEIGHT_1,
						MinDepth = DEFAULT_MINDEPTH_1,
						MaxDepth = DEFAULT_MAXDEPTH_1,
					},
				});

			Assert.IsFalse (OVERRIDE_VIEWPORT.Equals (DEFAULT_VIEWPORT));
		}

		[TestCase]
		public void DepthIsDifferent ()
		{
			const float OVERRIDE_X_0 = 3f;
			const float OVERRIDE_Y_0 = 5f;
			const float OVERRIDE_WIDTH_0 = 7f;
			const float OVERRIDE_HEIGHT_0 = 11f;
			const float OVERRIDE_MINDEPTH_0 = 13f;
			const float OVERRIDE_MAXDEPTH_0 = 17f;

			const float OVERRIDE_X_1 = 101f;
			const float OVERRIDE_Y_1 = 103f;
			const float OVERRIDE_WIDTH_1 = 105f;
			const float OVERRIDE_HEIGHT_1 = 107f;
			const float OVERRIDE_MINDEPTH_1 = 109f;
			const float OVERRIDE_MAXDEPTH_1 = 111f;

			var OVERRIDE_VIEWPORT = new GLCmdViewportParameter (
				0, 
				new [] {
					new MgViewport {
						X = OVERRIDE_X_0,
						Y = OVERRIDE_Y_0,
						Width = OVERRIDE_WIDTH_0,
						Height = OVERRIDE_HEIGHT_0,
						MinDepth = OVERRIDE_MINDEPTH_0,
						MaxDepth = OVERRIDE_MAXDEPTH_0,
					},
					new MgViewport {
						X = OVERRIDE_X_1,
						Y = OVERRIDE_Y_1,
						Width = OVERRIDE_WIDTH_1,
						Height = OVERRIDE_HEIGHT_1,
						MinDepth = OVERRIDE_MINDEPTH_1,
						MaxDepth = OVERRIDE_MAXDEPTH_1,
					},
				});

			const float DEFAULT_MAXDEPTH_1 = 511f;

			var DEFAULT_VIEWPORT = new GLCmdViewportParameter (
				0, 
				new [] {
					new MgViewport {
						X = OVERRIDE_X_0,
						Y = OVERRIDE_Y_0,
						Width = OVERRIDE_WIDTH_0,
						Height = OVERRIDE_HEIGHT_0,
						MinDepth = OVERRIDE_MINDEPTH_0,
						MaxDepth = OVERRIDE_MAXDEPTH_0,
					},
					new MgViewport {
						X = OVERRIDE_X_1,
						Y = OVERRIDE_Y_1,
						Width = OVERRIDE_WIDTH_1,
						Height = OVERRIDE_HEIGHT_1,
						MinDepth = OVERRIDE_MINDEPTH_1,
						MaxDepth = DEFAULT_MAXDEPTH_1,
					},
				});

			Assert.IsFalse (OVERRIDE_VIEWPORT.Equals (DEFAULT_VIEWPORT));
		}

		[TestCase]
		public void ScreenIsDifferent ()
		{
			const int FIRST_INDEX = 0;

			const float OVERRIDE_X_0 = 3f;
			const float OVERRIDE_Y_0 = 5f;
			const float OVERRIDE_WIDTH_0 = 7f;
			const float OVERRIDE_HEIGHT_0 = 11f;
			const float OVERRIDE_MINDEPTH_0 = 13f;
			const float OVERRIDE_MAXDEPTH_0 = 17f;

			const float OVERRIDE_X_1 = 101f;
			const float OVERRIDE_Y_1 = 103f;
			const float OVERRIDE_WIDTH_1 = 105f;
			const float OVERRIDE_HEIGHT_1 = 107f;
			const float OVERRIDE_MINDEPTH_1 = 109f;
			const float OVERRIDE_MAXDEPTH_1 = 111f;

			var OVERRIDE_VIEWPORT = new GLCmdViewportParameter (
				FIRST_INDEX, 
				new [] {
					new MgViewport {
						X = OVERRIDE_X_0,
						Y = OVERRIDE_Y_0,
						Width = OVERRIDE_WIDTH_0,
						Height = OVERRIDE_HEIGHT_0,
						MinDepth = OVERRIDE_MINDEPTH_0,
						MaxDepth = OVERRIDE_MAXDEPTH_0,
					},
					new MgViewport {
						X = OVERRIDE_X_1,
						Y = OVERRIDE_Y_1,
						Width = OVERRIDE_WIDTH_1,
						Height = OVERRIDE_HEIGHT_1,
						MinDepth = OVERRIDE_MINDEPTH_1,
						MaxDepth = OVERRIDE_MAXDEPTH_1,
					},
				});
			
			const float DEFAULT_X_1 = 501f;
			const float DEFAULT_Y_1 = 503f;
			const float DEFAULT_WIDTH_1 = 505f;
			const float DEFAULT_HEIGHT_1 = 507f;

			var DEFAULT_VIEWPORT = new GLCmdViewportParameter (
				FIRST_INDEX, 
				new [] {
					new MgViewport {
						X = OVERRIDE_X_0,
						Y = OVERRIDE_Y_0,
						Width = OVERRIDE_WIDTH_0,
						Height = OVERRIDE_HEIGHT_0,
						MinDepth = OVERRIDE_MINDEPTH_0,
						MaxDepth = OVERRIDE_MAXDEPTH_0,
					},
					new MgViewport {
						X = DEFAULT_X_1,
						Y = DEFAULT_Y_1,
						Width = DEFAULT_WIDTH_1,
						Height = DEFAULT_HEIGHT_1,
						MinDepth = OVERRIDE_MINDEPTH_0,
						MaxDepth = OVERRIDE_MAXDEPTH_0,
					},

				});

			Assert.IsFalse (OVERRIDE_VIEWPORT.Equals (DEFAULT_VIEWPORT));
		}

		[TestCase]
		public void NoOfViewportsIsDifferent ()
		{
			const int FIRST_INDEX = 0;

			const float OVERRIDE_X_0 = 3f;
			const float OVERRIDE_Y_0 = 5f;
			const float OVERRIDE_WIDTH_0 = 7f;
			const float OVERRIDE_HEIGHT_0 = 11f;
			const float OVERRIDE_MINDEPTH_0 = 13f;
			const float OVERRIDE_MAXDEPTH_0 = 17f;

			const float OVERRIDE_X_1 = 101f;
			const float OVERRIDE_Y_1 = 103f;
			const float OVERRIDE_WIDTH_1 = 105f;
			const float OVERRIDE_HEIGHT_1 = 107f;
			const float OVERRIDE_MINDEPTH_1 = 109f;
			const float OVERRIDE_MAXDEPTH_1 = 111f;

			var OVERRIDE_VIEWPORT = new GLCmdViewportParameter (
				FIRST_INDEX, 
				new [] {
					new MgViewport {
						X = OVERRIDE_X_0,
						Y = OVERRIDE_Y_0,
						Width = OVERRIDE_WIDTH_0,
						Height = OVERRIDE_HEIGHT_0,
						MinDepth = OVERRIDE_MINDEPTH_0,
						MaxDepth = OVERRIDE_MAXDEPTH_0,
					},
					new MgViewport {
						X = OVERRIDE_X_1,
						Y = OVERRIDE_Y_1,
						Width = OVERRIDE_WIDTH_1,
						Height = OVERRIDE_HEIGHT_1,
						MinDepth = OVERRIDE_MINDEPTH_1,
						MaxDepth = OVERRIDE_MAXDEPTH_1,
					},
				});

			const float DEFAULT_X_0 = 203f;
			const float DEFAULT_Y_0 = 205f;
			const float DEFAULT_WIDTH_0 = 207f;
			const float DEFAULT_HEIGHT_0 = 211f;
			const float DEFAULT_MINDEPTH_0 = 213f;
			const float DEFAULT_MAXDEPTH_0 = 217f;

			const float DEFAULT_X_1 = 501f;
			const float DEFAULT_Y_1 = 503f;
			const float DEFAULT_WIDTH_1 = 505f;
			const float DEFAULT_HEIGHT_1 = 507f;
			const float DEFAULT_MINDEPTH_1 = 509f;
			const float DEFAULT_MAXDEPTH_1 = 511f;

			var DEFAULT_VIEWPORT = new GLCmdViewportParameter (
				FIRST_INDEX, 
				new [] {
					new MgViewport {
						X = OVERRIDE_X_0,
						Y = OVERRIDE_Y_0,
						Width = OVERRIDE_WIDTH_0,
						Height = OVERRIDE_HEIGHT_0,
						MinDepth = OVERRIDE_MINDEPTH_0,
						MaxDepth = OVERRIDE_MAXDEPTH_0,
					},
					new MgViewport {
						X = OVERRIDE_X_1,
						Y = OVERRIDE_Y_1,
						Width = OVERRIDE_WIDTH_1,
						Height = OVERRIDE_HEIGHT_1,
						MinDepth = OVERRIDE_MINDEPTH_1,
						MaxDepth = OVERRIDE_MAXDEPTH_1,
					},					
					new MgViewport {
						X = DEFAULT_X_0,
						Y = DEFAULT_Y_0,
						Width = DEFAULT_WIDTH_0,
						Height = DEFAULT_HEIGHT_0,
						MinDepth = DEFAULT_MINDEPTH_0,
						MaxDepth = DEFAULT_MAXDEPTH_0,
					},
					new MgViewport {
						X = DEFAULT_X_1,
						Y = DEFAULT_Y_1,
						Width = DEFAULT_WIDTH_1,
						Height = DEFAULT_HEIGHT_1,
						MinDepth = DEFAULT_MINDEPTH_1,
						MaxDepth = DEFAULT_MAXDEPTH_1,
					},
				});

			Assert.IsFalse (OVERRIDE_VIEWPORT.Equals (DEFAULT_VIEWPORT));
		}

		[TestCase]
		public void UseDifferentFirstIndex ()
		{
			const int FIRST_INDEX = 0;

			const float OVERRIDE_X_0 = 3f;
			const float OVERRIDE_Y_0 = 5f;
			const float OVERRIDE_WIDTH_0 = 7f;
			const float OVERRIDE_HEIGHT_0 = 11f;
			const float OVERRIDE_MINDEPTH_0 = 13f;
			const float OVERRIDE_MAXDEPTH_0 = 17f;

			const float OVERRIDE_X_1 = 101f;
			const float OVERRIDE_Y_1 = 103f;
			const float OVERRIDE_WIDTH_1 = 105f;
			const float OVERRIDE_HEIGHT_1 = 107f;
			const float OVERRIDE_MINDEPTH_1 = 109f;
			const float OVERRIDE_MAXDEPTH_1 = 111f;

			var OVERRIDE_VIEWPORT = new GLCmdViewportParameter (
				FIRST_INDEX, 
				new [] {
					new MgViewport {
						X = OVERRIDE_X_0,
						Y = OVERRIDE_Y_0,
						Width = OVERRIDE_WIDTH_0,
						Height = OVERRIDE_HEIGHT_0,
						MinDepth = OVERRIDE_MINDEPTH_0,
						MaxDepth = OVERRIDE_MAXDEPTH_0,
					},
					new MgViewport {
						X = OVERRIDE_X_1,
						Y = OVERRIDE_Y_1,
						Width = OVERRIDE_WIDTH_1,
						Height = OVERRIDE_HEIGHT_1,
						MinDepth = OVERRIDE_MINDEPTH_1,
						MaxDepth = OVERRIDE_MAXDEPTH_1,
					},
				});

			const float DEFAULT_X_0 = 203f;
			const float DEFAULT_Y_0 = 205f;
			const float DEFAULT_WIDTH_0 = 207f;
			const float DEFAULT_HEIGHT_0 = 211f;
			const float DEFAULT_MINDEPTH_0 = 213f;
			const float DEFAULT_MAXDEPTH_0 = 217f;

			const float DEFAULT_X_1 = 501f;
			const float DEFAULT_Y_1 = 503f;
			const float DEFAULT_WIDTH_1 = 505f;
			const float DEFAULT_HEIGHT_1 = 507f;
			const float DEFAULT_MINDEPTH_1 = 509f;
			const float DEFAULT_MAXDEPTH_1 = 511f;

			const int NEXT_INDEX = 1;

			var DEFAULT_VIEWPORT = new GLCmdViewportParameter (
				NEXT_INDEX, 
				new [] {
					new MgViewport {
						X = DEFAULT_X_0,
						Y = DEFAULT_Y_0,
						Width = DEFAULT_WIDTH_0,
						Height = DEFAULT_HEIGHT_0,
						MinDepth = DEFAULT_MINDEPTH_0,
						MaxDepth = DEFAULT_MAXDEPTH_0,
					},
					new MgViewport {
						X = DEFAULT_X_1,
						Y = DEFAULT_Y_1,
						Width = DEFAULT_WIDTH_1,
						Height = DEFAULT_HEIGHT_1,
						MinDepth = DEFAULT_MINDEPTH_1,
						MaxDepth = DEFAULT_MAXDEPTH_1,
					},
				});

			Assert.IsFalse (OVERRIDE_VIEWPORT.Equals (DEFAULT_VIEWPORT));
		}
	}
}

