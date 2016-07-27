using NUnit.Framework;

namespace Magnesium.OpenGL.UnitTests
{
	[TestFixture]
	public class TransformingDepthBoundsUnitTests
	{
		[TestCase]
		public void InitialiseCheck()
		{
			IGLCmdBufferRepository repo = new GLCmdBufferRepository ();
			Assert.AreEqual (0, repo.DepthBounds.Count);

			ICmdVBOCapabilities vbo = new MockVertexBufferFactory ();
			var transform = new Transformer (vbo, repo);

			

			Assert.IsNotNull (transform.DepthBounds);
			Assert.AreEqual (0, transform.DepthBounds.Count);
		}

		[TestCase]
		public void NoneFound ()
		{
			IGLCmdBufferRepository repo = new GLCmdBufferRepository ();
			Assert.AreEqual (0, repo.DepthBounds.Count);

			ICmdVBOCapabilities vbo = new MockVertexBufferFactory ();
			var transform = new Transformer (vbo, repo);
			

			var command = new GLCmdDrawCommand{ DepthBounds = null };

			var actual = transform.InitialiseDrawItem (repo, null, command);
			Assert.IsFalse (actual);
			Assert.IsNotNull (transform.DepthBounds);
			Assert.AreEqual (0, transform.DepthBounds.Count);
		}

		[TestCase]
		public void NoPipeline ()
		{
			IGLCmdBufferRepository repo = new GLCmdBufferRepository ();

			repo.DepthBounds.Add(new GLCmdDepthBoundsParameter
				{
					MinDepthBounds = 5f,
					MaxDepthBounds = 7f,
				}
			);

			Assert.AreEqual (1, repo.DepthBounds.Count);

			ICmdVBOCapabilities vbo = new MockVertexBufferFactory ();
			var transform = new Transformer (vbo, repo);
			

			var command = new GLCmdDrawCommand{ Pipeline = null, DepthBounds = 0, Draw = new GLCmdInternalDraw{ }  };

			var actual = transform.InitialiseDrawItem (repo, null, command);
			Assert.IsFalse (actual);
			Assert.IsNotNull (transform.DepthBounds);
			Assert.AreEqual (0, transform.DepthBounds.Count);
		}

		[TestCase]
		public void NoOverrideAllowed ()
		{
			IGLCmdBufferRepository repo = new GLCmdBufferRepository ();

			const float OVERRIDE_MINDEPTH = 5f;
			const float OVERRIDE_MAXDEPTH = 13f;
			repo.DepthBounds.Add(new GLCmdDepthBoundsParameter
				{
					MinDepthBounds = OVERRIDE_MINDEPTH,
					MaxDepthBounds = OVERRIDE_MAXDEPTH,
				}
			);

			Assert.AreEqual (1, repo.DepthBounds.Count);

			var bindings = new GLVertexBufferBinding[]{ };
			var attributes = new GLVertexInputAttribute[]{ };

			const float DEFAULT_MINDEPTH = 100f;
			const float DEFAULT_MAXDEPTH = 300f;

			var origin = new MockIGLRenderPass ();
			var pass = new GLCmdRenderPassCommand{ Origin = origin};

			repo.GraphicsPipelines.Add (new MockGLGraphicsPipeline 
				{
					VertexInput = new GLVertexBufferBinder(bindings, attributes),
					DynamicsStates = 0,
					MinDepthBounds = DEFAULT_MINDEPTH,
					MaxDepthBounds = DEFAULT_MAXDEPTH,
					Viewports = new GLCmdViewportParameter(0, new MgViewport []{}),
					Scissors = new GLCmdScissorParameter(0, new MgRect2D[]{}),
					ColorBlendEnums = new GLQueueRendererColorBlendState{ Attachments = new GLQueueColorAttachmentBlendState[]{} },
				}
			);

			Assert.AreEqual (1, repo.GraphicsPipelines.Count);

			ICmdVBOCapabilities vbo = new MockVertexBufferFactory ();
			var transform = new Transformer (vbo, repo);
			

			var command = new GLCmdDrawCommand{ Pipeline = 0, DepthBounds = 0, Draw = new GLCmdInternalDraw{ }  };

			var result = transform.InitialiseDrawItem (repo, pass, command);
			Assert.IsTrue (result);
			Assert.IsNotNull (transform.DepthBounds);
			Assert.AreEqual (1, transform.DepthBounds.Count);

			var actual = transform.DepthBounds.Items [0];
			Assert.AreEqual (DEFAULT_MINDEPTH, actual.MinDepthBounds);
			Assert.AreEqual (DEFAULT_MAXDEPTH, actual.MaxDepthBounds);
		}

		[TestCase]
		public void OverrideAllowed ()
		{
			IGLCmdBufferRepository repo = new GLCmdBufferRepository ();

			const float OVERRIDE_MINDEPTH = 5f;
			const float OVERRIDE_MAXDEPTH = 13f;
			repo.DepthBounds.Add(new GLCmdDepthBoundsParameter
				{
					MinDepthBounds = OVERRIDE_MINDEPTH,
					MaxDepthBounds = OVERRIDE_MAXDEPTH,
				}
			);

			Assert.AreEqual (1, repo.DepthBounds.Count);

			var bindings = new GLVertexBufferBinding[]{ };
			var attributes = new GLVertexInputAttribute[]{ };

			const float DEFAULT_MINDEPTH = 100f;
			const float DEFAULT_MAXDEPTH = 300f;

			var origin = new MockIGLRenderPass ();
			var pass = new GLCmdRenderPassCommand{ Origin = origin};

			repo.GraphicsPipelines.Add (new MockGLGraphicsPipeline 
				{
					VertexInput = new GLVertexBufferBinder(bindings, attributes),
					DynamicsStates = GLGraphicsPipelineDynamicStateFlagBits.DEPTH_BOUNDS,
					MinDepthBounds = DEFAULT_MINDEPTH,
					MaxDepthBounds = DEFAULT_MAXDEPTH,
					Viewports = new GLCmdViewportParameter(0, new MgViewport[]{}),
					Scissors = new GLCmdScissorParameter(0, new MgRect2D[]{}),
					ColorBlendEnums = new GLQueueRendererColorBlendState{ Attachments = new GLQueueColorAttachmentBlendState[]{} },
				}
			);

			Assert.AreEqual (1, repo.GraphicsPipelines.Count);

			ICmdVBOCapabilities vbo = new MockVertexBufferFactory ();
			var transform = new Transformer (vbo, repo);
			

			// USE OVERRIDE 
			var command_0 = new GLCmdDrawCommand{ Pipeline = 0, DepthBounds = 0, Draw = new GLCmdInternalDraw{ } };

			var result = transform.InitialiseDrawItem (repo, pass, command_0);
			Assert.IsTrue (result);
			Assert.IsNotNull (transform.DepthBounds);
			Assert.AreEqual (1, transform.DepthBounds.Count);

			var actualValues_0 = transform.DepthBounds.Items [0];
			Assert.AreEqual (OVERRIDE_MINDEPTH, actualValues_0.MinDepthBounds);
			Assert.AreEqual (OVERRIDE_MAXDEPTH, actualValues_0.MaxDepthBounds);

			Assert.IsNotNull (transform.DrawItems);
			Assert.AreEqual (1, transform.DrawItems.Count);
			var drawItem_0 = transform.DrawItems [0];
			Assert.AreEqual (0, drawItem_0.DepthBounds);

			// NEXT TEST - IF VALUES DIFFER, CREATE NEW DEPTHBIAS
			var command_1 = new GLCmdDrawCommand{ Pipeline = 0, DepthBounds = null, Draw = new GLCmdInternalDraw{ }  };

			result = transform.InitialiseDrawItem (repo, pass, command_1);
			Assert.IsTrue (result);
			Assert.AreEqual (2, transform.DepthBounds.Count);

			var actualValues_1 = transform.DepthBounds.Items [1];
			Assert.AreEqual (DEFAULT_MINDEPTH, actualValues_1.MinDepthBounds);
			Assert.AreEqual (DEFAULT_MAXDEPTH, actualValues_1.MaxDepthBounds);

			Assert.AreEqual (2, transform.DrawItems.Count);

			var drawItem_1 = transform.DrawItems [1];
			Assert.AreEqual (1, drawItem_1.DepthBounds);

			// NEXT TEST - IF DEPTHBIAS IS SAME, REUSE INDEX 1
			var command_2 = new GLCmdDrawCommand{ Pipeline = 0, DepthBounds = null, Draw = new GLCmdInternalDraw{ }  };

			result = transform.InitialiseDrawItem (repo, pass, command_2);
			Assert.IsTrue (result);
			Assert.AreEqual (2, transform.DepthBounds.Count);

			Assert.AreEqual (3, transform.DrawItems.Count);

			var drawItem_2 = transform.DrawItems [2];
			var index = drawItem_2.DepthBounds;
			Assert.AreEqual (1, index);

			var actualValues_2 = transform.DepthBounds.Items [index];
			Assert.AreEqual (DEFAULT_MINDEPTH, actualValues_2.MinDepthBounds);
			Assert.AreEqual (DEFAULT_MAXDEPTH, actualValues_2.MaxDepthBounds);
		}
	}
}

