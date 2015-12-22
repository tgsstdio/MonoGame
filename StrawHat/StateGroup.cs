using System;

namespace StrawHat
{
	public class StateGroup
	{
		public ShaderTechnique Technique { get; set;}
		public ShaderProgram Program {get;set;}
		public byte? PassNumber {get;set;}
		public int? ShaderOptions { get; set;}
		public int? UniformsIndex { get; set;}
		public InputLayout MeshLayout { get; set;}
		public int? MeshIndex {get;set;}
		public BlendState? BlendValues {get;set;}
		public RasterizerState? RasterizerValues {get;set;}
		public DepthStencilState? DepthStencilValues {get;set;}
		public ConstantBuffer[] Buffers { get; set;}
		public ResourceList[] Resources {get;set;}
		public Sampler[] Samplers {get;set;}
		public ImageTarget[] Targets {get;set;}
	}
}

