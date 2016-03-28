using System;

namespace MonoGame.Graphics.Vk
{
	public class PipelineColorBlendStateCreateInfo
	{
		public UInt32 Flags { get; set; }
		public bool LogicOpEnable { get; set; }
		public LogicOp LogicOp { get; set; }
		public PipelineColorBlendAttachmentState[] Attachments { get; set; }
		public Vec4f BlendConstants { get; set; } // 4
	}
}

