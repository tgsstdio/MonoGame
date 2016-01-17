using MonoGame.Content;

namespace BirdNest.Rendering
{
	public interface IShaderProgram
	{
		AssetIdentifier Id {get;}
		LayerType Layer { get; }
		void Use();
		void Unuse();
	}
}

