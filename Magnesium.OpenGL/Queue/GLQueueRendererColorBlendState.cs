
namespace Magnesium.OpenGL
{
	public class GLQueueRendererColorBlendState
	{
		public bool LogicOpEnable { get; set; }
		public MgLogicOp LogicOp { get; set; }
		public GLQueueColorAttachmentBlendState[] Attachments { get; set; }
	}
}

