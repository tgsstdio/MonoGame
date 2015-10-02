using BirdNest.MonoGame.Core;
using BirdNest.MonoGame.Blocks;

namespace BirdNest.MonoGame.Graphics
{
	public interface IShaderInfoLookup
	{
		bool TryGetValue (AssetIdentifier identifier, out ShaderInfo result);
		void Scan(BlockIdentifier identifier, ShaderInfo shader);		
	}
}

