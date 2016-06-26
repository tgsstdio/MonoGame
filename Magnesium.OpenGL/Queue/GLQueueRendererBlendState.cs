
namespace Magnesium.OpenGL
{
	public class GLQueueRendererBlendState
	{
		public bool LogicOpEnable { get; set; }
		public MgLogicOp LogicOp { get; set; }
		public GLQueueColorAttachmentBlendState[] Attachments { get; set; }
	}
}

