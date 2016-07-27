
namespace Magnesium.OpenGL
{
	public interface IGLRenderPass : IMgRenderPass
	{
		GLClearAttachmentType[] AttachmentFormats { get; }
	}

}

