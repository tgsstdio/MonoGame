using BirdNest.MonoGame.Core;
using BirdNest.MonoGame.Blocks;

namespace BirdNest.MonoGame.Graphics
{
	public interface ITexturePageLookup
	{
		bool TryGetValue (AssetIdentifier identifier, out TexturePageInfo result);
		void Scan(BlockIdentifier identifier, TextureChapterInfo chapter);
	}
}

