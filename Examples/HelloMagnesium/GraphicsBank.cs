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

		public void Destroy(IMgThreadPartition partition)
		{
			if (partition == null)
				return;

			if (PostPresentBarrierCmd != null)
				partition.Device.FreeCommandBuffers(partition.CommandPool, new [] {PostPresentBarrierCmd} );

			if (PrePresentBarrierCmd != null)
				partition.Device.FreeCommandBuffers(partition.CommandPool, new [] {PrePresentBarrierCmd} );

			if (RenderComplete != null)
				RenderComplete.DestroySemaphore (partition.Device, null);

			if (PresentComplete != null)
				PresentComplete.DestroySemaphore (partition.Device, null);

			if (CommandBuffers != null)
				partition.Device.FreeCommandBuffers(partition.CommandPool, CommandBuffers );
		}
	}
}

