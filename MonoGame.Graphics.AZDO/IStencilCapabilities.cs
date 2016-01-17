namespace MonoGame.Graphics.AZDO
{
	public interface IStencilCapabilities
	{
		void Initialise ();
		void EnableStencilBuffer();
		void DisableStencilBuffer();
		bool IsStencilBufferEnabled { get; }
		void SetStencilWriteMask(int mask);

		void SetFrontFaceCullStencilFunction (CompareFunction func, int referenceStencil, int stencilMask);
		void SetBackFaceCullStencilFunction(CompareFunction func, int referenceStencil, int stencilMask);

		void SetFrontFaceStencilOperation(
			StencilOperation stencilFail,
			StencilOperation stencilDepthBufferFail,
			StencilOperation stencilPass);

		void SetBackFaceStencilOperation(
			StencilOperation stencilFail,
			StencilOperation stencilDepthBufferFail,
			StencilOperation stencilPass);

		void SetStencilFunction(
			CompareFunction stencilFunction,
			int referenceStencil,
			int stencilMask);

		void SetStencilOperation(
			StencilOperation stencilFail,
			StencilOperation stencilDepthBufferFail,
			StencilOperation stencilPass);
	}
}

