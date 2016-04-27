using System;
using MonoGame.Core;
using MonoGame.Content;

namespace MonoGame.Graphics
{
	public class MgTexture2DLoader : ITexture2DLoader
	{
		public MgTexture2DLoader ()
		{
		}

		#region ITexture2DLoader implementation

		public ITexture2D Load (AssetIdentifier assetId)
		{
			throw new NotImplementedException ();
		}

		#endregion
	}
}

