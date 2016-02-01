using MonoGame.Graphics;

namespace NewFences
{
	public class RenderPass
	{
		public int Id {
			get;
			set;
		}

		public RenderPassGroup[] Groups {
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
