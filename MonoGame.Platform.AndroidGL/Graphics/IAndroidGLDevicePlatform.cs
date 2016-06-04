namespace MonoGame.Platform.AndroidGL.Graphics
{
	public interface IAndroidGLDevicePlatform
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