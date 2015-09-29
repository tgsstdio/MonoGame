using BirdNest.MonoGame.Core;

namespace BirdNest.MonoGame.Graphics
{
	public interface IShaderLoader
	{
		ShaderProgramData Load(AssetIdentifier identifier);
	}
}

