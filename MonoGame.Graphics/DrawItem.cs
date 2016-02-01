using System.Runtime.InteropServices;

namespace MonoGame.Graphics
{
	[StructLayout(LayoutKind.Sequential)]
	public struct DrawItem
	{
		public DrawMode Mode { get; set; }			
		public DrawState State { get; set; }
		public byte SlotIndex { get; set; }
		public byte TargetIndex {get;set;}
		public ushort ProgramIndex { get; set;}
		public byte UniformsIndex { get; set;}
		public DrawPrimitive Primitive {get;set;}

		public ushort BufferMask { get; set; }
		public ushort ShaderOptions { get; set; }

		public uint ResourceIndex { get; set; }

		public uint BindingSet { get; set; }
		public uint MarkerIndex { get; set; }

		public RasterizerState RasterizerValues {get;set;}
		public StencilState StencilValues {get;set;}
		public BlendState BlendValues { get; set;}
		public DepthState DepthValues {get;set;}

		public DrawItemBitFlags Flags {get;set;}
	}
}

