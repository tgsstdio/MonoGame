namespace MonoGame.Graphics.AZDO
{
	public interface IDepthStencilCapabilities
	{
		bool IsDepthBufferEnabled { get; }
		void Initialise ();
		void EnableDepthBuffer();
		void DisableDepthBuffer();

		void SetDepthBufferFunc(CompareFunction func);
		void SetDepthMask (bool isMaskOn);

		void EnableStencilBuffer();
		void DisableStencilBuffer();
		bool IsStencilBufferEnabled { get; }

		void SetStencilWriteMask(int mask);

	}
}

