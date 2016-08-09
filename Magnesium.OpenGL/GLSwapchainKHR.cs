using Magnesium;
using OpenTK.Graphics;

namespace Magnesium.OpenGL
{
	public class GLSwapchainKHR : IOpenTKSwapchainKHR
	{
		public uint Index { get; private set; }
		public uint MaxNoOfImages {	get; private set; }

		#region IOpenTKSwapchainKHR implementation

		private IGraphicsContext mContext;
		public void Initialise(IGraphicsContext context, uint maxNoOfImages)
		{
			Index = maxNoOfImages - 1;
			MaxNoOfImages = maxNoOfImages;
			mContext = context;
		}

		public uint GetNextImage()
		{
			Index = (Index + 1) % MaxNoOfImages;
			return Index;
		}

		public void SwapBuffers ()
		{
			if (mContext != null && !mContext.IsDisposed)
				mContext.SwapBuffers ();
		}

		#endregion

		#region IMgSwapchainKHR implementation
		public void DestroySwapchainKHR (IMgDevice device, MgAllocationCallbacks allocator)
		{

		}
		#endregion
	}

}

