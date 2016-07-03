using NUnit.Framework;

namespace Magnesium.OpenGL.UnitTests
{
	[TestFixture]
	public class TransformingLineWidthsUnitTests
	{
		[TestCase]
		public void InitialiseCheck()
		{
			IGLCmdBufferRepository repo = new GLCmdBufferRepository ();
			Assert.AreEqual (0, repo.LineWidths.Count);

			ICmdVBOCapabilities vbo = new MockVertexBufferFactory ();
			var transform = new Transformer (vbo);

			transform.Initialise (repo);

			Assert.IsNotNull (transform.LineWidths);
			Assert.AreEqual (0, transform.LineWidths.Count);
		}

		[TestCase]
		public void NoneFound ()
		{
			IGLCmdBufferRepository repo = new GLCmdBufferRepository ();
			Assert.AreEqual (0, repo.LineWidths.Count);

			ICmdVBOCapabilities vbo = new MockVertexBufferFactory ();
			var transform = new Transformer (vbo);
			transform.Initialise (repo);

			var command = new GLCmdDrawCommand{ LineWidth = null };

			var actual = transform.InitialiseDrawItem (repo, command);
			Assert.IsFalse (actual);
			Assert.IsNotNull (transform.LineWidths);
			Assert.AreEqual (0, transform.LineWidths.Count);
		}

		[TestCase]
		public void NoPipeline ()
		{
			IGLCmdBufferRepository repo = new GLCmdBufferRepository ();

			const float LINE_WIDTH = 3f;
			repo.LineWidths.Add (LINE_WIDTH);

			Assert.AreEqual (1, repo.LineWidths.Count);

			ICmdVBOCapabilities vbo = new MockVertexBufferFactory ();
			var transform = new Transformer (vbo);
			transform.Initialise (repo);

			var command = new GLCmdDrawCommand{ Pipeline = null, LineWidth = 0 };

			var actual = transform.InitialiseDrawItem (repo, command);
			Assert.IsFalse (actual);
			Assert.IsNotNull (transform.LineWidths);
			Assert.AreEqual (0, transform.LineWidths.Count);
		}

		[TestCase]
		public void NoOverrideAllowed ()
		{
			IGLCmdBufferRepository repo = new GLCmdBufferRepository ();

			const float OVERRIDE_LINEWIDTH = 5f;
			repo.LineWidths.Add(OVERRIDE_LINEWIDTH);

			Assert.AreEqual (1, repo.LineWidths.Count);

			var bindings = new GLVertexBufferBinding[]{ };
			var attributes = new GLVertexInputAttribute[]{ };

			const float DEFAULT_LINEWIDTH = 100f;

			repo.GraphicsPipelines.Add (new MockGLGraphicsPipeline 
				{
					VertexInput = new GLVertexBufferBinder(bindings, attributes),
					DynamicsStates = 0,
					LineWidth = DEFAULT_LINEWIDTH,
					Viewports = new GLCmdViewportParameter(0, new MgViewport[]{}),
				}
			);

			Assert.AreEqual (1, repo.GraphicsPipelines.Count);

			ICmdVBOCapabilities vbo = new MockVertexBufferFactory ();
			var transform = new Transformer (vbo);
			transform.Initialise (repo);

			var command = new GLCmdDrawCommand{ Pipeline = 0, LineWidth = 0 };

			var result = transform.InitialiseDrawItem (repo, command);
			Assert.IsTrue (result);
			Assert.IsNotNull (transform.LineWidths);
			Assert.AreEqual (1, transform.LineWidths.Count);

			float actual = transform.LineWidths.Items [0];
			Assert.AreEqual (DEFAULT_LINEWIDTH, actual);
		}

		[TestCase]
		public void OverrideAllowed ()
		{
			IGLCmdBufferRepository repo = new GLCmdBufferRepository ();

			const float OVERRIDE_LINEWIDTH = 5f;
			repo.LineWidths.Add(OVERRIDE_LINEWIDTH);

			Assert.AreEqual (1, repo.LineWidths.Count);

			var bindings = new GLVertexBufferBinding[]{ };
			var attributes = new GLVertexInputAttribute[]{ };

			const float DEFAULT_LINEWIDTH = 100f;

			repo.GraphicsPipelines.Add (new MockGLGraphicsPipeline 
				{
					VertexInput = new GLVertexBufferBinder(bindings, attributes),
					DynamicsStates = GLGraphicsPipelineDynamicStateFlagBits.LINE_WIDTH,
					LineWidth = DEFAULT_LINEWIDTH,
					Viewports = new GLCmdViewportParameter(0, new MgViewport[]{}),
				}
			);

			Assert.AreEqual (1, repo.GraphicsPipelines.Count);

			ICmdVBOCapabilities vbo = new MockVertexBufferFactory ();
			var transform = new Transformer (vbo);
			transform.Initialise (repo);

			// USE OVERRIDE 
			var command_0 = new GLCmdDrawCommand{ Pipeline = 0, LineWidth = 0 };

			var result = transform.InitialiseDrawItem (repo, command_0);
			Assert.IsTrue (result);
			Assert.IsNotNull (transform.LineWidths);
			Assert.AreEqual (1, transform.LineWidths.Count);

			float actualValues_0 = transform.LineWidths.Items [0];
			Assert.AreEqual (OVERRIDE_LINEWIDTH, actualValues_0);

			Assert.IsNotNull (transform.DrawItems);
			Assert.AreEqual (1, transform.DrawItems.Count);
			var drawItem_0 = transform.DrawItems [0];
			Assert.AreEqual (0, drawItem_0.LineWidth);

			// NEXT TEST - IF VALUES DIFFER, CREATE NEW DEPTHBIAS
			var command_1 = new GLCmdDrawCommand{ Pipeline = 0, LineWidth = null };

			result = transform.InitialiseDrawItem (repo, command_1);
			Assert.IsTrue (result);
			Assert.AreEqual (2, transform.LineWidths.Count);

			float actualValues_1 = transform.LineWidths.Items [1];
			Assert.AreEqual (DEFAULT_LINEWIDTH, actualValues_1);

			Assert.AreEqual (2, transform.DrawItems.Count);

			var drawItem_1 = transform.DrawItems [1];
			Assert.AreEqual (1, drawItem_1.LineWidth);

			// NEXT TEST - IF DEPTHBIAS IS SAME, REUSE INDEX 1
			var command_2 = new GLCmdDrawCommand{ Pipeline = 0, LineWidth = null };

			result = transform.InitialiseDrawItem (repo, command_2);
			Assert.IsTrue (result);
			Assert.AreEqual (2, transform.LineWidths.Count);

			Assert.AreEqual (3, transform.DrawItems.Count);

			var drawItem_2 = transform.DrawItems [2];
			var index = drawItem_2.LineWidth;
			Assert.AreEqual (1, index);

			float actualValues_2 = transform.LineWidths.Items [index];
			Assert.AreEqual (DEFAULT_LINEWIDTH, actualValues_2);
		}
	}
}

