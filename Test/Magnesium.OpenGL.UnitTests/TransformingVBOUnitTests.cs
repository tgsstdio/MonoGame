using System;
using NUnit.Framework;

namespace Magnesium.OpenGL.UnitTests
{
	[TestFixture]
	public class TransformingVBOUnitTests
	{
		[TestCase]
		public void InitialiseCheck()
		{
			IGLCmdBufferRepository repo = new GLCmdBufferRepository ();
			Assert.AreEqual (0, repo.VertexBuffers.Count);

			ICmdVBOCapabilities vbo = new MockVertexBufferFactory ();
			var transform = new Transformer (vbo, repo);

			Assert.IsNotNull (transform.VBOs);
			Assert.AreEqual (1, transform.VBOs.Count);
		}

		[TestCase]
		public void NoneFound ()
		{
			IGLCmdBufferRepository repo = new GLCmdBufferRepository ();
			Assert.AreEqual (0, repo.VertexBuffers.Count);

			ICmdVBOCapabilities vbo = new MockVertexBufferFactory ();
			var transform = new Transformer (vbo, repo);

			var command = new GLCmdDrawCommand{ VertexBuffer = null, Draw = new GLCmdInternalDraw{ }  };

			var actual = transform.ExtractVertexBuffer (repo, command);
			Assert.AreEqual (0, actual);
		}

		[TestCase]
		public void TwoFoundInitCheck ()
		{
			IGLCmdBufferRepository repo = new GLCmdBufferRepository ();

			repo.VertexBuffers.Add (new GLCmdVertexBufferParameter {
				firstBinding = 0,
				pBuffers = new IMgBuffer[]{},
				pOffsets = new ulong[]{},
			});

			repo.VertexBuffers.Add (new GLCmdVertexBufferParameter {
				firstBinding = 1,
				pBuffers = new IMgBuffer[]{},
				pOffsets = new ulong[]{},
			});

			Assert.AreEqual (2, repo.VertexBuffers.Count);

			ICmdVBOCapabilities vbo = new MockVertexBufferFactory ();
			var transform = new Transformer (vbo, repo);
			Assert.AreEqual (1, transform.VBOs.Count);
		}

		[TestCase]
		public void TwoFoundNoPipeline ()
		{
			IGLCmdBufferRepository repo = new GLCmdBufferRepository ();

			repo.VertexBuffers.Add (new GLCmdVertexBufferParameter {
				firstBinding = 0,
				pBuffers = new IMgBuffer[]{},
				pOffsets = new ulong[]{},
			});

			repo.VertexBuffers.Add (new GLCmdVertexBufferParameter {
				firstBinding = 1,
				pBuffers = new IMgBuffer[]{},
				pOffsets = new ulong[]{},
			});

			Assert.AreEqual (2, repo.VertexBuffers.Count);

			ICmdVBOCapabilities vbo = new MockVertexBufferFactory ();
			var transform = new Transformer (vbo, repo);

			var command = new GLCmdDrawCommand{ VertexBuffer = 1, Draw = new GLCmdInternalDraw{ }  };

			var actual = transform.ExtractVertexBuffer (repo, command);
			Assert.AreEqual (0, actual);
			Assert.AreEqual (1, transform.VBOs.Count);
		}

		[TestCase]
		public void TwoFoundAndPipeline ()
		{
			IGLCmdBufferRepository repo = new GLCmdBufferRepository ();

			repo.VertexBuffers.Add (new GLCmdVertexBufferParameter {
				firstBinding = 0,
				pBuffers = new IMgBuffer[]{},
				pOffsets = new ulong[]{},
			});

			repo.VertexBuffers.Add (new GLCmdVertexBufferParameter {
				firstBinding = 1,
				pBuffers = new IMgBuffer[]{},
				pOffsets = new ulong[]{},
			});

			var bindings = new GLVertexBufferBinding[]{ };
			var attributes = new GLVertexInputAttribute[]{ };
			repo.GraphicsPipelines.Add (new MockGLGraphicsPipeline 
				{
					VertexInput = new GLVertexBufferBinder(bindings, attributes),
				}
			);

			Assert.AreEqual (1, repo.GraphicsPipelines.Count);
			Assert.AreEqual (2, repo.VertexBuffers.Count);

			var vbo = new MockVertexBufferFactory ();
			var transform = new Transformer (vbo, repo);

			var command_0 = new GLCmdDrawCommand{ VertexBuffer = 1, Pipeline = 0 , Draw = new GLCmdInternalDraw{ } };

			const int BUFFER_0 = 100;
			vbo.Index = BUFFER_0;
			var actual_0 = transform.ExtractVertexBuffer (repo, command_0);
			Assert.AreEqual (BUFFER_0, actual_0);
			Assert.AreEqual (2, transform.VBOs.Count);

			var command_1 = new GLCmdDrawCommand{ VertexBuffer = 1, Pipeline = 0, Draw = new GLCmdInternalDraw{ }  };

			const int BUFFER_1 = 200;
			vbo.Index = BUFFER_1;
			var actual_1 = transform.ExtractVertexBuffer (repo, command_1);
			Assert.AreEqual (BUFFER_0, actual_1);
			Assert.AreEqual (2, transform.VBOs.Count);

			var command_2 = new GLCmdDrawCommand{ VertexBuffer = 0, Pipeline = 0, Draw = new GLCmdInternalDraw{ }  };

			var actual_2 = transform.ExtractVertexBuffer (repo, command_2);
			Assert.AreEqual (BUFFER_1, actual_2);
			Assert.AreEqual (3, transform.VBOs.Count);
		}
	}
}

