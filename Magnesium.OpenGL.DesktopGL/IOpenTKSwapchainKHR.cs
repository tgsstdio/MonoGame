using OpenTK.Graphics;

namespace Magnesium.OpenGL
{
	public interface IOpenTKSwapchainKHR : IGLSwapchainKHR, IMgSwapchainKHR
	{
		void Initialize (IGraphicsContext context, uint maxNoOfImages);
	}
}

