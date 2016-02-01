using MonoGame.Shaders;


namespace BirdNest.Rendering
{
	public interface IViewer
	{
		InstanceIdentifier Id { get; }
		LayerType GetLayerType();
	}
}

