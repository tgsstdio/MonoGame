using MonoGame.Content;

namespace MonoGame.Models
{
	public interface IModelLoader
	{
		bool Load(AssetIdentifier modelId);
	}
}

