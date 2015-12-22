using System;

namespace StrawHat
{
	public struct DrawItem
	{
		public int ProgramIndex { get; set;}
		public int UniformsIndex { get; set;}
		public int MeshIndex { get; set; }
		public DrawCommand Command {get;set;}
		public RasterizerState RasterizerValues {get;set;}
		public DepthStencilState DepthStencilValues {get;set;}
		public BlendState BlendValues { get; set;}
	}
}

