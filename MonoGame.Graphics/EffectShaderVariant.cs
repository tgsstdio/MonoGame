namespace MonoGame.Graphics
{
	public class EffectShaderVariant
	{		
		public ShaderProgram Program {get;set;}
		public VertexLayout Layout {get;set;}
		public ushort Options { get; set;}
		public RenderPass Destination { get; set; }
	}
}

