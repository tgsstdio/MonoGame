using BirdNest.MonoGame.Graphics;


namespace BirdNest.Rendering.UnitTests
{
	public interface IFrameDuplicator
	{
		void Add(LayerType layer, IRenderTarget target, IShaderProgram program, OptimizedPass values);
		Frame[] GetFrames();
	}
}

