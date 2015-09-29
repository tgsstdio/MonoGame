using System.IO;
using System.Collections.Generic;
using BirdNest.MonoGame.Core;
using System.IO.Compression;

namespace BirdNest.MonoGame.FileSystem.Zips
{
	public class ZippedFileSystem : IFileSystem
	{
		public string Path { get; private set; }
		private IAssetLocator mLocator;
		public ZippedFileSystem (IAssetLocator locator, string folder)
		{
			Path = folder;
			mLocator = locator;
		}

		#region IFileSystem implementation

		private class ZippedBlockEntry
		{
			public BlockIdentifier Id;
			public string Path;
			public string BlockFile;
		}

		private Dictionary<ulong, ZippedBlockEntry> mBlocks;
		public bool Register (BlockIdentifier identifier)
		{
			// already
			if (IsRegistered (identifier))
			{
				return true;
			}

			string dirPath = System.IO.Path.Combine (Path, identifier.BlockId.ToString());

			if (!Directory.Exists (dirPath))
			{
				return false;
			}

			//string blockFile = System.IO.Path.Combine (dirPath, );
			string zipFile = GetBlockFileName (identifier);
			string fullPath = System.IO.Path.Combine (dirPath, zipFile);

			if (!File.Exists (fullPath))
			{
				return false;
			}

			var archive = new ZippedBlockEntry();
			archive.Id = identifier;
			archive.Path = fullPath;
			archive.BlockFile = mLocator.Serializer.GetBlockPath (identifier);

			using (var fs = File.OpenRead(fullPath))
			using (var zip = new ZipArchive (fs))
			using (var stream = zip.GetEntry(archive.BlockFile).Open())
			{
				mLocator.Scan (stream);
			}

			return true;
		}

		private static string GetBlockFileName (BlockIdentifier identifier)
		{
			return identifier.BlockId + ".zip";
		}

		public void Initialise (string path)
		{
			mBlocks = new Dictionary<ulong, ZippedBlockEntry> ();
			Path = path;
		}

		public bool IsRegistered (BlockIdentifier identifier)
		{
			return mBlocks.ContainsKey (identifier.BlockId);
		}

		public Stream OpenStream (BlockIdentifier identifier, string path)
		{
			ZippedBlockEntry found = null;
			if (!mBlocks.TryGetValue (identifier.BlockId, out found))
			{
				throw new KeyNotFoundException ();
			}

			return new ZipArchive (File.OpenRead (found.Path), ZipArchiveMode.Read, false).GetEntry (path).Open ();
		}
		#endregion
	}
}

