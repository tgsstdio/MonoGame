using NUnit.Framework;

namespace Magnesium.OpenGL.UnitTests
{
	[TestFixture]
	public class TransformingColorBlendEnumsUnitTests
	{
		[TestCase]
		public void InitialiseCheck()
		{
			IGLCmdBufferRepository repo = new GLCmdBufferRepository ();
			Assert.AreEqual (0, repo.GraphicsPipelines.Count);

			IGLCmdVBOEntrypoint vbo = new MockVertexBufferFactory ();
			var transform = new CmdBufferInstructionSetTransformer (vbo, repo);

			Assert.IsNotNull (transform.Pipelines);
			Assert.AreEqual (0, transform.Pipelines.Count);

			Assert.IsNotNull (transform.ColorBlendEnums);
			Assert.AreEqual (0, transform.ColorBlendEnums.Count);
		}

		[TestCase]
		public void NoPipeline ()
		{
			IGLCmdBufferRepository repo = new GLCmdBufferRepository ();
			Assert.AreEqual (0, repo.GraphicsPipelines.Count);

			IGLCmdVBOEntrypoint vbo = new MockVertexBufferFactory ();
			var transform = new CmdBufferInstructionSetTransformer (vbo, repo);

			var command = new GLCmdDrawCommand{ Pipeline = null, Draw = new GLCmdInternalDraw{ } };

			var actual = transform.InitializeDrawItem (repo, null, command);
			Assert.IsFalse (actual);

			Assert.IsNotNull (transform.Pipelines);
			Assert.AreEqual (0, transform.Pipelines.Count);

			Assert.IsNotNull (transform.ColorBlendEnums);
			Assert.AreEqual (0, transform.ColorBlendEnums.Count);
		}

		[TestCase]
		public void NoPipeline2 ()
		{
			IGLCmdBufferRepository repo = new GLCmdBufferRepository ();
			Assert.AreEqual (0, repo.GraphicsPipelines.Count);

			IGLCmdVBOEntrypoint vbo = new MockVertexBufferFactory ();
			var transform = new CmdBufferInstructionSetTransformer (vbo, repo);

			var origin = new MockIGLRenderPass ();
			var pass = new GLCmdRenderPassCommand{ Origin = origin};

			var command = new GLCmdDrawCommand{ Pipeline = null, Draw = new GLCmdInternalDraw{ } };

			var actual = transform.InitializeDrawItem (repo, pass, command);
			Assert.IsFalse (actual);

			Assert.IsNotNull (transform.Pipelines);
			Assert.AreEqual (0, transform.Pipelines.Count);

			Assert.IsNotNull (transform.ColorBlendEnums);
			Assert.AreEqual (0, transform.ColorBlendEnums.Count);
		}

		[TestCase]
		public void NoPipeline3 ()
		{
			IGLCmdBufferRepository repo = new GLCmdBufferRepository ();

			var bindings = new GLVertexBufferBinding[]{ };
			var attributes = new GLVertexInputAttribute[]{ };
			repo.GraphicsPipelines.Add (new MockGLGraphicsPipeline 
				{
					VertexInput = new GLVertexBufferBinder(bindings, attributes),
					DynamicsStates = 0,
					Viewports = new GLCmdViewportParameter(0, new MgViewport[]{}),
					Scissors = new GLCmdScissorParameter(0, new MgRect2D[]{}),
					ColorBlendEnums = new GLGraphicsPipelineBlendColorState{ Attachments = new GLGraphicsPipelineBlendColorAttachmentState[]{} },
				}
			);

			Assert.AreEqual (1, repo.GraphicsPipelines.Count);

			IGLCmdVBOEntrypoint vbo = new MockVertexBufferFactory ();
			var transform = new CmdBufferInstructionSetTransformer (vbo, repo);

			var origin = new MockIGLRenderPass ();
			var pass = new GLCmdRenderPassCommand{ Origin = origin};

			var command = new GLCmdDrawCommand{ Pipeline = null, Draw = new GLCmdInternalDraw{ } };

			var actual = transform.InitializeDrawItem (repo, pass, command);
			Assert.IsFalse (actual);

			Assert.IsNotNull (transform.Pipelines);
			Assert.AreEqual (0, transform.Pipelines.Count);

			Assert.IsNotNull (transform.ColorBlendEnums);
			Assert.AreEqual (0, transform.ColorBlendEnums.Count);
		}

		[TestCase]
		public void OneBlankValue ()
		{
			IGLCmdBufferRepository repo = new GLCmdBufferRepository ();

			var bindings = new GLVertexBufferBinding[]{ };
			var attributes = new GLVertexInputAttribute[]{ };
			repo.GraphicsPipelines.Add (new MockGLGraphicsPipeline 
				{
					VertexInput = new GLVertexBufferBinder(bindings, attributes),
					DynamicsStates = 0,
					Viewports = new GLCmdViewportParameter(0, new MgViewport[]{}),
					Scissors = new GLCmdScissorParameter(0, new MgRect2D[]{}),
					ColorBlendEnums = new GLGraphicsPipelineBlendColorState{ Attachments = new GLGraphicsPipelineBlendColorAttachmentState[]{} },
				}
			);

			Assert.AreEqual (1, repo.GraphicsPipelines.Count);

			IGLCmdVBOEntrypoint vbo = new MockVertexBufferFactory ();
			var transform = new CmdBufferInstructionSetTransformer (vbo, repo);

			var origin = new MockIGLRenderPass ();
			var pass = new GLCmdRenderPassCommand{ Origin = origin};

			var command = new GLCmdDrawCommand{ Pipeline = 0, Draw = new GLCmdInternalDraw{ } };

			var actual = transform.InitializeDrawItem (repo, pass, command);
			Assert.True (actual);

			Assert.IsNotNull (transform.Pipelines);
			Assert.AreEqual (1, transform.Pipelines.Count);

			Assert.IsNotNull (transform.DrawItems);
			Assert.AreEqual (1, transform.DrawItems.Count);

			Assert.IsNotNull (transform.ColorBlendEnums);
			Assert.AreEqual (1, transform.ColorBlendEnums.Count);
		}

		[TestCase]
		public void StillOneBlankValue ()
		{
			IGLCmdBufferRepository repo = new GLCmdBufferRepository ();

			var bindings = new GLVertexBufferBinding[]{ };
			var attributes = new GLVertexInputAttribute[]{ };
			repo.GraphicsPipelines.Add (new MockGLGraphicsPipeline 
				{
					VertexInput = new GLVertexBufferBinder(bindings, attributes),
					DynamicsStates = 0,
					Viewports = new GLCmdViewportParameter(0, new MgViewport[]{}),
					Scissors = new GLCmdScissorParameter(0, new MgRect2D[]{}),
					ColorBlendEnums = new GLGraphicsPipelineBlendColorState{ Attachments = new GLGraphicsPipelineBlendColorAttachmentState[]{} },
				}
			);

			Assert.AreEqual (1, repo.GraphicsPipelines.Count);

			IGLCmdVBOEntrypoint vbo = new MockVertexBufferFactory ();
			var transform = new CmdBufferInstructionSetTransformer (vbo, repo);

			var origin = new MockIGLRenderPass ();
			var pass = new GLCmdRenderPassCommand{ Origin = origin};

			var command = new GLCmdDrawCommand{ Pipeline = 0, Draw = new GLCmdInternalDraw{ } };

			var actual = transform.InitializeDrawItem (repo, pass, command);
			Assert.True (actual);

			Assert.IsNotNull (transform.Pipelines);
			Assert.AreEqual (1, transform.Pipelines.Count);

			Assert.IsNotNull (transform.DrawItems);
			Assert.AreEqual (1, transform.DrawItems.Count);

			Assert.IsNotNull (transform.ColorBlendEnums);
			Assert.AreEqual (1, transform.ColorBlendEnums.Count);

			var command_1 = new GLCmdDrawCommand{ Pipeline = 0, Draw = new GLCmdInternalDraw{ } };

			var actual_1 = transform.InitializeDrawItem (repo, pass, command_1);
			Assert.True (actual_1);

			Assert.IsNotNull (transform.Pipelines);
			Assert.AreEqual (1, transform.Pipelines.Count);

			Assert.IsNotNull (transform.DrawItems);
			Assert.AreEqual (2, transform.DrawItems.Count);

			Assert.IsNotNull (transform.ColorBlendEnums);
			Assert.AreEqual (1, transform.ColorBlendEnums.Count);
		}

		[TestCase]
		public void SingleValue ()
		{
			IGLCmdBufferRepository repo = new GLCmdBufferRepository ();

			var EXPECTED = new GLGraphicsPipelineBlendColorState {
				LogicOp = MgLogicOp.INVERT,
				LogicOpEnable = true,
				Attachments = new GLGraphicsPipelineBlendColorAttachmentState[]
				{
					new GLGraphicsPipelineBlendColorAttachmentState
					{
						AlphaBlendOp = MgBlendOp.ADD,
						BlendEnable = true,
						ColorBlendOp = MgBlendOp.MAX,
						ColorWriteMask = MgColorComponentFlagBits.R_BIT,
						DstAlphaBlendFactor = MgBlendFactor.CONSTANT_ALPHA,
						DstColorBlendFactor = MgBlendFactor.CONSTANT_COLOR,
						SrcAlphaBlendFactor = MgBlendFactor.ONE,
						SrcColorBlendFactor = MgBlendFactor.SRC1_COLOR,
					},
					new GLGraphicsPipelineBlendColorAttachmentState
					{
						AlphaBlendOp = MgBlendOp.ADD,
						BlendEnable = true,
						ColorBlendOp = MgBlendOp.MAX,
						ColorWriteMask = MgColorComponentFlagBits.R_BIT,
						DstAlphaBlendFactor = MgBlendFactor.CONSTANT_ALPHA,
						DstColorBlendFactor = MgBlendFactor.CONSTANT_COLOR,
						SrcAlphaBlendFactor = MgBlendFactor.ONE,
						SrcColorBlendFactor = MgBlendFactor.SRC1_COLOR,
					}
				}
			};

			var bindings = new GLVertexBufferBinding[]{ };
			var attributes = new GLVertexInputAttribute[]{ };
			repo.GraphicsPipelines.Add (new MockGLGraphicsPipeline 
				{
					VertexInput = new GLVertexBufferBinder(bindings, attributes),
					DynamicsStates = 0,
					Viewports = new GLCmdViewportParameter(0, new MgViewport[]{}),
					Scissors = new GLCmdScissorParameter(0, new MgRect2D[]{}),
					ColorBlendEnums = EXPECTED,
				}
			);

			Assert.AreEqual (1, repo.GraphicsPipelines.Count);

			IGLCmdVBOEntrypoint vbo = new MockVertexBufferFactory ();
			var transform = new CmdBufferInstructionSetTransformer (vbo, repo);

			var origin = new MockIGLRenderPass ();
			var pass = new GLCmdRenderPassCommand{ Origin = origin };

			var command = new GLCmdDrawCommand{ Pipeline = 0, Draw = new GLCmdInternalDraw{ } };

			var result_0 = transform.InitializeDrawItem (repo, pass, command);
			Assert.True (result_0);

			Assert.IsNotNull (transform.Pipelines);
			Assert.AreEqual (1, transform.Pipelines.Count);

			var pipeline_0 = transform.Pipelines [0];
			Assert.AreEqual (0, pipeline_0.ColorBlendEnums);

			Assert.IsNotNull (transform.DrawItems);
			Assert.AreEqual (1, transform.DrawItems.Count);

			Assert.IsNotNull (transform.ColorBlendEnums);
			Assert.AreEqual (1, transform.ColorBlendEnums.Count);

			var actual_0 = transform.ColorBlendEnums [0]; 
			Assert.IsTrue (EXPECTED.Equals (actual_0));
		}

		[TestCase]
		public void SamePipelineUsed ()
		{
			IGLCmdBufferRepository repo = new GLCmdBufferRepository ();

			var EXPECTED = new GLGraphicsPipelineBlendColorState {
				LogicOp = MgLogicOp.INVERT,
				LogicOpEnable = true,
				Attachments = new GLGraphicsPipelineBlendColorAttachmentState[]
				{
					new GLGraphicsPipelineBlendColorAttachmentState
					{
						AlphaBlendOp = MgBlendOp.ADD,
						BlendEnable = true,
						ColorBlendOp = MgBlendOp.MAX,
						ColorWriteMask = MgColorComponentFlagBits.R_BIT,
						DstAlphaBlendFactor = MgBlendFactor.CONSTANT_ALPHA,
						DstColorBlendFactor = MgBlendFactor.CONSTANT_COLOR,
						SrcAlphaBlendFactor = MgBlendFactor.ONE,
						SrcColorBlendFactor = MgBlendFactor.SRC1_COLOR,
					},
					new GLGraphicsPipelineBlendColorAttachmentState
					{
						AlphaBlendOp = MgBlendOp.ADD,
						BlendEnable = true,
						ColorBlendOp = MgBlendOp.MAX,
						ColorWriteMask = MgColorComponentFlagBits.R_BIT,
						DstAlphaBlendFactor = MgBlendFactor.CONSTANT_ALPHA,
						DstColorBlendFactor = MgBlendFactor.CONSTANT_COLOR,
						SrcAlphaBlendFactor = MgBlendFactor.ONE,
						SrcColorBlendFactor = MgBlendFactor.SRC1_COLOR,
					}
				}
			};

			var bindings = new GLVertexBufferBinding[]{ };
			var attributes = new GLVertexInputAttribute[]{ };
			repo.GraphicsPipelines.Add (new MockGLGraphicsPipeline 
				{
					VertexInput = new GLVertexBufferBinder(bindings, attributes),
					DynamicsStates = 0,
					Viewports = new GLCmdViewportParameter(0, new MgViewport[]{}),
					Scissors = new GLCmdScissorParameter(0, new MgRect2D[]{}),
					ColorBlendEnums = EXPECTED,
				}
			);

			Assert.AreEqual (1, repo.GraphicsPipelines.Count);

			IGLCmdVBOEntrypoint vbo = new MockVertexBufferFactory ();
			var transform = new CmdBufferInstructionSetTransformer (vbo, repo);

			var origin = new MockIGLRenderPass ();
			var pass = new GLCmdRenderPassCommand{ Origin = origin};

			var command = new GLCmdDrawCommand{ Pipeline = 0, Draw = new GLCmdInternalDraw{ } };

			var result_0 = transform.InitializeDrawItem (repo, pass, command);
			Assert.True (result_0);

			Assert.IsNotNull (transform.Pipelines);
			Assert.AreEqual (1, transform.Pipelines.Count);

			var pipeline_0 = transform.Pipelines [0];
			Assert.AreEqual (0, pipeline_0.ColorBlendEnums);

			Assert.IsNotNull (transform.DrawItems);
			Assert.AreEqual (1, transform.DrawItems.Count);

			Assert.IsNotNull (transform.ColorBlendEnums);
			Assert.AreEqual (1, transform.ColorBlendEnums.Count);

			var actual_0 = transform.ColorBlendEnums [0]; 
			Assert.IsTrue (EXPECTED.Equals (actual_0));

			var command_1 = new GLCmdDrawCommand{ Pipeline = 0, Draw = new GLCmdInternalDraw{ } };

			var result_1 = transform.InitializeDrawItem (repo, pass, command_1);
			Assert.True (result_1);

			Assert.IsNotNull (transform.Pipelines);
			Assert.AreEqual (1, transform.Pipelines.Count);

			Assert.IsNotNull (transform.DrawItems);
			Assert.AreEqual (2, transform.DrawItems.Count);

			Assert.IsNotNull (transform.ColorBlendEnums);
			Assert.AreEqual (1, transform.ColorBlendEnums.Count);
		}

		[TestCase]
		public void TwoColorBlendEnums ()
		{
			IGLCmdBufferRepository repo = new GLCmdBufferRepository ();

			var EXPECTED_0 = new GLGraphicsPipelineBlendColorState {
				LogicOp = MgLogicOp.INVERT,
				LogicOpEnable = true,
				Attachments = new GLGraphicsPipelineBlendColorAttachmentState[]
				{
					new GLGraphicsPipelineBlendColorAttachmentState
					{
						AlphaBlendOp = MgBlendOp.ADD,
						BlendEnable = true,
						ColorBlendOp = MgBlendOp.MAX,
						ColorWriteMask = MgColorComponentFlagBits.R_BIT,
						DstAlphaBlendFactor = MgBlendFactor.CONSTANT_ALPHA,
						DstColorBlendFactor = MgBlendFactor.CONSTANT_COLOR,
						SrcAlphaBlendFactor = MgBlendFactor.ONE,
						SrcColorBlendFactor = MgBlendFactor.SRC1_COLOR,
					},
					new GLGraphicsPipelineBlendColorAttachmentState
					{
						AlphaBlendOp = MgBlendOp.ADD,
						BlendEnable = true,
						ColorBlendOp = MgBlendOp.MAX,
						ColorWriteMask = MgColorComponentFlagBits.R_BIT,
						DstAlphaBlendFactor = MgBlendFactor.CONSTANT_ALPHA,
						DstColorBlendFactor = MgBlendFactor.CONSTANT_COLOR,
						SrcAlphaBlendFactor = MgBlendFactor.ONE,
						SrcColorBlendFactor = MgBlendFactor.SRC1_COLOR,
					}
				}
			};

			var bindings = new GLVertexBufferBinding[]{ };
			var attributes = new GLVertexInputAttribute[]{ };
			repo.GraphicsPipelines.Add (new MockGLGraphicsPipeline 
				{
					VertexInput = new GLVertexBufferBinder(bindings, attributes),
					DynamicsStates = 0,
					Viewports = new GLCmdViewportParameter(0, new MgViewport[]{}),
					Scissors = new GLCmdScissorParameter(0, new MgRect2D[]{}),
					ColorBlendEnums = EXPECTED_0,
				}
			);

			Assert.AreEqual (1, repo.GraphicsPipelines.Count);

			IGLCmdVBOEntrypoint vbo = new MockVertexBufferFactory ();
			var transform = new CmdBufferInstructionSetTransformer (vbo, repo);

			var origin = new MockIGLRenderPass ();

			var pass_0 = new GLCmdRenderPassCommand{ Origin = origin };

			var command = new GLCmdDrawCommand{ Pipeline = 0, Draw = new GLCmdInternalDraw{ } };

			var result_0 = transform.InitializeDrawItem (repo, pass_0, command);
			Assert.True (result_0);

			Assert.IsNotNull (transform.Pipelines);
			Assert.AreEqual (1, transform.Pipelines.Count);

			var pipeline_0 = transform.Pipelines [0];
			Assert.AreEqual (0, pipeline_0.ColorBlendEnums);

			Assert.IsNotNull (transform.DrawItems);
			Assert.AreEqual (1, transform.DrawItems.Count);

			Assert.IsNotNull (transform.ColorBlendEnums);
			Assert.AreEqual (1, transform.ColorBlendEnums.Count);

			var actual_0 = transform.ColorBlendEnums [0]; 
			Assert.IsTrue (EXPECTED_0.Equals (actual_0));

			var EXPECTED_1 = new GLGraphicsPipelineBlendColorState {
					LogicOp = MgLogicOp.INVERT,
					LogicOpEnable = true,
					Attachments = new GLGraphicsPipelineBlendColorAttachmentState[]
					{
						new GLGraphicsPipelineBlendColorAttachmentState
						{
							AlphaBlendOp = MgBlendOp.REVERSE_SUBTRACT,
							BlendEnable = true,
							ColorBlendOp = MgBlendOp.SUBTRACT,
							ColorWriteMask = MgColorComponentFlagBits.A_BIT,
							DstAlphaBlendFactor = MgBlendFactor.DST_COLOR,
							DstColorBlendFactor = MgBlendFactor.ONE_MINUS_CONSTANT_ALPHA,
							SrcAlphaBlendFactor = MgBlendFactor.ZERO,
							SrcColorBlendFactor = MgBlendFactor.SRC_ALPHA,
						},
						new GLGraphicsPipelineBlendColorAttachmentState
						{
							AlphaBlendOp = MgBlendOp.SUBTRACT,
							BlendEnable = false,
							ColorBlendOp = MgBlendOp.MIN,
							ColorWriteMask = MgColorComponentFlagBits.G_BIT,
							DstAlphaBlendFactor = MgBlendFactor.ONE_MINUS_DST_ALPHA,
							DstColorBlendFactor = MgBlendFactor.SRC_ALPHA_SATURATE,
							SrcAlphaBlendFactor = MgBlendFactor.ONE_MINUS_SRC_ALPHA,
							SrcColorBlendFactor = MgBlendFactor.ONE,
						}
					}
				};

			repo.GraphicsPipelines.Add (new MockGLGraphicsPipeline 
				{
					VertexInput = new GLVertexBufferBinder(bindings, attributes),
					DynamicsStates = 0,
					Viewports = new GLCmdViewportParameter(0, new MgViewport[]{}),
					Scissors = new GLCmdScissorParameter(0, new MgRect2D[]{}),
					ColorBlendEnums = EXPECTED_1,
				}
			);

			Assert.AreEqual (2, repo.GraphicsPipelines.Count);

			var command_1 = new GLCmdDrawCommand{ Pipeline = 1, Draw = new GLCmdInternalDraw{ } };

			var pass_1 = new GLCmdRenderPassCommand{ Origin = origin };

			var result_1 = transform.InitializeDrawItem (repo, pass_1, command_1);
			Assert.True (result_1);

			Assert.IsNotNull (transform.Pipelines);
			Assert.AreEqual (2, transform.Pipelines.Count);

			var pipeline_1 = transform.Pipelines [1];
			Assert.AreEqual (1, pipeline_1.ColorBlendEnums);

			Assert.IsNotNull (transform.DrawItems);
			Assert.AreEqual (2, transform.DrawItems.Count);

			Assert.IsNotNull (transform.ColorBlendEnums);
			Assert.AreEqual (2, transform.ColorBlendEnums.Count);

			var actual_1 = transform.ColorBlendEnums [1]; 
			Assert.IsTrue (EXPECTED_1.Equals (actual_1));
		}
	}

}

