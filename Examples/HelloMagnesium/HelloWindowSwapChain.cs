using Magnesium;

namespace HelloMagnesium
{
	public class HelloWindowSwapChain : IMgSwapchainCollection
	{
		public HelloWindowSwapChain ()
		{
			Buffers = new MgSwapchainBuffer[]{ };
		}

		#region IMgSwapchain implementation

		public IMgSwapchainKHR Swapchain {
			get {
				throw new System.NotImplementedException ();
			}
		}

		public uint Width {
			get {
				throw new System.NotImplementedException ();
			}
		}

		public uint Height {
			get {
				throw new System.NotImplementedException ();
			}
		}

		public void Setup ()
		{

		}

		public void Create (IMgCommandBuffer cmd, uint width, uint height)
		{

		}

		public MgSwapchainBuffer[] Buffers {
			get;
			private set;
		}

		#endregion

		#region IDisposable implementation

		public void Dispose ()
		{

		}

		#endregion
	}
}

