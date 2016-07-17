using Magnesium;

namespace Magnesium.OpenGL
{
	public class GLSwapchainKHR : IMgSwapchainKHR
	{
		public GLSwapchainKHR (uint maxNoOfImages)
		{
			Index = maxNoOfImages - 1;
			MaxNoOfImages = maxNoOfImages;
		}
		public uint Index { get; private set; }
		public uint MaxNoOfImages {	get; private set; }

		public uint GetNextImage()
		{
			Index = (Index + 1) % MaxNoOfImages;
			return Index;
		}

		#region IMgSwapchainKHR implementation
		public void DestroySwapchainKHR (IMgDevice device, MgAllocationCallbacks allocator)
		{

		}
		#endregion
	}

}

