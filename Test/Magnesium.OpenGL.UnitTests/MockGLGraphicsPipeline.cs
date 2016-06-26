namespace Magnesium.OpenGL.UnitTests
{
	public class MockGLGraphicsPipeline : IGLGraphicsPipeline
	{
		#region IGLGraphicsPipeline implementation

		public GLQueueRendererBlendState ColorBlends {
			get;
			set;
		}

		public GLGraphicsPipelineDynamicStateFlagBits DynamicsStates {
			get;
			set;
		}

		public GLCmdViewportParameter Viewports {
			get;
			set;
		}

		public GLCmdScissorParameter Scissors {
			get;
			set;
		}


		public int ProgramID {
			get;
			set;
		}

		public QueueDrawItemBitFlags Flags {
			get;
			set;
		}

		public MgPolygonMode PolygonMode {
			get;
			set;
		}

		public MgPrimitiveTopology Topology {
			get;
			set;
		}

		public GLQueueStencilMasks Front {
			get;
			set;
		}

		public GLQueueStencilMasks Back {
			get;
			set;
		}

		public GLQueueDepthState DepthState {
			get;
			set;
		}

		public GLVertexBufferBinder VertexInput {
			get;
			set;
		}

		public MgColor4f BlendConstants {
			get;
			set;
		}

		public float MinDepthBounds {
			get;
			set;
		}
		public float MaxDepthBounds {
			get;
			set;
		}
		public float DepthBiasSlopeFactor {
			get;
			set;
		}
		public float DepthBiasClamp {
			get;
			set;
		}
		public float DepthBiasConstantFactor {
			get;
			set;
		}
		public float LineWidth {
			get;
			set;
		}
		public GLQueueStencilState StencilState {
			get;
			set;
		}
		#endregion

	}
}

