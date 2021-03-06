﻿using MonoGame.Content;
using MonoGame.Content.Blocks;

namespace MonoGame.Shaders.GLSL.DesktopGL
{
	public class GLSLTextShaderLoader
	{
		private readonly IFileSystem mFileSystem;
		private readonly IShaderInfoLookup mLookup;
		private readonly IShaderRegistry mRegistry;
		public GLSLTextShaderLoader (IFileSystem fs, IShaderInfoLookup lookup, IShaderRegistry registry)
		{
			mFileSystem = fs;
			mLookup = lookup;
			mRegistry = registry;
		}

		#region IShaderLoader implementation

		public GLShaderProgram Load (AssetIdentifier identifier)
		{	
			// 
			GLShaderProgram existingProgram;
			if (mRegistry.TryGetValue (identifier, out existingProgram))
			{				
				var result = new GLShaderProgram{ Identifier = identifier};				
				result.ProgramID = existingProgram.ProgramID;
				result.Block = existingProgram.Block;
				result.IsLoaded = true;
				return result;
			}

			ShaderInfo scannedAsset = null;
			if (mLookup.TryGetValue (identifier, out scannedAsset))
			{
				var result = new ShaderProgram{ Identifier = identifier};				
				if (!string.IsNullOrWhiteSpace (scannedAsset.ComputePath))
				{
					using (var fs = mFileSystem.OpenStream (scannedAsset.Asset.Block, scannedAsset.ComputePath))
					{
						// TODO : prefixes ???
						result.ProgramID = GLSLTextShaderManager.CreateComputeShader (fs, "");
					}
				} else
				{
					using (var vert = mFileSystem.OpenStream (scannedAsset.Asset.Block, scannedAsset.VertexPath))
					using (var frag = mFileSystem.OpenStream (scannedAsset.Asset.Block, scannedAsset.FragmentPath))
					{
						// TODO : prefixes ???						
						result.ProgramID = GLSLTextShaderManager.CreateFragmentProgram (vert,frag, "");
					}
				}
				result.Block = scannedAsset.Asset.Block;
				mRegistry.Add (scannedAsset.Asset, result);
				result.IsLoaded = true;
				return result;
			}
			else
			{
				var result = new ShaderProgram{ Identifier = identifier};
				result.IsLoaded = false;
				return result;
			}

		}

		#endregion

	}
}

