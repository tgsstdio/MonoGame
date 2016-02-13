using MonoGame.Graphics;

namespace NewFences
{
	public class RenderPass : IRenderPass
	{
		public int Id {
			get;
			set;
		}

		public IRenderPassGroup[] Groups {
			get;
			set;
		}

		public IGPUSyncObject[] Requirements { get; set;}

		public IPassSyncObject Fence {
			get;
			set;
		}
	}

}
