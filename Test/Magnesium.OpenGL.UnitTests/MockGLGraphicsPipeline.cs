namespace Magnesium.OpenGL.UnitTests
{
	public class MockGLGraphicsPipeline : IMgPipeline, IGLGraphicsPipeline
	{
		#region IMgPipeline implementation

		public void DestroyPipeline (IMgDevice device, MgAllocationCallbacks allocator)
		{
			throw new System.NotImplementedException ();
		}

		#endregion

		#region IGLGraphicsPipeline implementation

		public GLGraphicsPipelineBlendColorState ColorBlendEnums {
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

		public GLGraphicsPipelineFlagBits Flags {
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

		public GLGraphicsPipelineStencilMasks Front {
			get;
			set;
		}

		public GLGraphicsPipelineStencilMasks Back {
			get;
			set;
		}

		public GLGraphicsPipelineDepthState DepthState {
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
		public GLGraphicsPipelineStencilState StencilState {
			get;
			set;
		}
		#endregion

	}
}

