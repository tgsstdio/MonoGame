using NUnit.Framework;

namespace Magnesium.OpenGL.UnitTests
{
	[TestFixture]
	public class TransformingFrontCompareMasksUnitTests
	{
		[TestCase]
		public void InitialiseCheck()
		{
			IGLCmdBufferRepository repo = new GLCmdBufferRepository ();
			Assert.AreEqual (0, repo.FrontCompareMasks.Count);

			ICmdVBOCapabilities vbo = new MockVertexBufferFactory ();
			var transform = new Transformer (vbo, repo);


			Assert.IsNotNull (transform.FrontCompareMasks);
			Assert.AreEqual (0, transform.FrontCompareMasks.Count);
		}

		[TestCase]
		public void NoneFound ()
		{
			IGLCmdBufferRepository repo = new GLCmdBufferRepository ();
			Assert.AreEqual (0, repo.FrontCompareMasks.Count);

			ICmdVBOCapabilities vbo = new MockVertexBufferFactory ();
			var transform = new Transformer (vbo, repo);

			var command = new GLCmdDrawCommand{ FrontCompareMask = null, Draw = new GLCmdInternalDraw{ }  };

			var actual = transform.InitialiseDrawItem (repo, null, command);
			Assert.IsFalse (actual);
			Assert.IsNotNull (transform.FrontCompareMasks);
			Assert.AreEqual (0, transform.FrontCompareMasks.Count);
		}

		[TestCase]
		public void NoPipeline ()
		{
			IGLCmdBufferRepository repo = new GLCmdBufferRepository ();

			const int OVERRIDE_VALUE = 3;
			repo.FrontCompareMasks.Add (OVERRIDE_VALUE);

			Assert.AreEqual (1, repo.FrontCompareMasks.Count);

			ICmdVBOCapabilities vbo = new MockVertexBufferFactory ();
			var transform = new Transformer (vbo, repo);

			var command = new GLCmdDrawCommand{ Pipeline = null, FrontCompareMask = 0, Draw = new GLCmdInternalDraw{ }  };

			var actual = transform.InitialiseDrawItem (repo, null, command);
			Assert.IsFalse (actual);
			Assert.IsNotNull (transform.FrontCompareMasks);
			Assert.AreEqual (0, transform.FrontCompareMasks.Count);
		}

		[TestCase]
		public void NoOverrideAllowed ()
		{
			IGLCmdBufferRepository repo = new GLCmdBufferRepository ();

			const int OVERRIDE_VALUE = 5;
			repo.FrontCompareMasks.Add(OVERRIDE_VALUE);

			Assert.AreEqual (1, repo.FrontCompareMasks.Count);

			var bindings = new GLVertexBufferBinding[]{ };
			var attributes = new GLVertexInputAttribute[]{ };

			const int DEFAULT_VALUE = 100;

			var origin = new MockIGLRenderPass ();
			var pass = new GLCmdRenderPassCommand{ Origin = origin};

			repo.GraphicsPipelines.Add (new MockGLGraphicsPipeline 
				{
					VertexInput = new GLVertexBufferBinder(bindings, attributes),
					DynamicsStates = 0,
					Front = new GLQueueStencilMasks{ CompareMask = DEFAULT_VALUE},
					Viewports = new GLCmdViewportParameter(0, new MgViewport[]{}),
					Scissors = new GLCmdScissorParameter(0, new MgRect2D[]{}),
					ColorBlendEnums = new GLQueueRendererColorBlendState{ Attachments = new GLQueueColorAttachmentBlendState[]{} },
				}
			);

			Assert.AreEqual (1, repo.GraphicsPipelines.Count);

			ICmdVBOCapabilities vbo = new MockVertexBufferFactory ();
			var transform = new Transformer (vbo, repo);

			var command = new GLCmdDrawCommand{ Pipeline = 0, FrontCompareMask = 0, Draw = new GLCmdInternalDraw{ }  };

			var result = transform.InitialiseDrawItem (repo, pass, command);
			Assert.IsTrue (result);
			Assert.IsNotNull (transform.FrontCompareMasks);
			Assert.AreEqual (1, transform.FrontCompareMasks.Count);

			float actual = transform.FrontCompareMasks.Items [0];
			Assert.AreEqual (DEFAULT_VALUE, actual);
		}

		[TestCase]
		public void OverrideAllowed ()
		{
			IGLCmdBufferRepository repo = new GLCmdBufferRepository ();

			const int OVERRIDE_VALUE = 5;
			repo.FrontCompareMasks.Add(OVERRIDE_VALUE);

			Assert.AreEqual (1, repo.FrontCompareMasks.Count);

			var bindings = new GLVertexBufferBinding[]{ };
			var attributes = new GLVertexInputAttribute[]{ };

			const int DEFAULT_VALUE = 100;

			var origin = new MockIGLRenderPass ();
			var pass = new GLCmdRenderPassCommand{ Origin = origin};

			repo.GraphicsPipelines.Add (new MockGLGraphicsPipeline 
				{
					VertexInput = new GLVertexBufferBinder(bindings, attributes),
					DynamicsStates = GLGraphicsPipelineDynamicStateFlagBits.STENCIL_COMPARE_MASK,
					Front = new GLQueueStencilMasks{CompareMask = DEFAULT_VALUE},
					Viewports = new GLCmdViewportParameter(0, new MgViewport[]{}),
					Scissors = new GLCmdScissorParameter(0, new MgRect2D[]{}),
					ColorBlendEnums = new GLQueueRendererColorBlendState{ Attachments = new GLQueueColorAttachmentBlendState[]{} },
				}
			);

			Assert.AreEqual (1, repo.GraphicsPipelines.Count);

			ICmdVBOCapabilities vbo = new MockVertexBufferFactory ();
			var transform = new Transformer (vbo, repo);

			// USE OVERRIDE 
			var command_0 = new GLCmdDrawCommand{ Pipeline = 0, FrontCompareMask = 0, Draw = new GLCmdInternalDraw{ }  };

			var result = transform.InitialiseDrawItem (repo, pass, command_0);
			Assert.IsTrue (result);
			Assert.IsNotNull (transform.FrontCompareMasks);
			Assert.AreEqual (1, transform.FrontCompareMasks.Count);

			float actualValues_0 = transform.FrontCompareMasks.Items [0];
			Assert.AreEqual (OVERRIDE_VALUE, actualValues_0);

			Assert.IsNotNull (transform.DrawItems);
			Assert.AreEqual (1, transform.DrawItems.Count);
			var drawItem_0 = transform.DrawItems [0];
			Assert.AreEqual (0, drawItem_0.FrontStencilCompareMask);

			// NEXT TEST - IF VALUES DIFFER, CREATE NEW DEPTHBIAS
			var command_1 = new GLCmdDrawCommand{ Pipeline = 0, FrontCompareMask = null, Draw = new GLCmdInternalDraw{ }  };

			result = transform.InitialiseDrawItem (repo, pass, command_1);
			Assert.IsTrue (result);
			Assert.AreEqual (2, transform.FrontCompareMasks.Count);

			var actualValues_1 = transform.FrontCompareMasks.Items [1];
			Assert.AreEqual (DEFAULT_VALUE, actualValues_1);

			Assert.AreEqual (2, transform.DrawItems.Count);

			var drawItem_1 = transform.DrawItems [1];
			Assert.AreEqual (1, drawItem_1.FrontStencilCompareMask);

			// NEXT TEST - IF DEPTHBIAS IS SAME, REUSE INDEX 1
			var command_2 = new GLCmdDrawCommand{ Pipeline = 0, FrontCompareMask = null, Draw = new GLCmdInternalDraw{ }  };

			result = transform.InitialiseDrawItem (repo, pass, command_2);
			Assert.IsTrue (result);
			Assert.AreEqual (2, transform.FrontCompareMasks.Count);

			Assert.AreEqual (3, transform.DrawItems.Count);

			var drawItem_2 = transform.DrawItems [2];
			var index = drawItem_2.FrontStencilCompareMask;
			Assert.AreEqual (1, index);

			var actualValues_2 = transform.FrontCompareMasks.Items [index];
			Assert.AreEqual (DEFAULT_VALUE, actualValues_2);
		}
	}
}

