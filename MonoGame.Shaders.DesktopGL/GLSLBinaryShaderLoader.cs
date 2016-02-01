using System.IO;
using OpenTK.Graphics.OpenGL;
using MonoGame.Content;
using MonoGame.Content.Blocks;

namespace MonoGame.Shaders.GLSL.DesktopGL
{
	public class GLBinaryShaderLoader : IShaderLoader
	{
		private readonly IFileSystem mFileSystem;
		private readonly IShaderInfoLookup mLookup;		
		private readonly IShaderRegistry mRegistry;
		public GLBinaryShaderLoader (IFileSystem fs, IShaderInfoLookup lookup, IShaderRegistry registry)
		{
			mFileSystem = fs;
			mLookup = lookup;
			mRegistry = registry;
		}

		#region IShaderLoader implementation

		public ShaderProgram Load (AssetIdentifier identifier)
		{
			ShaderProgram existingProgram;
			if (mRegistry.TryGetValue (identifier, out existingProgram))
			{				
				var result = new ShaderProgram{ Identifier = identifier};				
				result.ProgramID = existingProgram.ProgramID;
				result.Block = existingProgram.Block;
				result.IsLoaded = true;
				return result;
			}

			ShaderInfo scannedAsset = null;
			if (mLookup.TryGetValue (identifier, out scannedAsset))
			{
				var result = new ShaderProgram{ Identifier = identifier};

				string programFilePath = identifier.AssetId + "_glsl.bin";
				if (!string.IsNullOrWhiteSpace (scannedAsset.ComputePath))
				{
					using (var fs = mFileSystem.OpenStream (scannedAsset.Asset.Block, programFilePath))
					using (var ms = new MemoryStream())
					{
						fs.CopyTo(ms);
						byte[] programData = ms.ToArray();
						result.ProgramID = GL.CreateProgram ();
						GL.ProgramBinary<byte> (result.ProgramID, (BinaryFormat) 0, programData, programData.Length);
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

