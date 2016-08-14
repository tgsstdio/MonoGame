using OpenTK.Graphics;

namespace Magnesium.OpenGL.DesktopGL
{
	public interface IOpenTKSwapchainKHR : IGLSwapchainKHR, IMgSwapchainKHR
	{
		void Initialize (IGraphicsContext context, uint maxNoOfImages);
	}
}

