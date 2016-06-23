using System;
using NUnit.Framework;

namespace Magnesium.OpenGL.UnitTests
{
	[TestFixture]
	public class CmdBufferRepoUnitTests
	{
		[TestCase]
		public void CopyPipelineValues()
		{			
			var repository = new GLCmdBufferRepository ();

			var pipeline = new MockGLGraphicsPipeline ();

			pipeline.BlendConstants = new MgColor4f (0.5f, 0.25f, 0.33f, 1f);

			pipeline.DepthBiasClamp = 1f;
			pipeline.DepthBiasConstantFactor = 3f;
			pipeline.DepthBiasSlopeFactor = 5f;

			pipeline.LineWidth = 7f;

			pipeline.MinDepthBounds = 11f;
			pipeline.MaxDepthBounds = 13f;

			var front = new GLQueueStencilMasks {
				Reference = 17,
				WriteMask = 19,
				CompareMask = 23,
			};

			pipeline.Front = front;;

			var back = new GLQueueStencilMasks {
				Reference = 29,
				WriteMask = 31,
				CompareMask = 37,
			};

			pipeline.Back = back;

			repository.PushGraphicsPipeline (pipeline);

			Assert.AreEqual (1, repository.GraphicsPipelines.Count);

			{
				var store = repository.DepthBias;
				Assert.AreEqual (1, store.Count);
				Assert.AreEqual (0, store.LastIndex ());

				var actual = store.At (0);
				Assert.IsNotNull (actual);
				Assert.AreEqual (pipeline.DepthBiasClamp, actual.DepthBiasClamp);
				Assert.AreEqual (pipeline.DepthBiasConstantFactor, actual.DepthBiasConstantFactor);
				Assert.AreEqual (pipeline.DepthBiasSlopeFactor, actual.DepthBiasSlopeFactor);
			}

			{
				var store = repository.DepthBounds;
				Assert.AreEqual (1, store.Count);
				Assert.AreEqual (0, store.LastIndex ());
				var actual = store.At (0);

				Assert.AreEqual (pipeline.MinDepthBounds, actual.MinDepthBounds);
				Assert.AreEqual (pipeline.MaxDepthBounds, actual.MaxDepthBounds);
			}

			{
				var store = repository.BlendConstants;
				Assert.AreEqual (1, store.Count);
				Assert.AreEqual (0, store.LastIndex ());
				var actual = store.At (0);
				var expected = pipeline.BlendConstants;
				Assert.IsNotNull (actual);
				Assert.AreEqual (expected, actual);


				Assert.AreEqual (expected, actual);
			}

			{
				var store = repository.BackCompareMasks;
				Assert.AreEqual (1, store.Count);
				Assert.AreEqual (0, store.LastIndex ());
				var actual = store.At (0);
				var expected = back.CompareMask;
				Assert.AreEqual (expected, actual);
			}

			{
				var store = repository.BackReferences;
				Assert.AreEqual (1, store.Count);
				Assert.AreEqual (0, store.LastIndex ());
				var actual = store.At (0);
				var expected = back.Reference;
				Assert.AreEqual (expected, actual);
			}

			{
				var store = repository.BackWriteMasks;
				Assert.AreEqual (1, store.Count);
				Assert.AreEqual (0, store.LastIndex ());
				var actual = store.At (0);
				var expected = back.WriteMask;
				Assert.AreEqual (expected, actual);
			}

			{
				var store = repository.FrontWriteMasks;
				Assert.AreEqual (1, store.Count);
				Assert.AreEqual (0, store.LastIndex ());
				var actual = store.At (0);
				var expected = front.WriteMask;
				Assert.AreEqual (expected, actual);
			}

			{
				var store = repository.FrontReferences;
				Assert.AreEqual (1, store.Count);
				Assert.AreEqual (0, store.LastIndex ());
				var actual = store.At (0);
				var expected = front.Reference;
				Assert.AreEqual (expected, actual);
			}

			{
				var store = repository.FrontCompareMasks;
				Assert.AreEqual (1, store.Count);
				Assert.AreEqual (0, store.LastIndex ());
				var actual = store.At (0);
				var expected = front.CompareMask;
				Assert.AreEqual (expected, actual);
			}

			Assert.AreEqual (1, repository.LineWidths.Count);
			var lineWidths = repository.LineWidths.At (0);
			Assert.AreEqual (pipeline.LineWidth, lineWidths);
		}
	}
}

