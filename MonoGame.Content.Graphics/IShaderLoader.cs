using MonoGame.Content;

namespace MonoGame.Shaders
{
	public interface IShaderLoader
	{
		ShaderProgram Load(AssetIdentifier identifier);
	}
}

