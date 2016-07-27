using OpenTK.Graphics;

namespace Magnesium.OpenGL
{
	public interface IOpenTKSwapchainKHR : IMgSwapchainKHR
	{
		void Initialise (IGraphicsContext context, uint maxNoOfImages);
		uint GetNextImage ();
		void SwapBuffers();
	}
}

