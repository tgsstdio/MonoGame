using System.Collections.Generic;
using MonoGame.Content.Blocks;
using MonoGame.Content;

namespace BirdNest.MonoGame
{
	public class SparseTextureLookup : ITextureLookup
	{
		private readonly IAssetManager mAssetManager;

		private readonly Dictionary<ulong, SparseTextureReference> mReferences;
		public SparseTextureLookup (IAssetManager assetManager)
		{
			mAssetManager = assetManager;
			mReferences = new Dictionary<ulong, SparseTextureReference> ();
		}

		#region ITextureLookup implementation

		public bool Add (AssetInfo asset, ArrayTextureLocation location)
		{
			if (asset.AssetType != AssetType.Texture)
			{
				return false;
			}

			if (mReferences.ContainsKey (asset.Identifier.AssetId))
			{
				return false;
			}

			mAssetManager.Add (asset);
			mReferences.Add(asset.Identifier.AssetId, new SparseTextureReference{Location=location, IsResident=false});
			return true;
		}

		public bool TryGetValue (AssetIdentifier key, out ArrayTextureLocation result)
		{
			SparseTextureReference found = null;
			if (mReferences.TryGetValue (key.AssetId, out found))
			{
				result = found.Location;
				return true;
			} 
			else
			{
				result = new ArrayTextureLocation();
				return false;	
			}				
		}

		public bool Contains (AssetIdentifier key)
		{
			return mReferences.ContainsKey(key.AssetId);
		}

		#endregion
	}
}

