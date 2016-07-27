using NUnit.Framework;

namespace Magnesium.OpenGL.UnitTests
{
	[TestFixture]
	public class TransformingDepthBiasUnitTests
	{
		[TestCase]
		public void InitialiseCheck()
		{
			IGLCmdBufferRepository repo = new GLCmdBufferRepository ();
			Assert.AreEqual (0, repo.DepthBias.Count);

			ICmdVBOCapabilities vbo = new MockVertexBufferFactory ();
			var transform = new Transformer (vbo);

			transform.Initialise (repo);

			Assert.IsNotNull (transform.DepthBias);
			Assert.AreEqual (0, transform.DepthBias.Count);
		}

		[TestCase]
		public void NoneFound ()
		{
			IGLCmdBufferRepository repo = new GLCmdBufferRepository ();
			Assert.AreEqual (0, repo.DepthBias.Count);

			ICmdVBOCapabilities vbo = new MockVertexBufferFactory ();
			var transform = new Transformer (vbo);
			transform.Initialise (repo);

			var command = new GLCmdDrawCommand{ DepthBias = null , Draw = new GLCmdInternalDraw{ } };

			var actual = transform.InitialiseDrawItem (repo, null, command);
			Assert.IsFalse (actual);
			Assert.IsNotNull (transform.DepthBias);
			Assert.AreEqual (0, transform.DepthBias.Count);
		}

		[TestCase]
		public void NoPipeline ()
		{
			IGLCmdBufferRepository repo = new GLCmdBufferRepository ();

			repo.DepthBias.Add(new GLCmdDepthBiasParameter
				{
					DepthBiasClamp = 5f,
					DepthBiasConstantFactor = 7f,
					DepthBiasSlopeFactor = 13f,
				}
			);

			Assert.AreEqual (1, repo.DepthBias.Count);

			ICmdVBOCapabilities vbo = new MockVertexBufferFactory ();
			var transform = new Transformer (vbo);
			transform.Initialise (repo);

			var command = new GLCmdDrawCommand{ Pipeline = null, DepthBias = 0 , Draw = new GLCmdInternalDraw{ } };

			var actual = transform.InitialiseDrawItem (repo, null, command);
			Assert.IsFalse (actual);
			Assert.IsNotNull (transform.DepthBias);
			Assert.AreEqual (0, transform.DepthBias.Count);
		}

		[TestCase]
		public void NoOverrideAllowed ()
		{
			IGLCmdBufferRepository repo = new GLCmdBufferRepository ();

			const float OVERRIDE_CLAMP = 5f;
			const float OVERRIDE_CONSTANT_FACTOR = 7f;
			const float OVERRIDE_SLOPE = 13f;
			repo.DepthBias.Add(new GLCmdDepthBiasParameter
				{
				DepthBiasClamp = OVERRIDE_CLAMP,
				DepthBiasConstantFactor = OVERRIDE_CONSTANT_FACTOR,
				DepthBiasSlopeFactor = OVERRIDE_SLOPE,
				}
			);

			Assert.AreEqual (1, repo.DepthBias.Count);

			var bindings = new GLVertexBufferBinding[]{ };
			var attributes = new GLVertexInputAttribute[]{ };

			const float DEFAULT_CLAMP = 100f;
			const float DEFAULT_CONSTANT_FACTOR = 200f;
			const float DEFAULT_SLOPE = 300f;

			var origin = new MockIGLRenderPass ();
			var pass = new GLCmdRenderPassCommand{ Origin = origin};

			repo.GraphicsPipelines.Add (new MockGLGraphicsPipeline 
				{
					VertexInput = new GLVertexBufferBinder(bindings, attributes),
					DynamicsStates = 0,
					DepthBiasClamp = DEFAULT_CLAMP,
					DepthBiasConstantFactor = DEFAULT_CONSTANT_FACTOR,
					DepthBiasSlopeFactor = DEFAULT_SLOPE,
					Viewports = new GLCmdViewportParameter(0, new MgViewport[]{}),
					Scissors = new GLCmdScissorParameter(0, new MgRect2D[]{}),
					ColorBlendEnums = new GLQueueRendererColorBlendState{ Attachments = new GLQueueColorAttachmentBlendState[]{} },
				}
			);

			Assert.AreEqual (1, repo.GraphicsPipelines.Count);

			ICmdVBOCapabilities vbo = new MockVertexBufferFactory ();
			var transform = new Transformer (vbo);
			transform.Initialise (repo);

			var command = new GLCmdDrawCommand{ Pipeline = 0, DepthBias = 0 , Draw = new GLCmdInternalDraw{ } };

			var result = transform.InitialiseDrawItem (repo, pass, command);
			Assert.IsTrue (result);
			Assert.IsNotNull (transform.DepthBias);
			Assert.AreEqual (1, transform.DepthBias.Count);

			var actual = transform.DepthBias [0];
			Assert.AreEqual (DEFAULT_CLAMP, actual.DepthBiasClamp);
			Assert.AreEqual (DEFAULT_CONSTANT_FACTOR, actual.DepthBiasConstantFactor);
			Assert.AreEqual (DEFAULT_SLOPE, actual.DepthBiasSlopeFactor);
		}

		[TestCase]
		public void OverrideAllowed ()
		{
			IGLCmdBufferRepository repo = new GLCmdBufferRepository ();

			const float OVERRIDE_CLAMP = 5f;
			const float OVERRIDE_CONSTANT_FACTOR = 7f;
			const float OVERRIDE_SLOPE = 13f;
			repo.DepthBias.Add(new GLCmdDepthBiasParameter
				{
					DepthBiasClamp = OVERRIDE_CLAMP,
					DepthBiasConstantFactor = OVERRIDE_CONSTANT_FACTOR,
					DepthBiasSlopeFactor = OVERRIDE_SLOPE,
				}
			);

			Assert.AreEqual (1, repo.DepthBias.Count);

			var bindings = new GLVertexBufferBinding[]{ };
			var attributes = new GLVertexInputAttribute[]{ };

			const float DEFAULT_CLAMP = 100f;
			const float DEFAULT_CONSTANT_FACTOR = 200f;
			const float DEFAULT_SLOPE = 300f;

			var origin = new MockIGLRenderPass ();
			var pass = new GLCmdRenderPassCommand{ Origin = origin};

			repo.GraphicsPipelines.Add (new MockGLGraphicsPipeline 
				{
					VertexInput = new GLVertexBufferBinder(bindings, attributes),
					DynamicsStates = GLGraphicsPipelineDynamicStateFlagBits.DEPTH_BIAS,
					DepthBiasClamp = DEFAULT_CLAMP,
					DepthBiasConstantFactor = DEFAULT_CONSTANT_FACTOR,
					DepthBiasSlopeFactor = DEFAULT_SLOPE,
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
			var command_0 = new GLCmdDrawCommand{ Pipeline = 0, DepthBias = 0, Draw = new GLCmdInternalDraw{ }  };

			var result = transform.InitialiseDrawItem (repo, pass, command_0);
			Assert.IsTrue (result);
			Assert.IsNotNull (transform.DepthBias);
			Assert.AreEqual (1, transform.DepthBias.Count);

			var actualValues_0 = transform.DepthBias [0];
			Assert.AreEqual (OVERRIDE_CLAMP, actualValues_0.DepthBiasClamp);
			Assert.AreEqual (OVERRIDE_CONSTANT_FACTOR, actualValues_0.DepthBiasConstantFactor);
			Assert.AreEqual (OVERRIDE_SLOPE, actualValues_0.DepthBiasSlopeFactor);

			Assert.IsNotNull (transform.DrawItems);
			Assert.AreEqual (1, transform.DrawItems.Count);
			var drawItem_0 = transform.DrawItems [0];
			Assert.AreEqual (0, drawItem_0.DepthBias);

			// NEXT TEST - IF VALUES DIFFER, CREATE NEW DEPTHBIAS
			var command_1 = new GLCmdDrawCommand{ Pipeline = 0, DepthBias = null, Draw = new GLCmdInternalDraw{ }  };

			result = transform.InitialiseDrawItem (repo, pass, command_1);
			Assert.IsTrue (result);
			Assert.AreEqual (2, transform.DepthBias.Count);

			var actualValues_1 = transform.DepthBias [1];
			Assert.AreEqual (DEFAULT_CLAMP, actualValues_1.DepthBiasClamp);
			Assert.AreEqual (DEFAULT_CONSTANT_FACTOR, actualValues_1.DepthBiasConstantFactor);
			Assert.AreEqual (DEFAULT_SLOPE, actualValues_1.DepthBiasSlopeFactor);

			Assert.AreEqual (2, transform.DrawItems.Count);

			var drawItem_1 = transform.DrawItems [1];
			Assert.AreEqual (1, drawItem_1.DepthBias);

			// NEXT TEST - IF DEPTHBIAS IS SAME, REUSE INDEX 1
			var command_2 = new GLCmdDrawCommand{ Pipeline = 0, DepthBias = null, Draw = new GLCmdInternalDraw{ }  };

			result = transform.InitialiseDrawItem (repo, pass, command_2);
			Assert.IsTrue (result);
			Assert.AreEqual (2, transform.DepthBias.Count);

			Assert.AreEqual (3, transform.DrawItems.Count);

			var drawItem_2 = transform.DrawItems [2];
			var index = drawItem_2.DepthBias;
			Assert.AreEqual (1, index);

			var actualValues_2 = transform.DepthBias [index];
			Assert.AreEqual (DEFAULT_CLAMP, actualValues_2.DepthBiasClamp);
			Assert.AreEqual (DEFAULT_CONSTANT_FACTOR, actualValues_2.DepthBiasConstantFactor);
			Assert.AreEqual (DEFAULT_SLOPE, actualValues_2.DepthBiasSlopeFactor);
		}
	}
}

