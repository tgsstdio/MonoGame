using System.Runtime.InteropServices;

namespace Magnesium.OpenGL
{
	[StructLayout(LayoutKind.Sequential)]
	public struct GLQueueRendererStencilState
	{
		public QueueDrawItemBitFlags Flags { get; set; }
		public GLQueueStencilState Enums { get; set; }
		public GLQueueStencilMasks Front { get; set; }
		public GLQueueStencilMasks Back  { get; set; }
	}
}

