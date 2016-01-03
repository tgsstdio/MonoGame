using System;
using System.Runtime.InteropServices;

namespace MonoGame.Graphics
{
	[StructLayout(LayoutKind.Sequential)]
	public struct DrawItem
	{
		public DrawState State { get; set; }		
		public byte SlotIndex { get; set; }
		public byte TargetIndex {get;set;}
		public byte ProgramIndex { get; set;}
		public ushort ShaderOptions { get; set; }
		public byte UniformsIndex { get; set;}
		public byte ResourceListIndex { get; set; }
		public uint ResourceItemIndex { get; set; }

		public uint MeshIndex { get; set; }
		public ushort BufferMask { get; set; }

		public DrawCommand Command {get;set;}
		public RasterizerState RasterizerValues {get;set;}
		public DepthStencilState DepthStencilValues {get;set;}
		public BlendState BlendValues { get; set;}

		public DrawItemBitFlags Flags {get;set;}
	}
}

