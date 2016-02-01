using MonoGame.Shaders;

namespace BirdNest.Rendering
{
	public interface IUniformBinder
	{
		InstanceIdentifier Id { get; }
		void Bind();
	}
}

