using MonoGame.Content;
using MonoGame.Content.Blocks;

namespace MonoGame.Textures
{
	public interface ITexturePageLookup
	{
		bool TryGetValue (AssetIdentifier identifier, out TexturePageInfo result);
		void Scan(BlockIdentifier identifier, TextureChapterInfo chapter);
	}
}

