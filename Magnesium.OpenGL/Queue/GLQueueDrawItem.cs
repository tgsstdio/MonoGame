using System.Runtime.InteropServices;

namespace Magnesium.OpenGL
{
	[StructLayout(LayoutKind.Sequential)]
	public struct GLQueueDrawItem
	{
		public MgPolygonMode Mode { get; set; }			
		public DrawState State { get; set; }
		public byte SlotIndex { get; set; }
		public byte TargetIndex {get;set;}
		public ushort ProgramIndex { get; set;}
		public byte UniformsIndex { get; set;}
		public MgPrimitiveTopology Primitive {get;set;}

		public ushort BufferMask { get; set; }
		public ushort ShaderOptions { get; set; }

		public uint ResourceIndex { get; set; }

		public uint BindingSet { get; set; }
		public uint MarkerIndex { get; set; }

		public GLQueueRasterizerState RasterizerValues {get;set;}
		public GLQueueStencilState StencilValues {get;set;}
		public GLQueueBlendState BlendValues { get; set;}
		public GLQueueDepthState DepthValues {get;set;}

		public QueueDrawItemBitFlags Flags {get;set;}
	}
}

