namespace MonoGame.Platform.DesktopGL
{
	public interface IGLDevicePlatform
	{
		void Initialise();
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

