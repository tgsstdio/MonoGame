using Microsoft.Xna.Framework.Graphics;

namespace Microsoft.Xna.Framework
{
	public interface IRenderTarget2DFactory
	{
		IRenderTarget2D Construct (
			int width, int height,
			bool mipMap, SurfaceFormat preferredFormat,
			DepthFormat preferredDepthFormat,
			int preferredMultiSampleCount, RenderTargetUsage usage,
			bool shared, int arraySize);
		
		IRenderTarget2D Construct (int width, int height,
			bool mipMap, SurfaceFormat preferredFormat,
			DepthFormat preferredDepthFormat, int preferredMultiSampleCount,
			RenderTargetUsage usage, bool shared);
		
		IRenderTarget2D Construct (int width, int height,
			bool mipMap, SurfaceFormat preferredFormat,
			DepthFormat preferredDepthFormat, int preferredMultiSampleCount,
			RenderTargetUsage usage);
		
		IRenderTarget2D Construct (int width, int height,
			bool mipMap, SurfaceFormat preferredFormat,
			DepthFormat preferredDepthFormat);
		
		IRenderTarget2D Construct (int width, int height);
	}
}

