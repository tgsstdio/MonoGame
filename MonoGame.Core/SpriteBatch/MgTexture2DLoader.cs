using System;

namespace MonoGame.Core
{
	public class MgTexture2DLoader : ITexture2DLoader
	{
		public MgTexture2DLoader ()
		{
		}

		#region ITexture2DLoader implementation

		public ITexture2D Load (MonoGame.Content.AssetIdentifier assetId)
		{
			throw new NotImplementedException ();
		}

		#endregion
	}
}

