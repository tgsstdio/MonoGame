namespace Magnesium.OpenGL
{
	public interface IGLGraphicsPipeline
	{
		int ProgramID {
			get;
		}

		GLGraphicsPipelineDynamicStateFlagBits DynamicsStates { get; }
		GLQueueRendererBlendState ColorBlends { get; }

		GLCmdViewportParameter Viewports { get; }
		GLCmdScissorParameter Scissors { get; }

		QueueDrawItemBitFlags Flags { get; }

		MgPolygonMode PolygonMode { get; }

		GLQueueStencilMasks Front { get; }
		GLQueueStencilMasks Back { get; }

		MgPrimitiveTopology Topology { get; }

		GLQueueDepthState DepthState {
			get;
		}

		GLVertexBufferBinder VertexInput { get; }

		MgColor4f BlendConstants {
			get;
		}

		float MinDepthBounds {
			get;
		}

		float MaxDepthBounds {
			get;
		}

		float DepthBiasSlopeFactor {
			get;
		}

		float DepthBiasClamp {
			get;
		}

		float DepthBiasConstantFactor {
			get;
		}

		float LineWidth {
			get;
		}

		GLQueueStencilState StencilState {
			get;
		}		
	}
}

