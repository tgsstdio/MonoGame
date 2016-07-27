using NUnit.Framework;

namespace Magnesium.OpenGL.UnitTests
{
	[TestFixture]
	public class TransformingFrontWriteMasksUnitTests
	{
		[TestCase]
		public void InitialiseCheck()
		{
			IGLCmdBufferRepository repo = new GLCmdBufferRepository ();
			Assert.AreEqual (0, repo.FrontWriteMasks.Count);

			ICmdVBOCapabilities vbo = new MockVertexBufferFactory ();
			var transform = new Transformer (vbo);

			transform.Initialise (repo);

			Assert.IsNotNull (transform.FrontWriteMasks);
			Assert.AreEqual (0, transform.FrontWriteMasks.Count);
		}

		[TestCase]
		public void NoneFound ()
		{
			IGLCmdBufferRepository repo = new GLCmdBufferRepository ();
			Assert.AreEqual (0, repo.FrontWriteMasks.Count);

			ICmdVBOCapabilities vbo = new MockVertexBufferFactory ();
			var transform = new Transformer (vbo);
			transform.Initialise (repo);

			var command = new GLCmdDrawCommand{ FrontWriteMask = null, Draw = new GLCmdInternalDraw{ }  };

			var actual = transform.InitialiseDrawItem (repo, null, command);
			Assert.IsFalse (actual);
			Assert.IsNotNull (transform.FrontWriteMasks);
			Assert.AreEqual (0, transform.FrontWriteMasks.Count);
		}

		[TestCase]
		public void NoPipeline ()
		{
			IGLCmdBufferRepository repo = new GLCmdBufferRepository ();

			const int OVERRIDE_VALUE = 3;
			repo.FrontWriteMasks.Add (OVERRIDE_VALUE);

			Assert.AreEqual (1, repo.FrontWriteMasks.Count);

			ICmdVBOCapabilities vbo = new MockVertexBufferFactory ();
			var transform = new Transformer (vbo);
			transform.Initialise (repo);

			var command = new GLCmdDrawCommand{ Pipeline = null, FrontWriteMask = 0, Draw = new GLCmdInternalDraw{ }  };

			var actual = transform.InitialiseDrawItem (repo, null, command);
			Assert.IsFalse (actual);
			Assert.IsNotNull (transform.FrontWriteMasks);
			Assert.AreEqual (0, transform.FrontWriteMasks.Count);
		}

		[TestCase]
		public void NoOverrideAllowed ()
		{
			IGLCmdBufferRepository repo = new GLCmdBufferRepository ();

			const int OVERRIDE_VALUE = 5;
			repo.FrontWriteMasks.Add(OVERRIDE_VALUE);

			Assert.AreEqual (1, repo.FrontWriteMasks.Count);

			var bindings = new GLVertexBufferBinding[]{ };
			var attributes = new GLVertexInputAttribute[]{ };

			const int DEFAULT_VALUE = 100;

			var origin = new MockIGLRenderPass ();
			var pass = new GLCmdRenderPassCommand{ Origin = origin};
			repo.GraphicsPipelines.Add (new MockGLGraphicsPipeline 
				{
					VertexInput = new GLVertexBufferBinder(bindings, attributes),
					DynamicsStates = 0,
					Front = new GLQueueStencilMasks{ WriteMask = DEFAULT_VALUE},
					Viewports = new GLCmdViewportParameter(0, new MgViewport[]{}),
					Scissors = new GLCmdScissorParameter(0, new MgRect2D[]{}),
					ColorBlendEnums = new GLQueueRendererColorBlendState{ Attachments = new GLQueueColorAttachmentBlendState[]{} },
				}
			);

			Assert.AreEqual (1, repo.GraphicsPipelines.Count);

			ICmdVBOCapabilities vbo = new MockVertexBufferFactory ();
			var transform = new Transformer (vbo);
			transform.Initialise (repo);

			var command = new GLCmdDrawCommand{ Pipeline = 0, FrontWriteMask = 0, Draw = new GLCmdInternalDraw{ }  };

			var result = transform.InitialiseDrawItem (repo, pass, command);
			Assert.IsTrue (result);
			Assert.IsNotNull (transform.FrontWriteMasks);
			Assert.AreEqual (1, transform.FrontWriteMasks.Count);

			float actual = transform.FrontWriteMasks.Items [0];
			Assert.AreEqual (DEFAULT_VALUE, actual);
		}

		[TestCase]
		public void OverrideAllowed ()
		{
			IGLCmdBufferRepository repo = new GLCmdBufferRepository ();

			const int OVERRIDE_VALUE = 5;
			repo.FrontWriteMasks.Add(OVERRIDE_VALUE);

			Assert.AreEqual (1, repo.FrontWriteMasks.Count);

			var bindings = new GLVertexBufferBinding[]{ };
			var attributes = new GLVertexInputAttribute[]{ };

			const int DEFAULT_VALUE = 100;

			var origin = new MockIGLRenderPass ();
			var pass = new GLCmdRenderPassCommand{ Origin = origin};

			repo.GraphicsPipelines.Add (new MockGLGraphicsPipeline 
				{
					VertexInput = new GLVertexBufferBinder(bindings, attributes),
					DynamicsStates = GLGraphicsPipelineDynamicStateFlagBits.STENCIL_WRITE_MASK,
					Front = new GLQueueStencilMasks{WriteMask = DEFAULT_VALUE},
					Viewports = new GLCmdViewportParameter(0, new MgViewport[]{}),
					Scissors = new GLCmdScissorParameter(0, new MgRect2D[]{}),
					ColorBlendEnums = new GLQueueRendererColorBlendState{ Attachments = new GLQueueColorAttachmentBlendState[]{} },
				}
			);

			Assert.AreEqual (1, repo.GraphicsPipelines.Count);

			ICmdVBOCapabilities vbo = new MockVertexBufferFactory ();
			var transform = new Transformer (vbo);
			transform.Initialise (repo);

			// USE OVERRIDE 
			var command_0 = new GLCmdDrawCommand{ Pipeline = 0, FrontWriteMask = 0, Draw = new GLCmdInternalDraw{ }  };

			var result = transform.InitialiseDrawItem (repo, pass, command_0);
			Assert.IsTrue (result);
			Assert.IsNotNull (transform.FrontWriteMasks);
			Assert.AreEqual (1, transform.FrontWriteMasks.Count);

			float actualValues_0 = transform.FrontWriteMasks.Items [0];
			Assert.AreEqual (OVERRIDE_VALUE, actualValues_0);

			Assert.IsNotNull (transform.DrawItems);
			Assert.AreEqual (1, transform.DrawItems.Count);
			var drawItem_0 = transform.DrawItems [0];
			Assert.AreEqual (0, drawItem_0.FrontStencilWriteMask);

			// NEXT TEST - IF VALUES DIFFER, CREATE NEW DEPTHBIAS
			var command_1 = new GLCmdDrawCommand{ Pipeline = 0, FrontWriteMask = null, Draw = new GLCmdInternalDraw{ }  };

			result = transform.InitialiseDrawItem (repo, pass, command_1);
			Assert.IsTrue (result);
			Assert.AreEqual (2, transform.FrontWriteMasks.Count);

			var actualValues_1 = transform.FrontWriteMasks.Items [1];
			Assert.AreEqual (DEFAULT_VALUE, actualValues_1);

			Assert.AreEqual (2, transform.DrawItems.Count);

			var drawItem_1 = transform.DrawItems [1];
			Assert.AreEqual (1, drawItem_1.FrontStencilWriteMask);

			// NEXT TEST - IF DEPTHBIAS IS SAME, REUSE INDEX 1
			var command_2 = new GLCmdDrawCommand{ Pipeline = 0, FrontWriteMask = null, Draw = new GLCmdInternalDraw{ }  };

			result = transform.InitialiseDrawItem (repo, pass, command_2);
			Assert.IsTrue (result);
			Assert.AreEqual (2, transform.FrontWriteMasks.Count);

			Assert.AreEqual (3, transform.DrawItems.Count);

			var drawItem_2 = transform.DrawItems [2];
			var index = drawItem_2.FrontStencilWriteMask;
			Assert.AreEqual (1, index);

			var actualValues_2 = transform.FrontWriteMasks.Items [index];
			Assert.AreEqual (DEFAULT_VALUE, actualValues_2);
		}
	}
}

