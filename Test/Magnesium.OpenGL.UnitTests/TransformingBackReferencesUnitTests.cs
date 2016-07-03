using NUnit.Framework;

namespace Magnesium.OpenGL.UnitTests
{
	[TestFixture]
	public class TransformingBackReferencesUnitTests
	{
		[TestCase]
		public void InitialiseCheck()
		{
			IGLCmdBufferRepository repo = new GLCmdBufferRepository ();
			Assert.AreEqual (0, repo.BackReferences.Count);

			ICmdVBOCapabilities vbo = new MockVertexBufferFactory ();
			var transform = new Transformer (vbo);

			transform.Initialise (repo);

			Assert.IsNotNull (transform.BackReferences);
			Assert.AreEqual (0, transform.BackReferences.Count);
		}

		[TestCase]
		public void NoneFound ()
		{
			IGLCmdBufferRepository repo = new GLCmdBufferRepository ();
			Assert.AreEqual (0, repo.BackReferences.Count);

			ICmdVBOCapabilities vbo = new MockVertexBufferFactory ();
			var transform = new Transformer (vbo);
			transform.Initialise (repo);

			var command = new GLCmdDrawCommand{ BackReference = null };

			var actual = transform.InitialiseDrawItem (repo, command);
			Assert.IsFalse (actual);
			Assert.IsNotNull (transform.BackReferences);
			Assert.AreEqual (0, transform.BackReferences.Count);
		}

		[TestCase]
		public void NoPipeline ()
		{
			IGLCmdBufferRepository repo = new GLCmdBufferRepository ();

			const int OVERRIDE_VALUE = 3;
			repo.BackReferences.Add (OVERRIDE_VALUE);

			Assert.AreEqual (1, repo.BackReferences.Count);

			ICmdVBOCapabilities vbo = new MockVertexBufferFactory ();
			var transform = new Transformer (vbo);
			transform.Initialise (repo);

			var command = new GLCmdDrawCommand{ Pipeline = null, BackReference = 0 };

			var actual = transform.InitialiseDrawItem (repo, command);
			Assert.IsFalse (actual);
			Assert.IsNotNull (transform.BackReferences);
			Assert.AreEqual (0, transform.BackReferences.Count);
		}

		[TestCase]
		public void NoOverrideAllowed ()
		{
			IGLCmdBufferRepository repo = new GLCmdBufferRepository ();

			const int OVERRIDE_VALUE = 5;
			repo.BackReferences.Add(OVERRIDE_VALUE);

			Assert.AreEqual (1, repo.BackReferences.Count);

			var bindings = new GLVertexBufferBinding[]{ };
			var attributes = new GLVertexInputAttribute[]{ };

			const int DEFAULT_VALUE = 100;

			repo.GraphicsPipelines.Add (new MockGLGraphicsPipeline 
				{
					VertexInput = new GLVertexBufferBinder(bindings, attributes),
					DynamicsStates = 0,
					Back = new GLQueueStencilMasks{ Reference = DEFAULT_VALUE},
					Viewports = new GLCmdViewportParameter(0, new MgViewport[]{}),
				}
			);

			Assert.AreEqual (1, repo.GraphicsPipelines.Count);

			ICmdVBOCapabilities vbo = new MockVertexBufferFactory ();
			var transform = new Transformer (vbo);
			transform.Initialise (repo);

			var command = new GLCmdDrawCommand{ Pipeline = 0, BackReference = 0 };

			var result = transform.InitialiseDrawItem (repo, command);
			Assert.IsTrue (result);
			Assert.IsNotNull (transform.BackReferences);
			Assert.AreEqual (1, transform.BackReferences.Count);

			float actual = transform.BackReferences.Items [0];
			Assert.AreEqual (DEFAULT_VALUE, actual);
		}

		[TestCase]
		public void OverrideAllowed ()
		{
			IGLCmdBufferRepository repo = new GLCmdBufferRepository ();

			const int OVERRIDE_VALUE = 5;
			repo.BackReferences.Add(OVERRIDE_VALUE);

			Assert.AreEqual (1, repo.BackReferences.Count);

			var bindings = new GLVertexBufferBinding[]{ };
			var attributes = new GLVertexInputAttribute[]{ };

			const int DEFAULT_VALUE = 100;

			repo.GraphicsPipelines.Add (new MockGLGraphicsPipeline 
				{
					VertexInput = new GLVertexBufferBinder(bindings, attributes),
					DynamicsStates = GLGraphicsPipelineDynamicStateFlagBits.STENCIL_REFERENCE,
					Back = new GLQueueStencilMasks{Reference = DEFAULT_VALUE},
					Viewports = new GLCmdViewportParameter(0, new MgViewport[]{}),
				}
			);

			Assert.AreEqual (1, repo.GraphicsPipelines.Count);

			ICmdVBOCapabilities vbo = new MockVertexBufferFactory ();
			var transform = new Transformer (vbo);
			transform.Initialise (repo);

			// USE OVERRIDE 
			var command_0 = new GLCmdDrawCommand{ Pipeline = 0, BackReference = 0 };

			var result = transform.InitialiseDrawItem (repo, command_0);
			Assert.IsTrue (result);
			Assert.IsNotNull (transform.BackReferences);
			Assert.AreEqual (1, transform.BackReferences.Count);

			float actualValues_0 = transform.BackReferences.Items [0];
			Assert.AreEqual (OVERRIDE_VALUE, actualValues_0);

			Assert.IsNotNull (transform.DrawItems);
			Assert.AreEqual (1, transform.DrawItems.Count);
			var drawItem_0 = transform.DrawItems [0];
			Assert.AreEqual (0, drawItem_0.BackStencilReference);

			// NEXT TEST - IF VALUES DIFFER, CREATE NEW DEPTHBIAS
			var command_1 = new GLCmdDrawCommand{ Pipeline = 0, BackReference = null };

			result = transform.InitialiseDrawItem (repo, command_1);
			Assert.IsTrue (result);
			Assert.AreEqual (2, transform.BackReferences.Count);

			var actualValues_1 = transform.BackReferences.Items [1];
			Assert.AreEqual (DEFAULT_VALUE, actualValues_1);

			Assert.AreEqual (2, transform.DrawItems.Count);

			var drawItem_1 = transform.DrawItems [1];
			Assert.AreEqual (1, drawItem_1.BackStencilReference);

			// NEXT TEST - IF DEPTHBIAS IS SAME, REUSE INDEX 1
			var command_2 = new GLCmdDrawCommand{ Pipeline = 0, BackReference = null };

			result = transform.InitialiseDrawItem (repo, command_2);
			Assert.IsTrue (result);
			Assert.AreEqual (2, transform.BackReferences.Count);

			Assert.AreEqual (3, transform.DrawItems.Count);

			var drawItem_2 = transform.DrawItems [2];
			var index = drawItem_2.BackStencilReference;
			Assert.AreEqual (1, index);

			var actualValues_2 = transform.BackReferences.Items [index];
			Assert.AreEqual (DEFAULT_VALUE, actualValues_2);
		}
	}
}

