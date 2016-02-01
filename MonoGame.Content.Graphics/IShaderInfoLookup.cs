using MonoGame.Content;
using MonoGame.Content.Blocks;

namespace MonoGame.Shaders
{
	public interface IShaderInfoLookup
	{
		bool TryGetValue (AssetIdentifier identifier, out ShaderInfo result);
		void Scan(BlockIdentifier identifier, ShaderInfo shader);		
	}
}

