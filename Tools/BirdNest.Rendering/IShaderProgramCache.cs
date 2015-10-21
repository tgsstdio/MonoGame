using BirdNest.MonoGame.Core;

namespace BirdNest.Rendering
{
	public interface IShaderProgramCache
	{
		bool TryGetValue(AssetIdentifier id, out IShaderProgram result);		
	}
}

