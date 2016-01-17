using System.IO;
using System.Collections.Generic;
using MonoGame.Content.Blocks;

namespace MonoGame.Content.Dirs
{
	public class DirectoryFileSystem : IFileSystem
	{
		private IAssetLocator mLocator;
		public DirectoryFileSystem (IAssetLocator serializer)
		{
			mLocator = serializer;
		}

		private class DirectoryBlockEntry
		{
			public BlockIdentifier Id;
			public string Path;
			public string BlockFile;
		}

		public Stream OpenStream (BlockIdentifier identifier, string path)
		{
			DirectoryBlockEntry found = null;
			if (!mBlocks.TryGetValue (identifier.BlockId, out found))
			{
				throw new KeyNotFoundException ();
			}

			string fullPath = System.IO.Path.Combine (found.Path, path);
			return File.OpenRead (fullPath);
		}

		public string Path {get; private set;}

		private Dictionary<ulong, DirectoryBlockEntry> mBlocks;
		public void Initialise (string path)
		{
			Path = path;
			mBlocks = new Dictionary<ulong, DirectoryBlockEntry> ();
		}

		public bool Register (BlockIdentifier identifier)
		{
			string dirPath = System.IO.Path.Combine (Path, identifier.BlockId.ToString());

			if (!Directory.Exists (dirPath))
			{
				return false;
			}

			var archive = new DirectoryBlockEntry();
			archive.Id = identifier;
			archive.Path = dirPath;
			archive.BlockFile = System.IO.Path.Combine (dirPath, mLocator.Serializer.GetBlockPath (identifier));

			mBlocks.Add (archive.Id.BlockId, archive);

			using (var fs = File.OpenRead(archive.BlockFile))
			{
				mLocator.Scan (fs);
			}
			return true;
		}

		public bool IsRegistered (BlockIdentifier identifier)
		{
			return mBlocks.ContainsKey (identifier.BlockId);
		}
	}
}

