using NUnit.Framework;

namespace Magnesium.OpenGL.UnitTests
{
	[TestFixture]
	public class TransformingViewportsUnitTests
	{
		[TestCase]
		public void InsertEmptyViewport()
		{
			IGLCmdBufferRepository repo = new GLCmdBufferRepository ();
			Assert.AreEqual (0, repo.Viewports.Count);

			ICmdVBOEntrypoint vbo = new MockVertexBufferFactory ();
			var transform = new CmdBufferInstructionSetTransformer (vbo, repo);

			

			Assert.IsNotNull (transform.Viewports);
			Assert.AreEqual (1, transform.Viewports.Count);

			var expected = new GLCmdViewportParameter (0, new MgViewport [] { });
			var actual = transform.Viewports.Items [0];

			Assert.IsTrue (expected.Equals (actual));

		}

		[TestCase]
		public void NoneFound ()
		{
			IGLCmdBufferRepository repo = new GLCmdBufferRepository ();
			Assert.AreEqual (0, repo.Viewports.Count);

			ICmdVBOEntrypoint vbo = new MockVertexBufferFactory ();
			var transform = new CmdBufferInstructionSetTransformer (vbo, repo);
			

			var command = new GLCmdDrawCommand{ Viewports = null, Draw = new GLCmdInternalDraw{ }  };

			var actual = transform.InitializeDrawItem (repo, null, command);
			Assert.IsFalse (actual);
			Assert.IsNotNull (transform.Viewports);
			Assert.AreEqual (1, transform.Viewports.Count);

			var expected = new GLCmdViewportParameter (0, new MgViewport [] { });
			var first = transform.Viewports.Items [0];

			Assert.IsTrue (expected.Equals (first));
		}

		[TestCase]
		public void NoPipeline ()
		{
			IGLCmdBufferRepository repo = new GLCmdBufferRepository ();

			repo.Viewports.Add(
				new GLCmdViewportParameter(
					0, 
					new MgViewport[]
					{
						new MgViewport{
							X = 0f,
							Y = 0f,
							Width = 100f,
							Height = 200f,
						},
						new MgViewport{
							X = 100f,
							Y = 200f,
							Width = 100f,
							Height = 200f,
						},
					})
			);

			Assert.AreEqual (1, repo.Viewports.Count);

			ICmdVBOEntrypoint vbo = new MockVertexBufferFactory ();
			var transform = new CmdBufferInstructionSetTransformer (vbo, repo);
			

			var command = new GLCmdDrawCommand{ Pipeline = null, Viewports = 0, Draw = new GLCmdInternalDraw{ }  };

			var actual = transform.InitializeDrawItem (repo, null, command);
			Assert.IsFalse (actual);
			Assert.IsNotNull (transform.Viewports);
			Assert.AreEqual (0, transform.Viewports.Count);
		}

		[TestCase]
		public void NoOverrideAllowed ()
		{
			IGLCmdBufferRepository repo = new GLCmdBufferRepository ();

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

				repo.Viewports.Add (OVERRIDE_VIEWPORT);
			}

			Assert.AreEqual (1, repo.Viewports.Count);

			var bindings = new GLVertexBufferBinding[]{ };
			var attributes = new GLVertexInputAttribute[]{ };

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
               	0, 
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

			var origin = new MockIGLRenderPass ();
			var pass = new GLCmdRenderPassCommand{ Origin = origin};

			repo.GraphicsPipelines.Add (new MockGLGraphicsPipeline 
				{
					VertexInput = new GLVertexBufferBinder(bindings, attributes),
					DynamicsStates = 0,
					Viewports = DEFAULT_VIEWPORT,
					Scissors = new GLCmdScissorParameter(0, new MgRect2D[]{}),
					ColorBlendEnums = new GLGraphicsPipelineBlendColorState{ Attachments = new GLGraphicsPipelineBlendColorAttachmentState[]{} },
				}
			);

			Assert.AreEqual (1, repo.GraphicsPipelines.Count);

			ICmdVBOEntrypoint vbo = new MockVertexBufferFactory ();
			var transform = new CmdBufferInstructionSetTransformer (vbo, repo);
			

			Assert.AreEqual (0, transform.Viewports.Count);

			var command = new GLCmdDrawCommand{ Pipeline = 0, Viewports = 0, Draw = new GLCmdInternalDraw{ }  };

			var result = transform.InitializeDrawItem (repo, pass, command);
			Assert.IsTrue (result);
			Assert.IsNotNull (transform.Viewports);
			Assert.AreEqual (1, transform.Viewports.Count);

			var actual = transform.Viewports.Items [0];

			Assert.IsTrue (DEFAULT_VIEWPORT.Equals(actual));
		}
		/**
		[TestCase]
		public void OverrideAllowed ()
		{
			IGLCmdBufferRepository repo = new GLCmdBufferRepository ();

			const float OVERRIDE_CLAMP = 5f;
			const float OVERRIDE_CONSTANT_FACTOR = 7f;
			const float OVERRIDE_SLOPE = 13f;

			repo.Viewports.Add(
				new GLCmdViewportParameter(
					0, 
					new MgViewport[]
					{
						new MgViewport{
							X = 0f,
							Y = 0f,
							Width = 100f,
							Height = 200f,
						},
						new MgViewport{
							X = 100f,
							Y = 200f,
							Width = 100f,
							Height = 200f,
						},
					})
			);

			Assert.AreEqual (1, repo.Viewports.Count);

			var bindings = new GLVertexBufferBinding[]{ };
			var attributes = new GLVertexInputAttribute[]{ };

			const float DEFAULT_CLAMP = 100f;
			const float DEFAULT_CONSTANT_FACTOR = 200f;
			const float DEFAULT_SLOPE = 300f;

			repo.GraphicsPipelines.Add (new MockGLGraphicsPipeline 
				{
					VertexInput = new GLVertexBufferBinder(bindings, attributes),
					DynamicsStates = GLGraphicsPipelineDynamicStateFlagBits.DEPTH_BIAS,
					ViewportsClamp = DEFAULT_CLAMP,
					ViewportsConstantFactor = DEFAULT_CONSTANT_FACTOR,
					ViewportsSlopeFactor = DEFAULT_SLOPE,
				}
			);

			Assert.AreEqual (1, repo.GraphicsPipelines.Count);

			ICmdVBOEntrypoint vbo = new MockVertexBufferFactory ();
			var transform = new Transformer (vbo, repo);
			

			// USE OVERRIDE 
			var command_0 = new GLCmdDrawCommand{ Pipeline = 0, Viewports = 0 };

			var result = transform.InitializeDrawItem (repo, null, command_0);
			Assert.IsTrue (result);
			Assert.IsNotNull (transform.Viewports);
			Assert.AreEqual (1, transform.Viewports.Count);

			var actualValues_0 = transform.Viewports [0];
			Assert.AreEqual (OVERRIDE_CLAMP, actualValues_0.ViewportsClamp);
			Assert.AreEqual (OVERRIDE_CONSTANT_FACTOR, actualValues_0.ViewportsConstantFactor);
			Assert.AreEqual (OVERRIDE_SLOPE, actualValues_0.ViewportsSlopeFactor);

			Assert.IsNotNull (transform.DrawItems);
			Assert.AreEqual (1, transform.DrawItems.Count);
			var drawItem_0 = transform.DrawItems [0];
			Assert.AreEqual (0, drawItem_0.Viewports);

			// NEXT TEST - IF VALUES DIFFER, CREATE NEW DEPTHBIAS
			var command_1 = new GLCmdDrawCommand{ Pipeline = 0, Viewports = null };

			result = transform.InitializeDrawItem (repo, null, command_1);
			Assert.IsTrue (result);
			Assert.AreEqual (2, transform.Viewports.Count);

			var actualValues_1 = transform.Viewports [1];
			Assert.AreEqual (DEFAULT_CLAMP, actualValues_1.ViewportsClamp);
			Assert.AreEqual (DEFAULT_CONSTANT_FACTOR, actualValues_1.ViewportsConstantFactor);
			Assert.AreEqual (DEFAULT_SLOPE, actualValues_1.ViewportsSlopeFactor);

			Assert.AreEqual (2, transform.DrawItems.Count);

			var drawItem_1 = transform.DrawItems [1];
			Assert.AreEqual (1, drawItem_1.Viewports);

			// NEXT TEST - IF DEPTHBIAS IS SAME, REUSE INDEX 1
			var command_2 = new GLCmdDrawCommand{ Pipeline = 0, Viewports = null };

			result = transform.InitializeDrawItem (repo, null, command_2);
			Assert.IsTrue (result);
			Assert.AreEqual (2, transform.Viewports.Count);

			Assert.AreEqual (3, transform.DrawItems.Count);

			var drawItem_2 = transform.DrawItems [2];
			var index = drawItem_2.Viewports;
			Assert.AreEqual (1, index);

			var actualValues_2 = transform.Viewports [index];
			Assert.AreEqual (DEFAULT_CLAMP, actualValues_2.ViewportsClamp);
			Assert.AreEqual (DEFAULT_CONSTANT_FACTOR, actualValues_2.ViewportsConstantFactor);
			Assert.AreEqual (DEFAULT_SLOPE, actualValues_2.ViewportsSlopeFactor);
		}

		**/
	}
}

