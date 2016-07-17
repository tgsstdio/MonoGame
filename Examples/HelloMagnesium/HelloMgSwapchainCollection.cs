using Magnesium;
using Magnesium.OpenGL;

namespace HelloMagnesium
{
	public class HelloMgSwapchainCollection : IMgSwapchainCollection
	{
		public HelloMgSwapchainCollection ()
		{
			Buffers = new MgSwapchainBuffer[]{ 
				new MgSwapchainBuffer{
					View = new GLNullImageView(),
				},
				new MgSwapchainBuffer{
					View = new GLNullImageView(),
				}
			};
			Swapchain = new GLSwapchainKHR (2);
		}

		#region IMgSwapchain implementation

		public IMgSwapchainKHR Swapchain {
			get;
			private set;
		}

		public uint Width {
			get;
			private set;
		}

		public uint Height {
			get;
			private set;
		}

		public void Setup ()
		{

		}

		public void Create (IMgCommandBuffer cmd, uint width, uint height)
		{
			Width = width;
			Height = height;
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

