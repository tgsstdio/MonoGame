using System.Runtime.InteropServices;

namespace Magnesium.OpenGL
{
	[StructLayout(LayoutKind.Sequential)]
	public struct GLCmdBufferPipelineItem
	{
		public QueueDrawItemBitFlags Flags {get;set;}

		public GLQueueDepthState DepthState { get; set;	}

		public GLQueueStencilState StencilState { get; set; }

		public byte Pass { get; set; }

		public byte Pipeline { get; set; }

		public byte Rasterization { get; set; }

		public byte BlendEnums { get; set; }
	}
}

