using System.Collections.Generic;
using BirdNest.MonoGame.Blocks;
using BirdNest.MonoGame.Core;

namespace BirdNest.MonoGame
{
	public class BindlessHandleLookup : ITextureHandleLookup
	{
		private readonly IAssetManager mAssetManager;
		private readonly Dictionary<ulong, TextureHandleReference> mReferences;
		private List<long> mHandles;
		public BindlessHandleLookup (IAssetManager assetManager)
		{
			mAssetManager = assetManager;
			mReferences = new Dictionary<ulong, TextureHandleReference> ();
			mHandles = new List<long>();
			mHandles.Add (0); // zero means nothing
		}

		#region ITextureHandleLookup implementation

		public bool Add (AssetInfo asset, long handle)
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
			var entry = new TextureHandle{Index=mHandles.Count};
			mHandles.Add (handle);

			mReferences.Add(asset.Identifier.AssetId, new TextureHandleReference{Entry=entry, Handle=handle});
			return true;
		}

		public bool TryGetValue (AssetIdentifier key, out TextureHandle result)
		{
			TextureHandleReference found = null;
			if (mReferences.TryGetValue (key.AssetId, out found))
			{
				result = found.Entry;
				return true;
			} 
			else
			{
				result = new TextureHandle{Index=0};
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

