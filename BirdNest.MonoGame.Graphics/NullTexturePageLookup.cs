using System;
using BirdNest.MonoGame.Core;
using BirdNest.MonoGame.Blocks;

namespace BirdNest.MonoGame.Graphics
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

