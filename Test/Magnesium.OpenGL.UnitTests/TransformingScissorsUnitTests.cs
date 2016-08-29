using NUnit.Framework;

namespace Magnesium.OpenGL.UnitTests
{
	[TestFixture]
	public class TransformingScissorsUnitTests
	{
		[TestCase]
		public void InsertEmptyScissor()
		{
			IGLCmdBufferRepository repo = new GLCmdBufferRepository ();
			Assert.AreEqual (0, repo.Scissors.Count);

			IGLCmdVBOEntrypoint vbo = new MockVertexBufferFactory ();
			var transform = new CmdBufferInstructionSetTransformer (vbo, repo);

			

			Assert.IsNotNull (transform.Scissors);
			Assert.AreEqual (1, transform.Scissors.Count);

			var expected = new GLCmdScissorParameter (0, new MgRect2D [] { });
			var actual = transform.Scissors.Items [0];

			Assert.IsTrue (expected.Equals (actual));

		}

		[TestCase]
		public void NoneFound ()
		{
			IGLCmdBufferRepository repo = new GLCmdBufferRepository ();
			Assert.AreEqual (0, repo.Scissors.Count);

			IGLCmdVBOEntrypoint vbo = new MockVertexBufferFactory ();
			var transform = new CmdBufferInstructionSetTransformer (vbo, repo);
			

			var command = new GLCmdDrawCommand{ Scissors = null, Draw = new GLCmdInternalDraw{ }  };

			var actual = transform.InitializeDrawItem (repo, null, command);
			Assert.IsFalse (actual);
			Assert.IsNotNull (transform.Scissors);
			Assert.AreEqual (1, transform.Scissors.Count);

			var expected = new GLCmdScissorParameter (0, new MgRect2D[] { });
			var first = transform.Scissors.Items [0];

			Assert.IsTrue (expected.Equals (first));
		}

		[TestCase]
		public void NoPipeline ()
		{
			IGLCmdBufferRepository repo = new GLCmdBufferRepository ();

			repo.Scissors.Add(
				new GLCmdScissorParameter(
					0, 
					new []
					{
						new MgRect2D{
							Offset = new MgOffset2D
							{
								X = 0,
								Y = 0,
							},
							Extent = new MgExtent2D
							{								
								Width = 100,
								Height = 200,
							},
						},
						new MgRect2D{
							Offset = new MgOffset2D
							{							
								X = 100,
								Y = 200,
							},
							Extent = new MgExtent2D
							{
								Width = 100,
								Height = 200,
							}
						},
					})
			);

			Assert.AreEqual (1, repo.Scissors.Count);

			IGLCmdVBOEntrypoint vbo = new MockVertexBufferFactory ();
			var transform = new CmdBufferInstructionSetTransformer (vbo, repo);
			

			var command = new GLCmdDrawCommand{ Pipeline = null, Scissors = 0, Draw = new GLCmdInternalDraw{ }  };

			var actual = transform.InitializeDrawItem (repo, null, command);
			Assert.IsFalse (actual);
			Assert.IsNotNull (transform.Scissors);
			Assert.AreEqual (0, transform.Scissors.Count);
		}

		[TestCase]
		public void NoOverrideAllowed ()
		{
			IGLCmdBufferRepository repo = new GLCmdBufferRepository ();

			{
				const int OVERRIDE_X_0 = 3;
				const int OVERRIDE_Y_0 = 5;
				const uint OVERRIDE_WIDTH_0 = 7;
				const uint OVERRIDE_HEIGHT_0 = 11;

				const int OVERRIDE_X_1 = 101;
				const int OVERRIDE_Y_1 = 103;
				const uint OVERRIDE_WIDTH_1 = 105;
				const uint OVERRIDE_HEIGHT_1 = 107;

				var OVERRIDE_VIEWPORT = new GLCmdScissorParameter (
                        0, 
                        new [] {
						new MgRect2D {
							Offset = new MgOffset2D
							{							
							X = OVERRIDE_X_0,
							Y = OVERRIDE_Y_0,
							},
							Extent = new MgExtent2D
							{
							Width = OVERRIDE_WIDTH_0,
							Height = OVERRIDE_HEIGHT_0,
							}
						},
						new MgRect2D {
							Offset = new MgOffset2D
							{
							X = OVERRIDE_X_1,
							Y = OVERRIDE_Y_1,
							},
							Extent = new MgExtent2D
							{
							Width = OVERRIDE_WIDTH_1,
							Height = OVERRIDE_HEIGHT_1,
							}
						},
					});

				repo.Scissors.Add (OVERRIDE_VIEWPORT);
			}

			Assert.AreEqual (1, repo.Scissors.Count);

			var bindings = new GLVertexBufferBinding[]{ };
			var attributes = new GLVertexInputAttribute[]{ };

			const int DEFAULT_X_0 = 203;
			const int DEFAULT_Y_0 = 205;
			const uint DEFAULT_WIDTH_0 = 207;
			const uint DEFAULT_HEIGHT_0 = 211;

			const int DEFAULT_X_1 = 501;
			const int DEFAULT_Y_1 = 503;
			const uint DEFAULT_WIDTH_1 = 505;
			const uint DEFAULT_HEIGHT_1 = 507;


			var DEFAULT_SCISSORS = new GLCmdScissorParameter (
               	0, 
           		new [] {
					new MgRect2D {
						Offset = new MgOffset2D
						{						
						X = DEFAULT_X_0,
						Y = DEFAULT_Y_0,
						},
						Extent = new MgExtent2D
						{
						Width = DEFAULT_WIDTH_0,
						Height = DEFAULT_HEIGHT_0,
						}
					},
					new MgRect2D {
						Offset = new MgOffset2D
						{						
						X = DEFAULT_X_1,
						Y = DEFAULT_Y_1,
						},
						Extent = new MgExtent2D
						{
						Width = DEFAULT_WIDTH_1,
						Height = DEFAULT_HEIGHT_1,
						}
					},
				});

			var origin = new MockIGLRenderPass ();
			var pass = new GLCmdRenderPassCommand{ Origin = origin};

			repo.GraphicsPipelines.Add (new MockGLGraphicsPipeline 
				{
					VertexInput = new GLVertexBufferBinder(bindings, attributes),
					DynamicsStates = 0,
					Viewports = new GLCmdViewportParameter(0, new MgViewport[] {}),
					Scissors = DEFAULT_SCISSORS,
					ColorBlendEnums = new GLGraphicsPipelineBlendColorState{ Attachments = new GLGraphicsPipelineBlendColorAttachmentState[]{} },
				}
			);

			Assert.AreEqual (1, repo.GraphicsPipelines.Count);

			IGLCmdVBOEntrypoint vbo = new MockVertexBufferFactory ();
			var transform = new CmdBufferInstructionSetTransformer (vbo, repo);
			

			Assert.AreEqual (0, transform.Scissors.Count);

			var command = new GLCmdDrawCommand{ Pipeline = 0, Scissors = 0, Draw = new GLCmdInternalDraw{ }  };

			var result = transform.InitializeDrawItem (repo, pass, command);
			Assert.IsTrue (result);
			Assert.IsNotNull (transform.Scissors);
			Assert.AreEqual (1, transform.Scissors.Count);

			var actual = transform.Scissors.Items [0];

			Assert.IsTrue (DEFAULT_SCISSORS.Equals(actual));
		}

		[TestCase]
		public void OverrideAllowed ()
		{
			IGLCmdBufferRepository repo = new GLCmdBufferRepository ();


			const int OVERRIDE_X_0 = 3;
			const int OVERRIDE_Y_0 = 5;
			const uint OVERRIDE_WIDTH_0 = 7;
			const uint OVERRIDE_HEIGHT_0 = 11;

			const int OVERRIDE_X_1 = 101;
			const int OVERRIDE_Y_1 = 103;
			const uint OVERRIDE_WIDTH_1 = 105;
			const uint OVERRIDE_HEIGHT_1 = 107;

			var OVERRIDE_SCISSORS = new GLCmdScissorParameter (
				0, 
				new [] {
					new MgRect2D {
						Offset = new MgOffset2D
						{							
							X = OVERRIDE_X_0,
							Y = OVERRIDE_Y_0,
						},
						Extent = new MgExtent2D
						{
							Width = OVERRIDE_WIDTH_0,
							Height = OVERRIDE_HEIGHT_0,
						}
					},
					new MgRect2D {
						Offset = new MgOffset2D
						{
							X = OVERRIDE_X_1,
							Y = OVERRIDE_Y_1,
						},
						Extent = new MgExtent2D
						{
							Width = OVERRIDE_WIDTH_1,
							Height = OVERRIDE_HEIGHT_1,
						}
					},
				});

			repo.Scissors.Add (OVERRIDE_SCISSORS);


			Assert.AreEqual (1, repo.Scissors.Count);

			var bindings = new GLVertexBufferBinding[]{ };
			var attributes = new GLVertexInputAttribute[]{ };

			const int DEFAULT_X_0 = 203;
			const int DEFAULT_Y_0 = 205;
			const uint DEFAULT_WIDTH_0 = 207;
			const uint DEFAULT_HEIGHT_0 = 211;

			const int DEFAULT_X_1 = 501;
			const int DEFAULT_Y_1 = 503;
			const uint DEFAULT_WIDTH_1 = 505;
			const uint DEFAULT_HEIGHT_1 = 507;


			var DEFAULT_SCISSORS = new GLCmdScissorParameter (
				0, 
				new [] {
					new MgRect2D {
						Offset = new MgOffset2D
						{						
							X = DEFAULT_X_0,
							Y = DEFAULT_Y_0,
						},
						Extent = new MgExtent2D
						{
							Width = DEFAULT_WIDTH_0,
							Height = DEFAULT_HEIGHT_0,
						}
					},
					new MgRect2D {
						Offset = new MgOffset2D
						{						
							X = DEFAULT_X_1,
							Y = DEFAULT_Y_1,
						},
						Extent = new MgExtent2D
						{
							Width = DEFAULT_WIDTH_1,
							Height = DEFAULT_HEIGHT_1,
						}
					},
				});

			var origin = new MockIGLRenderPass ();
			var pass = new GLCmdRenderPassCommand{ Origin = origin};

			repo.GraphicsPipelines.Add (new MockGLGraphicsPipeline 
				{
					VertexInput = new GLVertexBufferBinder(bindings, attributes),
					DynamicsStates = GLGraphicsPipelineDynamicStateFlagBits.SCISSOR,
					Viewports = new GLCmdViewportParameter(0, new MgViewport[] {}),
					Scissors = DEFAULT_SCISSORS,
					ColorBlendEnums = new GLGraphicsPipelineBlendColorState{ Attachments = new GLGraphicsPipelineBlendColorAttachmentState[]{} },
				}
			);

			Assert.AreEqual (1, repo.GraphicsPipelines.Count);

			IGLCmdVBOEntrypoint vbo = new MockVertexBufferFactory ();
			var transform = new CmdBufferInstructionSetTransformer (vbo, repo);
			

			// USE OVERRIDE 
			var command_0 = new GLCmdDrawCommand{ Pipeline = 0, Scissors = 0, Draw = new GLCmdInternalDraw{} };

			var result = transform.InitializeDrawItem (repo, pass, command_0);
			Assert.IsTrue (result);
			Assert.IsNotNull (transform.Scissors);
			Assert.AreEqual (1, transform.Scissors.Count);

			var actualValues_0 = transform.Scissors.Items [0];
			Assert.IsTrue (OVERRIDE_SCISSORS.Equals(actualValues_0));

			Assert.IsNotNull (transform.DrawItems);
			Assert.AreEqual (1, transform.DrawItems.Count);

			var drawItem_0 = transform.DrawItems [0];
			Assert.AreEqual (0, drawItem_0.Scissor);

			// NEXT TEST - IF VALUES DIFFER, CREATE NEW 
			var command_1 = new GLCmdDrawCommand{ Pipeline = 0, Scissors = null, Draw = new GLCmdInternalDraw{} };

			result = transform.InitializeDrawItem (repo, pass, command_1);
			Assert.IsTrue (result);
			Assert.AreEqual (2, transform.Scissors.Count);

			var actualValues_1 = transform.Scissors.Items [1];
			Assert.IsTrue (DEFAULT_SCISSORS.Equals(actualValues_1));

			Assert.AreEqual (2, transform.DrawItems.Count);

			var drawItem_1 = transform.DrawItems [1];
			Assert.AreEqual (1, drawItem_1.Scissor);

			// NEXT TEST - IF DEPTHBIAS IS SAME, REUSE INDEX 1
			var command_2 = new GLCmdDrawCommand{ Pipeline = 0, Scissors = null, Draw = new GLCmdInternalDraw{} };

			result = transform.InitializeDrawItem (repo, pass, command_2);
			Assert.IsTrue (result);
			Assert.AreEqual (2, transform.Scissors.Count);

			Assert.AreEqual (3, transform.DrawItems.Count);

			var drawItem_2 = transform.DrawItems [2];
			var index = drawItem_2.Scissor;
			Assert.AreEqual (1, index);

			var actualValues_2 = transform.Scissors.Items [index];
			Assert.IsTrue (DEFAULT_SCISSORS.Equals(actualValues_2));
		}
	}
}

