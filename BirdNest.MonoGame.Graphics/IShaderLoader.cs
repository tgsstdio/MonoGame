using BirdNest.MonoGame.Core;

namespace BirdNest.MonoGame.Graphics
{
	public interface IShaderLoader
	{
		ShaderProgram Load(AssetIdentifier identifier);
	}
}

