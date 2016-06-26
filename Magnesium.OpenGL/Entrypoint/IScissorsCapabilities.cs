namespace Magnesium.OpenGL
{
	public interface IScissorsCapabilities
	{
		void ApplyViewports (GLCmdViewportParameter viewports);

		void ApplyScissors(GLCmdScissorParameter scissors);
	}
}

