using System;
using MonoGame.Content;
using MonoGame.Content.Blocks;

namespace MonoGame.Textures
{
	public class NullTexturePageLookup : ITexturePageLookup
	{
		public NullTexturePageLookup ()
		{
		}

		#region ITexturePageLookup implementation

		public bool TryGetValue (AssetIdentifier identifier, out TexturePageInfo result)
		{
			throw new NotImplementedException ();
		}

		public void Scan (BlockIdentifier identifier, TextureChapterInfo chapter)
		{
			throw new NotImplementedException ();
		}

		#endregion
	}
}

