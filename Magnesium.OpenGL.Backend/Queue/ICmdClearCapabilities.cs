using Magnesium;

namespace Magnesium.OpenGL
{
	public interface ICmdClearCapabilities
	{
		GLQueueRendererClearValueState Initialize ();
		void ClearBuffers (GLQueueClearBufferMask combinedMask);
		void SetClearStencilValue (uint stencil);
		void SetClearDepthValue (float value);
		void SetClearColor (MgColor4f clearValue);
	}
}

