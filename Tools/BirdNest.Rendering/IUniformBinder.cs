using BirdNest.MonoGame.Graphics;

namespace BirdNest.Rendering
{
	public interface IUniformBinder
	{
		InstanceIdentifier Id { get; }
		void Bind();
	}
}

