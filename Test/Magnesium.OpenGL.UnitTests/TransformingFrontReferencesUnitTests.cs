using NUnit.Framework;

namespace Magnesium.OpenGL.UnitTests
{
	[TestFixture]
	public class TransformingFrontReferencesUnitTests
	{
		[TestCase]
		public void InitialiseCheck()
		{
			IGLCmdBufferRepository repo = new GLCmdBufferRepository ();
			Assert.AreEqual (0, repo.FrontReferences.Count);

			IGLCmdVBOEntrypoint vbo = new MockVertexBufferFactory ();
			var transform = new CmdBufferInstructionSetTransformer (vbo, repo);

			

			Assert.IsNotNull (transform.FrontReferences);
			Assert.AreEqual (0, transform.FrontReferences.Count);
		}

		[TestCase]
		public void NoneFound ()
		{
			IGLCmdBufferRepository repo = new GLCmdBufferRepository ();
			Assert.AreEqual (0, repo.FrontReferences.Count);

			IGLCmdVBOEntrypoint vbo = new MockVertexBufferFactory ();
			var transform = new CmdBufferInstructionSetTransformer (vbo, repo);
			

			var command = new GLCmdDrawCommand{ FrontReference = null, Draw = new GLCmdInternalDraw{ }  };

			var actual = transform.InitializeDrawItem (repo, null, command);
			Assert.IsFalse (actual);
			Assert.IsNotNull (transform.FrontReferences);
			Assert.AreEqual (0, transform.FrontReferences.Count);
		}

		[TestCase]
		public void NoPipeline ()
		{
			IGLCmdBufferRepository repo = new GLCmdBufferRepository ();

			const int OVERRIDE_VALUE = 3;
			repo.FrontReferences.Add (OVERRIDE_VALUE);

			Assert.AreEqual (1, repo.FrontReferences.Count);

			IGLCmdVBOEntrypoint vbo = new MockVertexBufferFactory ();
			var transform = new CmdBufferInstructionSetTransformer (vbo, repo);
			

			var command = new GLCmdDrawCommand{ Pipeline = null, FrontReference = 0, Draw = new GLCmdInternalDraw{ }  };

			var actual = transform.InitializeDrawItem (repo, null, command);
			Assert.IsFalse (actual);
			Assert.IsNotNull (transform.FrontReferences);
			Assert.AreEqual (0, transform.FrontReferences.Count);
		}

		[TestCase]
		public void NoOverrideAllowed ()
		{
			IGLCmdBufferRepository repo = new GLCmdBufferRepository ();

			const int OVERRIDE_VALUE = 5;
			repo.FrontReferences.Add(OVERRIDE_VALUE);

			Assert.AreEqual (1, repo.FrontReferences.Count);

			var bindings = new GLVertexBufferBinding[]{ };
			var attributes = new GLVertexInputAttribute[]{ };

			const int DEFAULT_VALUE = 100;

			var origin = new MockIGLRenderPass ();
			var pass = new GLCmdRenderPassCommand{ Origin = origin};

			repo.GraphicsPipelines.Add (new MockGLGraphicsPipeline 
				{
					VertexInput = new GLVertexBufferBinder(bindings, attributes),
					DynamicsStates = 0,
					Front = new GLGraphicsPipelineStencilMasks{ Reference = DEFAULT_VALUE},
					Viewports = new GLCmdViewportParameter(0, new MgViewport[]{}),
					Scissors = new GLCmdScissorParameter(0, new MgRect2D[]{}),
					ColorBlendEnums = new GLGraphicsPipelineBlendColorState{ Attachments = new GLGraphicsPipelineBlendColorAttachmentState[]{} },
				}
			);

			Assert.AreEqual (1, repo.GraphicsPipelines.Count);

			IGLCmdVBOEntrypoint vbo = new MockVertexBufferFactory ();
			var transform = new CmdBufferInstructionSetTransformer (vbo, repo);
			

			var command = new GLCmdDrawCommand{ Pipeline = 0, FrontReference = 0, Draw = new GLCmdInternalDraw{ }  };

			var result = transform.InitializeDrawItem (repo, pass, command);
			Assert.IsTrue (result);
			Assert.IsNotNull (transform.FrontReferences);
			Assert.AreEqual (1, transform.FrontReferences.Count);

			float actual = transform.FrontReferences.Items [0];
			Assert.AreEqual (DEFAULT_VALUE, actual);
		}

		[TestCase]
		public void OverrideAllowed ()
		{
			IGLCmdBufferRepository repo = new GLCmdBufferRepository ();

			const int OVERRIDE_VALUE = 5;
			repo.FrontReferences.Add(OVERRIDE_VALUE);

			Assert.AreEqual (1, repo.FrontReferences.Count);

			var bindings = new GLVertexBufferBinding[]{ };
			var attributes = new GLVertexInputAttribute[]{ };

			const int DEFAULT_VALUE = 100;

			var origin = new MockIGLRenderPass ();
			var pass = new GLCmdRenderPassCommand{ Origin = origin};

			repo.GraphicsPipelines.Add (new MockGLGraphicsPipeline 
				{
					VertexInput = new GLVertexBufferBinder(bindings, attributes),
					DynamicsStates = GLGraphicsPipelineDynamicStateFlagBits.STENCIL_REFERENCE,
					Front = new GLGraphicsPipelineStencilMasks{Reference = DEFAULT_VALUE},
					Viewports = new GLCmdViewportParameter(0, new MgViewport []{}),
					Scissors = new GLCmdScissorParameter(0, new MgRect2D[]{}),
					ColorBlendEnums = new GLGraphicsPipelineBlendColorState{ Attachments = new GLGraphicsPipelineBlendColorAttachmentState[]{} },
				}
			);

			Assert.AreEqual (1, repo.GraphicsPipelines.Count);

			IGLCmdVBOEntrypoint vbo = new MockVertexBufferFactory ();
			var transform = new CmdBufferInstructionSetTransformer (vbo, repo);
			

			// USE OVERRIDE 
			var command_0 = new GLCmdDrawCommand{ Pipeline = 0, FrontReference = 0, Draw = new GLCmdInternalDraw{ }  };

			var result = transform.InitializeDrawItem (repo, pass, command_0);
			Assert.IsTrue (result);
			Assert.IsNotNull (transform.FrontReferences);
			Assert.AreEqual (1, transform.FrontReferences.Count);

			float actualValues_0 = transform.FrontReferences.Items [0];
			Assert.AreEqual (OVERRIDE_VALUE, actualValues_0);

			Assert.IsNotNull (transform.DrawItems);
			Assert.AreEqual (1, transform.DrawItems.Count);
			var drawItem_0 = transform.DrawItems [0];
			Assert.AreEqual (0, drawItem_0.FrontStencilReference);

			// NEXT TEST - IF VALUES DIFFER, CREATE NEW DEPTHBIAS
			var command_1 = new GLCmdDrawCommand{ Pipeline = 0, FrontReference = null, Draw = new GLCmdInternalDraw{ }  };

			result = transform.InitializeDrawItem (repo, pass, command_1);
			Assert.IsTrue (result);
			Assert.AreEqual (2, transform.FrontReferences.Count);

			var actualValues_1 = transform.FrontReferences.Items [1];
			Assert.AreEqual (DEFAULT_VALUE, actualValues_1);

			Assert.AreEqual (2, transform.DrawItems.Count);

			var drawItem_1 = transform.DrawItems [1];
			Assert.AreEqual (1, drawItem_1.FrontStencilReference);

			// NEXT TEST - IF DEPTHBIAS IS SAME, REUSE INDEX 1
			var command_2 = new GLCmdDrawCommand{ Pipeline = 0, FrontReference = null, Draw = new GLCmdInternalDraw{ }  };

			result = transform.InitializeDrawItem (repo, pass, command_2);
			Assert.IsTrue (result);
			Assert.AreEqual (2, transform.FrontReferences.Count);

			Assert.AreEqual (3, transform.DrawItems.Count);

			var drawItem_2 = transform.DrawItems [2];
			var index = drawItem_2.FrontStencilReference;
			Assert.AreEqual (1, index);

			var actualValues_2 = transform.FrontReferences.Items [index];
			Assert.AreEqual (DEFAULT_VALUE, actualValues_2);
		}
	}
}

