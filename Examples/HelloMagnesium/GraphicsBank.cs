using System;
using Magnesium;

namespace HelloMagnesium
{
	public class GraphicsBank
	{
		public GraphRenderer Renderer {
			get;
			set;
		}

		public IMgCommandBuffer PostPresentBarrierCmd {
			get;
			set;
		}

		public IMgCommandBuffer PrePresentBarrierCmd {
			get;
			set;
		}

		public IMgSemaphore RenderComplete {
			get;
			set;
		}

		public IMgSemaphore PresentComplete {
			get;
			set;
		}

		public IMgCommandBuffer[] CommandBuffers {
			get;
			set;
		}

//		public IMgPipeline[] GraphicsPipelines {
//			get;
//			set;
//		}

//		public IMgPipelineLayout PipelineLayout {
//			get;
//			set;
//		}
//
//		public IMgDescriptorSetLayout SetLayout {
//			get;
//			set;
//		}

		public uint Height {
			get;
			set;
		}

		public uint Width {
			get;
			set;
		}

//		public MgRect2D CurrentScissor {
//			get;
//			set;
//		}
//
//		public MgViewport CurrentViewport {
//			get;
//			set;
//		}
	}
}

