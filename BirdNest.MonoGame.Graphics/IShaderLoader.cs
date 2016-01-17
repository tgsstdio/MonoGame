using MonoGame.Content;

namespace BirdNest.MonoGame.Graphics
{
	public interface IShaderLoader
	{
		ShaderProgram Load(AssetIdentifier identifier);
	}
}

