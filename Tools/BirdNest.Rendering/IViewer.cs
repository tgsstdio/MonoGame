using BirdNest.MonoGame.Graphics;

namespace BirdNest.Rendering
{
	public interface IViewer
	{
		InstanceIdentifier Id { get; }
		LayerType GetLayerType();
	}
}

