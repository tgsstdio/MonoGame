using System.Collections.Generic;
using BirdNest.MonoGame.Core;
using BirdNest.MonoGame.Blocks;

namespace BirdNest.MonoGame
{
	public class AssetManager : IAssetManager
	{
		private readonly Dictionary<ulong, AssetInfo> mResources;
		public AssetManager ()
		{
			mResources = new Dictionary<ulong, AssetInfo> ();
		}

		#region IAssetManager implementation

		public bool Contains (AssetIdentifier key)
		{
			return mResources.ContainsKey (key.AssetId);
		}

		public bool Add (AssetInfo key)
		{
			if (!mResources.ContainsKey (key.Identifier.AssetId))
			{
				mResources.Add (key.Identifier.AssetId, key);
				return true;
			}
			else
			{
				return false;
			}
		}

		public bool Remove (AssetIdentifier key)
		{
			return mResources.Remove (key.AssetId);
		}

		#endregion
	}
}

