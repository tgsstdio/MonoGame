using NUnit.Framework;

namespace Magnesium.OpenGL.UnitTests
{
	[TestFixture]
	public class TransformingBlendConstantsUnitTests
	{
		[TestCase]
		public void InitialiseCheck()
		{
			IGLCmdBufferRepository repo = new GLCmdBufferRepository ();
			Assert.AreEqual (0, repo.BlendConstants.Count);

			ICmdVBOEntrypoint vbo = new MockVertexBufferFactory ();
			var transform = new CmdBufferInstructionSetTransformer (vbo, repo);


			Assert.IsNotNull (transform.BlendConstants);
			Assert.AreEqual (0, transform.BlendConstants.Items.Count);
		}

		[TestCase]
		public void NoneFound ()
		{
			IGLCmdBufferRepository repo = new GLCmdBufferRepository ();
			Assert.AreEqual (0, repo.BlendConstants.Count);

			ICmdVBOEntrypoint vbo = new MockVertexBufferFactory ();
			var transform = new CmdBufferInstructionSetTransformer (vbo, repo);

			var command = new GLCmdDrawCommand{ BlendConstants = null };

			var actual = transform.InitializeDrawItem (repo, null, command);
			Assert.IsFalse (actual);
			Assert.IsNotNull (transform.BlendConstants);
			Assert.AreEqual (0, transform.BlendConstants.Items.Count);
		}

		[TestCase]
		public void NoPipeline ()
		{
			IGLCmdBufferRepository repo = new GLCmdBufferRepository ();

			repo.BlendConstants.Add(new MgColor4f());

			Assert.AreEqual (1, repo.BlendConstants.Count);

			ICmdVBOEntrypoint vbo = new MockVertexBufferFactory ();
			var transform = new CmdBufferInstructionSetTransformer (vbo, repo);

			var command = new GLCmdDrawCommand{ Pipeline = null, BlendConstants = 0, Draw = new GLCmdInternalDraw{ }  };

			var actual = transform.InitializeDrawItem (repo, null, command);
			Assert.IsFalse (actual);
			Assert.IsNotNull (transform.BlendConstants);
			Assert.AreEqual (0, transform.BlendConstants.Items.Count);
		}

		[TestCase]
		public void NoOverrideAllowed ()
		{
			IGLCmdBufferRepository repo = new GLCmdBufferRepository ();

			const float OVERRIDE_R = 5f;
			const float OVERRIDE_G = 7f;
			const float OVERRIDE_B = 13f;
			const float OVERRIDE_A = 17f;
			repo.BlendConstants.Add(new MgColor4f{R = OVERRIDE_R, G = OVERRIDE_G, B = OVERRIDE_B, A = OVERRIDE_A});

			Assert.AreEqual (1, repo.BlendConstants.Count);

			var bindings = new GLVertexBufferBinding[]{ };
			var attributes = new GLVertexInputAttribute[]{ };

			const float DEFAULT_R = 100f;
			const float DEFAULT_G = 200f;
			const float DEFAULT_B = 300f;
			const float DEFAULT_A = 400f;

			var origin = new MockIGLRenderPass ();
			var pass = new GLCmdRenderPassCommand{ Origin = origin};

			repo.GraphicsPipelines.Add (new MockGLGraphicsPipeline 
				{
					VertexInput = new GLVertexBufferBinder(bindings, attributes),
					DynamicsStates = 0,
					BlendConstants = new MgColor4f{R = DEFAULT_R, G = DEFAULT_G, B = DEFAULT_B, A = DEFAULT_A},
					Viewports = new GLCmdViewportParameter(0, new MgViewport[]{}),
					Scissors = new GLCmdScissorParameter(0, new MgRect2D[]{}),
					ColorBlendEnums = new GLGraphicsPipelineBlendColorState{ Attachments = new GLGraphicsPipelineBlendColorAttachmentState[]{} },
				}
			);

			Assert.AreEqual (1, repo.GraphicsPipelines.Count);

			ICmdVBOEntrypoint vbo = new MockVertexBufferFactory ();
			var transform = new CmdBufferInstructionSetTransformer (vbo, repo);

			var command = new GLCmdDrawCommand{ Pipeline = 0, BlendConstants = 0, Draw = new GLCmdInternalDraw{ }  };

			var result = transform.InitializeDrawItem (repo, pass, command);
			Assert.IsTrue (result);
			Assert.IsNotNull (transform.BlendConstants);
			Assert.AreEqual (1, transform.BlendConstants.Items.Count);

			var actual = transform.BlendConstants.Items [0];
			Assert.AreEqual (DEFAULT_R, actual.R);
			Assert.AreEqual (DEFAULT_G, actual.G);
			Assert.AreEqual (DEFAULT_B, actual.B);
			Assert.AreEqual (DEFAULT_A, actual.A);
		}

		[TestCase]
		public void OverrideAllowed ()
		{
			IGLCmdBufferRepository repo = new GLCmdBufferRepository ();

			const float OVERRIDE_R = 5f;
			const float OVERRIDE_G = 7f;
			const float OVERRIDE_B = 13f;
			const float OVERRIDE_A = 17f;
			repo.BlendConstants.Add(new MgColor4f{R = OVERRIDE_R, G = OVERRIDE_G, B = OVERRIDE_B, A = OVERRIDE_A});

			Assert.AreEqual (1, repo.BlendConstants.Count);

			var bindings = new GLVertexBufferBinding[]{ };
			var attributes = new GLVertexInputAttribute[]{ };

			const float DEFAULT_R = 100f;
			const float DEFAULT_G = 200f;
			const float DEFAULT_B = 300f;
			const float DEFAULT_A = 400f;

			var origin = new MockIGLRenderPass ();
			var pass = new GLCmdRenderPassCommand{ Origin = origin};

			repo.GraphicsPipelines.Add (new MockGLGraphicsPipeline 
				{
					VertexInput = new GLVertexBufferBinder(bindings, attributes),
					DynamicsStates = GLGraphicsPipelineDynamicStateFlagBits.BLEND_CONSTANTS,
					BlendConstants = new MgColor4f{R = DEFAULT_R, G = DEFAULT_G, B = DEFAULT_B, A = DEFAULT_A},
					Viewports = new GLCmdViewportParameter(0, new MgViewport[]{}),
					Scissors = new GLCmdScissorParameter(0, new MgRect2D[]{}),
					ColorBlendEnums = new GLGraphicsPipelineBlendColorState{ Attachments = new GLGraphicsPipelineBlendColorAttachmentState[]{} },
				}
			);

			Assert.AreEqual (1, repo.GraphicsPipelines.Count);

			ICmdVBOEntrypoint vbo = new MockVertexBufferFactory ();
			var transform = new CmdBufferInstructionSetTransformer (vbo, repo);


			// USE OVERRIDE 
			var command_0 = new GLCmdDrawCommand{ Pipeline = 0, BlendConstants = 0, Draw = new GLCmdInternalDraw{ }  };

			var result = transform.InitializeDrawItem (repo, pass, command_0);
			Assert.IsTrue (result);
			Assert.IsNotNull (transform.BlendConstants);
			Assert.AreEqual (1, transform.BlendConstants.Items.Count);

			var actualValues_0 = transform.BlendConstants.Items [0];
			Assert.AreEqual (OVERRIDE_R, actualValues_0.R);
			Assert.AreEqual (OVERRIDE_G, actualValues_0.G);
			Assert.AreEqual (OVERRIDE_B, actualValues_0.B);
			Assert.AreEqual (OVERRIDE_A, actualValues_0.A);

			Assert.IsNotNull (transform.DrawItems);
			Assert.AreEqual (1, transform.DrawItems.Count);
			var drawItem_0 = transform.DrawItems [0];
			Assert.AreEqual (0, drawItem_0.BlendConstants);

			// NEXT TEST - IF VALUES DIFFER, CREATE NEW DEPTHBIAS
			var command_1 = new GLCmdDrawCommand{ Pipeline = 0, BlendConstants = null, Draw = new GLCmdInternalDraw{ }  };

			result = transform.InitializeDrawItem (repo, pass, command_1);
			Assert.IsTrue (result);
			Assert.AreEqual (2, transform.BlendConstants.Items.Count);

			var actualValues_1 = transform.BlendConstants.Items [1];
			Assert.AreEqual (DEFAULT_R, actualValues_1.R);
			Assert.AreEqual (DEFAULT_G, actualValues_1.G);
			Assert.AreEqual (DEFAULT_B, actualValues_1.B);
			Assert.AreEqual (DEFAULT_A, actualValues_1.A);

			Assert.AreEqual (2, transform.DrawItems.Count);

			var drawItem_1 = transform.DrawItems [1];
			Assert.AreEqual (1, drawItem_1.BlendConstants);

			// NEXT TEST - IF DEPTHBIAS IS SAME, REUSE INDEX 1
			var command_2 = new GLCmdDrawCommand{ Pipeline = 0, BlendConstants = null, Draw = new GLCmdInternalDraw{ }  };

			result = transform.InitializeDrawItem (repo, pass, command_2);
			Assert.IsTrue (result);
			Assert.AreEqual (2, transform.BlendConstants.Items.Count);

			Assert.AreEqual (3, transform.DrawItems.Count);

			var drawItem_2 = transform.DrawItems [2];
			var index = drawItem_2.BlendConstants;
			Assert.AreEqual (1, index);

			var actualValues_2 = transform.BlendConstants.Items [index];
			Assert.AreEqual (DEFAULT_R, actualValues_2.R);
			Assert.AreEqual (DEFAULT_G, actualValues_2.G);
			Assert.AreEqual (DEFAULT_B, actualValues_2.B);
			Assert.AreEqual (DEFAULT_A, actualValues_2.A);
		}
	}
}

