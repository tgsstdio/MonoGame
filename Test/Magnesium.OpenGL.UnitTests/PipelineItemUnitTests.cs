using NUnit.Framework;

namespace Magnesium.OpenGL.UnitTests
{
	[TestFixture]
	public class PipelineItemUnitTests
	{
		ICmdVBOCapabilities mFactory;
		CmdBufferInstructionSetComposer mComposer;

		[SetUp]
		public void Initialise()
		{
			mFactory = new MockVertexBufferFactory ();
			mComposer = new CmdBufferInstructionSetComposer (mFactory);
		}

		[TestCase]
		public void CopyValues()
		{
			var pipeline = new MockGLGraphicsPipeline ();

			pipeline.Flags = QueueDrawItemBitFlags.AlphaColorWriteChannel | QueueDrawItemBitFlags.CullFrontFaces;

			pipeline.DepthState = new GLQueueDepthState{ DepthBufferFunction = MgCompareOp.GREATER };
			pipeline.StencilState = new GLQueueStencilState{
				BackDepthBufferFail = MgStencilOp.DECREMENT_AND_CLAMP,
				BackStencilFail = MgStencilOp.DECREMENT_AND_WRAP,
				BackStencilPass = MgStencilOp.INVERT,
				BackStencilFunction = MgCompareOp.NOT_EQUAL,

				FrontDepthBufferFail = MgStencilOp.INCREMENT_AND_WRAP,
				FrontStencilFail =  MgStencilOp.KEEP,
				FrontStencilPass = MgStencilOp.REPLACE,
				FrontStencilFunction = MgCompareOp.GREATER_OR_EQUAL, 
			};

			var actual = mComposer.GeneratePipelineItem (pipeline);
			Assert.IsNotNull (actual);

			Assert.AreEqual (pipeline.Flags, actual.Flags);

			// DEPTH 
			Assert.AreEqual (pipeline.DepthState.DepthBufferFunction, actual.DepthState.DepthBufferFunction);
			// DEPTH.Equals
			Assert.AreEqual (pipeline.DepthState, actual.DepthState);

			// STENCIL
			Assert.AreEqual (pipeline.StencilState.BackDepthBufferFail, actual.StencilState.BackDepthBufferFail);
			Assert.AreEqual (pipeline.StencilState.BackStencilFail, actual.StencilState.BackStencilFail);
			Assert.AreEqual (pipeline.StencilState.BackStencilPass, actual.StencilState.BackStencilPass);
			Assert.AreEqual (pipeline.StencilState.BackStencilFunction, actual.StencilState.BackStencilFunction);

			Assert.AreEqual (pipeline.StencilState.FrontDepthBufferFail, actual.StencilState.FrontDepthBufferFail);
			Assert.AreEqual (pipeline.StencilState.FrontStencilFail, actual.StencilState.FrontStencilFail);
			Assert.AreEqual (pipeline.StencilState.FrontStencilPass, actual.StencilState.FrontStencilPass);
			Assert.AreEqual (pipeline.StencilState.FrontStencilFunction, actual.StencilState.FrontStencilFunction);

			// STENCIL.Equals
			Assert.AreEqual (pipeline.StencilState, actual.StencilState);
		}

		[TearDown]
		public void Cleanup()
		{
			mFactory = null;
			mComposer = null;
		}
	}
}

