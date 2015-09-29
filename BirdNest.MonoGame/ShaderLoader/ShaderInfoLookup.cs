using BirdNest.MonoGame.Graphics;
using System.Collections.Generic;
using BirdNest.MonoGame.Blocks;
using BirdNest.MonoGame.Core;

namespace BirdNest.MonoGame
{
	public class ShaderInfoLookup : IShaderInfoLookup
	{
		private readonly Dictionary<ulong, ShaderInfo> mShaders;
		public ShaderInfoLookup ()
		{
			mShaders = new Dictionary<ulong, ShaderInfo> ();
		}

		#region IShaderInfoLookup implementation

		public bool TryGetValue (AssetIdentifier identifier, out ShaderInfo result)
		{
			return mShaders.TryGetValue (identifier.AssetId, out result);
		}

		public void Scan (BlockIdentifier identifier, ShaderInfo shader)
		{
			shader.Asset.AssetType = AssetType.Shader;
			shader.Asset.Block = identifier;
			mShaders.Add (shader.Asset.Identifier.AssetId, shader);
		}

		#endregion
	}
}

