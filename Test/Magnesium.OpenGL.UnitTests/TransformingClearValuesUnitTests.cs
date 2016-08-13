using NUnit.Framework;

namespace Magnesium.OpenGL.UnitTests
{
	[TestFixture]
	public class TransformingClearValuesUnitTests
	{
		[TestCase]
		public void InitialiseCheck()
		{
			IGLCmdBufferRepository repo = new GLCmdBufferRepository ();
			Assert.AreEqual (0, repo.GraphicsPipelines.Count);

			ICmdVBOEntrypoint vbo = new MockVertexBufferFactory ();
			var transform = new CmdBufferInstructionSetTransformer (vbo, repo);

			

			Assert.IsNotNull (transform.Pipelines);
			Assert.AreEqual (0, transform.Pipelines.Count);

			Assert.IsNotNull (transform.ClearValues);
			Assert.AreEqual (0, transform.ClearValues.Count);
		}

		[TestCase]
		public void NoPipeline ()
		{
			IGLCmdBufferRepository repo = new GLCmdBufferRepository ();
			Assert.AreEqual (0, repo.GraphicsPipelines.Count);

			ICmdVBOEntrypoint vbo = new MockVertexBufferFactory ();
			var transform = new CmdBufferInstructionSetTransformer (vbo, repo);
			

			var command = new GLCmdDrawCommand{ Pipeline = null, Draw = new GLCmdInternalDraw{ } };

			var actual = transform.InitializeDrawItem (repo, null, command);
			Assert.IsFalse (actual);

			Assert.IsNotNull (transform.Pipelines);
			Assert.AreEqual (0, transform.Pipelines.Count);

			Assert.IsNotNull (transform.ClearValues);
			Assert.AreEqual (0, transform.ClearValues.Count);
		}

		[TestCase]
		public void NoPipeline2 ()
		{
			IGLCmdBufferRepository repo = new GLCmdBufferRepository ();
			Assert.AreEqual (0, repo.GraphicsPipelines.Count);

			ICmdVBOEntrypoint vbo = new MockVertexBufferFactory ();
			var transform = new CmdBufferInstructionSetTransformer (vbo, repo);
			

			var origin = new MockIGLRenderPass ();
			var pass = new GLCmdRenderPassCommand{ Origin = origin};

			var command = new GLCmdDrawCommand{ Pipeline = null, Draw = new GLCmdInternalDraw{ } };

			var actual = transform.InitializeDrawItem (repo, pass, command);
			Assert.IsFalse (actual);

			Assert.IsNotNull (transform.Pipelines);
			Assert.AreEqual (0, transform.Pipelines.Count);

			Assert.IsNotNull (transform.ClearValues);
			Assert.AreEqual (0, transform.ClearValues.Count);
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

			ICmdVBOEntrypoint vbo = new MockVertexBufferFactory ();
			var transform = new CmdBufferInstructionSetTransformer (vbo, repo);
			

			var origin = new MockIGLRenderPass ();
			var pass = new GLCmdRenderPassCommand{ Origin = origin};

			var command = new GLCmdDrawCommand{ Pipeline = null, Draw = new GLCmdInternalDraw{ } };

			var actual = transform.InitializeDrawItem (repo, pass, command);
			Assert.IsFalse (actual);

			Assert.IsNotNull (transform.Pipelines);
			Assert.AreEqual (0, transform.Pipelines.Count);

			Assert.IsNotNull (transform.ClearValues);
			Assert.AreEqual (0, transform.ClearValues.Count);
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

			ICmdVBOEntrypoint vbo = new MockVertexBufferFactory ();
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

			Assert.IsNotNull (transform.ClearValues);
			Assert.AreEqual (1, transform.ClearValues.Count);
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

			ICmdVBOEntrypoint vbo = new MockVertexBufferFactory ();
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

			Assert.IsNotNull (transform.ClearValues);
			Assert.AreEqual (1, transform.ClearValues.Count);

			var command_1 = new GLCmdDrawCommand{ Pipeline = 0, Draw = new GLCmdInternalDraw{ } };

			var actual_1 = transform.InitializeDrawItem (repo, pass, command_1);
			Assert.True (actual_1);

			Assert.IsNotNull (transform.Pipelines);
			Assert.AreEqual (1, transform.Pipelines.Count);

			Assert.IsNotNull (transform.DrawItems);
			Assert.AreEqual (2, transform.DrawItems.Count);

			Assert.IsNotNull (transform.ClearValues);
			Assert.AreEqual (1, transform.ClearValues.Count);
		}

		[TestCase]
		public void OneClearValue ()
		{
			IGLCmdBufferRepository repo = new GLCmdBufferRepository ();

			var FLOAT_VALUE_0 = new MgClearValue{ Color = new MgClearColorValue{ Float32 = new MgColor4f(0.5f, 0.4f, 0.3f, 0.2f )}} ;
			var DEPTH_STENCIL_0 = new MgClearValue{ DepthStencil = new MgClearDepthStencilValue(0.7f, 13)};
			var INT_VALUE_0 = new MgClearValue{ Color = new MgClearColorValue{ Int32 = new MgVec4i(-3, -5, -7 , -1)}};
			var UINT_VALUE_0 = new MgClearValue{ Color = new MgClearColorValue{ Uint32 = new MgVec4Ui(13, 15, 17 , 11)}};

			var EXPECTED = new GLCmdClearValuesParameter {
				Attachments = new GLClearValueArrayItem[]
				{
					new GLClearValueArrayItem
					{
						Attachment = new GLClearAttachmentInfo{AttachmentType= GLClearAttachmentType.COLOR_FLOAT},
						Value = FLOAT_VALUE_0,
					}
					,new GLClearValueArrayItem
					{

						Attachment = new GLClearAttachmentInfo{AttachmentType=GLClearAttachmentType.DEPTH_STENCIL},
						Value = DEPTH_STENCIL_0,
					}
					,new GLClearValueArrayItem
					{
						Attachment = new GLClearAttachmentInfo{AttachmentType=GLClearAttachmentType.COLOR_INT},
						Value = INT_VALUE_0,
					}
					,new GLClearValueArrayItem
					{
						Attachment = new GLClearAttachmentInfo{AttachmentType=GLClearAttachmentType.COLOR_UINT},
						Value = UINT_VALUE_0,
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
					ColorBlendEnums = new GLGraphicsPipelineBlendColorState{ 
						Attachments = new GLGraphicsPipelineBlendColorAttachmentState[]{} 
					},
				}
			);

			Assert.AreEqual (1, repo.GraphicsPipelines.Count);

			ICmdVBOEntrypoint vbo = new MockVertexBufferFactory ();
			var transform = new CmdBufferInstructionSetTransformer (vbo, repo);
			

			var origin = new MockIGLRenderPass ();
			origin.AttachmentFormats = new [] {
				EXPECTED.Attachments[0].Attachment,
				EXPECTED.Attachments[1].Attachment,
				EXPECTED.Attachments[2].Attachment,
				EXPECTED.Attachments[3].Attachment,
			};

			var pass = new GLCmdRenderPassCommand{ Origin = origin, ClearValues = new MgClearValue[]{ FLOAT_VALUE_0, DEPTH_STENCIL_0, INT_VALUE_0, UINT_VALUE_0}};

			var command = new GLCmdDrawCommand{ Pipeline = 0, Draw = new GLCmdInternalDraw{ } };

			var result_0 = transform.InitializeDrawItem (repo, pass, command);
			Assert.True (result_0);

			Assert.IsNotNull (transform.Pipelines);
			Assert.AreEqual (1, transform.Pipelines.Count);

			var pipeline_0 = transform.Pipelines [0];
			Assert.AreEqual (0, pipeline_0.ClearValues);

			Assert.IsNotNull (transform.DrawItems);
			Assert.AreEqual (1, transform.DrawItems.Count);

			Assert.IsNotNull (transform.ClearValues);
			Assert.AreEqual (1, transform.ClearValues.Count);

			var actual_0 = transform.ClearValues [0]; 
			Assert.IsTrue (EXPECTED.Equals (actual_0));
		}

		[TestCase]
		public void ReuseOneClearValue ()
		{
			IGLCmdBufferRepository repo = new GLCmdBufferRepository ();

			var FLOAT_VALUE_0 = new MgClearValue{ Color = new MgClearColorValue{ Float32 = new MgColor4f(0.5f, 0.4f, 0.3f, 0.2f )}} ;
			var DEPTH_STENCIL_0 = new MgClearValue{ DepthStencil = new MgClearDepthStencilValue(0.7f, 13)};
			var INT_VALUE_0 = new MgClearValue{ Color = new MgClearColorValue{ Int32 = new MgVec4i(-3, -5, -7 , -1)}};
			var UINT_VALUE_0 = new MgClearValue{ Color = new MgClearColorValue{ Uint32 = new MgVec4Ui(13, 15, 17 , 11)}};

			var EXPECTED = new GLCmdClearValuesParameter {
				Attachments = new GLClearValueArrayItem[]
				{
					new GLClearValueArrayItem
					{
						Attachment = new GLClearAttachmentInfo{AttachmentType=GLClearAttachmentType.COLOR_FLOAT},
						Value = FLOAT_VALUE_0,
					}
					,new GLClearValueArrayItem
					{

						Attachment = new GLClearAttachmentInfo{AttachmentType=GLClearAttachmentType.DEPTH_STENCIL},
						Value = DEPTH_STENCIL_0,
					}
					,new GLClearValueArrayItem
					{
						Attachment = new GLClearAttachmentInfo{AttachmentType=GLClearAttachmentType.COLOR_INT},
						Value = INT_VALUE_0,
					}
					,new GLClearValueArrayItem
					{
						Attachment = new GLClearAttachmentInfo{AttachmentType=GLClearAttachmentType.COLOR_UINT},
						Value = UINT_VALUE_0,
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
					ColorBlendEnums = new GLGraphicsPipelineBlendColorState{ 
						Attachments = new GLGraphicsPipelineBlendColorAttachmentState[]{} 
					},
				}
			);

			Assert.AreEqual (1, repo.GraphicsPipelines.Count);

			ICmdVBOEntrypoint vbo = new MockVertexBufferFactory ();
			var transform = new CmdBufferInstructionSetTransformer (vbo, repo);
			

			var origin = new MockIGLRenderPass ();
			origin.AttachmentFormats = new [] {
				EXPECTED.Attachments[0].Attachment,
				EXPECTED.Attachments[1].Attachment,
				EXPECTED.Attachments[2].Attachment,
				EXPECTED.Attachments[3].Attachment,
			};

			var pass = new GLCmdRenderPassCommand{ Origin = origin, ClearValues = new []{ FLOAT_VALUE_0, DEPTH_STENCIL_0, INT_VALUE_0, UINT_VALUE_0}};

			var command = new GLCmdDrawCommand{ Pipeline = 0, Draw = new GLCmdInternalDraw{ } };

			var result_0 = transform.InitializeDrawItem (repo, pass, command);
			Assert.True (result_0);

			Assert.IsNotNull (transform.Pipelines);
			Assert.AreEqual (1, transform.Pipelines.Count);

			var pipeline_0 = transform.Pipelines [0];
			Assert.AreEqual (0, pipeline_0.ClearValues);

			Assert.IsNotNull (transform.DrawItems);
			Assert.AreEqual (1, transform.DrawItems.Count);

			Assert.IsNotNull (transform.ClearValues);
			Assert.AreEqual (1, transform.ClearValues.Count);

			var actual_0 = transform.ClearValues [0]; 
			Assert.IsTrue (EXPECTED.Equals (actual_0));

			var command_1 = new GLCmdDrawCommand{ Pipeline = 0, Draw = new GLCmdInternalDraw{ } };

			var result_1 = transform.InitializeDrawItem (repo, pass, command_1);
			Assert.True (result_1);

			Assert.IsNotNull (transform.Pipelines);
			Assert.AreEqual (1, transform.Pipelines.Count);

			Assert.IsNotNull (transform.DrawItems);
			Assert.AreEqual (2, transform.DrawItems.Count);

			Assert.IsNotNull (transform.ClearValues);
			Assert.AreEqual (1, transform.ClearValues.Count);
		}

		[TestCase]
		public void TwoClearValues ()
		{
			IGLCmdBufferRepository repo = new GLCmdBufferRepository ();

			var FLOAT_VALUE_0 = new MgClearValue{ Color = new MgClearColorValue{ Float32 = new MgColor4f(0.5f, 0.4f, 0.3f, 0.2f )}} ;
			var DEPTH_STENCIL_0 = new MgClearValue{ DepthStencil = new MgClearDepthStencilValue(0.7f, 13)};
			var INT_VALUE_0 = new MgClearValue{ Color = new MgClearColorValue{ Int32 = new MgVec4i(-3, -5, -7 , -1)}};
			var UINT_VALUE_0 = new MgClearValue{ Color = new MgClearColorValue{ Uint32 = new MgVec4Ui(13, 15, 17 , 11)}};

			var EXPECTED_0 = new GLCmdClearValuesParameter {
				Attachments = new GLClearValueArrayItem[]
				{
					new GLClearValueArrayItem
					{
						Attachment = new GLClearAttachmentInfo{AttachmentType=GLClearAttachmentType.COLOR_FLOAT},
						Value = FLOAT_VALUE_0,
					}
					,new GLClearValueArrayItem
					{

						Attachment = new GLClearAttachmentInfo{AttachmentType=GLClearAttachmentType.DEPTH_STENCIL},
						Value = DEPTH_STENCIL_0,
					}
					,new GLClearValueArrayItem
					{
						Attachment = new GLClearAttachmentInfo{AttachmentType=GLClearAttachmentType.COLOR_INT},
						Value = INT_VALUE_0,
					}
					,new GLClearValueArrayItem
					{
						Attachment = new GLClearAttachmentInfo{AttachmentType=GLClearAttachmentType.COLOR_UINT},
						Value = UINT_VALUE_0,
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
					ColorBlendEnums = new GLGraphicsPipelineBlendColorState{ 
						Attachments = new GLGraphicsPipelineBlendColorAttachmentState[]{} 
					},
				}
			);

			Assert.AreEqual (1, repo.GraphicsPipelines.Count);

			ICmdVBOEntrypoint vbo = new MockVertexBufferFactory ();
			var transform = new CmdBufferInstructionSetTransformer (vbo, repo);
			

			var origin = new MockIGLRenderPass ();
			origin.AttachmentFormats = new [] {
				EXPECTED_0.Attachments[0].Attachment,
				EXPECTED_0.Attachments[1].Attachment,
				EXPECTED_0.Attachments[2].Attachment,
				EXPECTED_0.Attachments[3].Attachment,
			};

			var pass_0 = new GLCmdRenderPassCommand{ Origin = origin, ClearValues = new []{ FLOAT_VALUE_0, DEPTH_STENCIL_0, INT_VALUE_0, UINT_VALUE_0}};

			var command = new GLCmdDrawCommand{ Pipeline = 0, Draw = new GLCmdInternalDraw{ } };

			var result_0 = transform.InitializeDrawItem (repo, pass_0, command);
			Assert.True (result_0);

			Assert.IsNotNull (transform.Pipelines);
			Assert.AreEqual (1, transform.Pipelines.Count);

			var pipeline_0 = transform.Pipelines [0];
			Assert.AreEqual (0, pipeline_0.ClearValues);

			Assert.IsNotNull (transform.DrawItems);
			Assert.AreEqual (1, transform.DrawItems.Count);

			Assert.IsNotNull (transform.ClearValues);
			Assert.AreEqual (1, transform.ClearValues.Count);

			var actual_0 = transform.ClearValues [0]; 
			Assert.IsTrue (EXPECTED_0.Equals (actual_0));

			var command_1 = new GLCmdDrawCommand{ Pipeline = 0, Draw = new GLCmdInternalDraw{ } };

			var EXPECTED_1 = new GLCmdClearValuesParameter {
				Attachments = new []
				{
					new GLClearValueArrayItem
					{
						Attachment = new GLClearAttachmentInfo{AttachmentType=GLClearAttachmentType.COLOR_FLOAT},
						Value = FLOAT_VALUE_0,
					}
				}
			};

			var pass_1 = new GLCmdRenderPassCommand{ Origin = origin, ClearValues = new []{ FLOAT_VALUE_0}};

			var result_1 = transform.InitializeDrawItem (repo, pass_1, command_1);
			Assert.True (result_1);

			Assert.IsNotNull (transform.Pipelines);
			Assert.AreEqual (2, transform.Pipelines.Count);

			var pipeline_1 = transform.Pipelines [1];
			Assert.AreEqual (1, pipeline_1.ClearValues);

			Assert.IsNotNull (transform.DrawItems);
			Assert.AreEqual (2, transform.DrawItems.Count);

			Assert.IsNotNull (transform.ClearValues);
			Assert.AreEqual (2, transform.ClearValues.Count);

			var actual_1 = transform.ClearValues [1]; 
			Assert.IsTrue (EXPECTED_1.Equals (actual_1));
		}
	}

}

