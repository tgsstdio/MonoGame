namespace Magnesium.OpenGL
{
	public interface IGLDevicePlatform
	{
		void Initialize();
		void AfterApplyRenderTargets(int renderCount);

		int MaxVertexAttributes {
			get;
		}

		int MaxTextureSize {
			get;
		}

		int MaxTextureSlots {
			get;
		}
	}
}