using BirdNest.MonoGame.Graphics;

namespace BirdNest.Rendering
{
	public class OptimizedPass
	{
		public uint Order { get; set; }
		public InstanceIdentifier Id { get; private set; }
		//public IUniformBinder Uniforms {get;set;}
		public IVertexBuffer VBO { get; set; }
		public ICommandFilter Commands { get; set; }
		public DrawState State {get;set;}
	}
}

