using MonoGame.Graphics;

namespace NewFences
{
	public interface IRenderPass
	{
		int Id {
			get;
			set;
		}

		IRenderPassGroup[] Groups {
			get;
			set;
		}

		IGPUSyncObject[] Requirements { get; set;}

		IPassSyncObject Fence {
			get;
			set;
		}		
	}
}
