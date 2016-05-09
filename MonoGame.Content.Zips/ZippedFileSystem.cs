using System.IO;
using System.Collections.Generic;
using System.IO.Compression;
using Microsoft.Xna.Framework;
using System;

namespace MonoGame.Content.Zips
{
	public class ZippedFileSystem : IFileSystem
	{
		private ITitleContainer mTitleContainer;
		private readonly Dictionary<uint, ZippedBlockEntry> mBlocks;
		public ZippedFileSystem (ITitleContainer container)
		{
			mBlocks = new Dictionary<uint, ZippedBlockEntry> ();
			mTitleContainer = container;
		}

		~ZippedFileSystem()
		{
			Dispose (false);
		}

		public void Dispose ()
		{
			Dispose (true);
			GC.SuppressFinalize (this);
		}

		void ReleaseUnmanagedResources ()
		{
	
		}

		void ReleaseManagedResources ()
		{
			
		}

		private bool mDisposed = false;
		protected virtual void Dispose(bool dispose)
		{
			if (mDisposed)
			{
				return;
			}

			ReleaseUnmanagedResources();

			if (dispose)
			{
				ReleaseManagedResources ();
			}

			mDisposed = true;
		}


		#region IFileSystem implementation

		private class ZippedBlockEntry
		{
			public BlockIdentifier Id;
			public string Path;
		}


		public bool Register (BlockIdentifier identifier)
		{
			// already
			if (IsRegistered (identifier))
			{
				return true;
			}

			string dirPath = identifier.BlockId.ToString();

			string zipFile = GetZipFileName (identifier);
			string fullPath = System.IO.Path.Combine (dirPath, zipFile);

			var archive = new ZippedBlockEntry();
			archive.Id = identifier;
			archive.Path = fullPath;

			mBlocks.Add (identifier.BlockId, archive);

			return true;
		}

		private static string GetZipFileName (BlockIdentifier identifier)
		{
			return identifier.BlockId + ".zip";
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

			return new ZipArchive (mTitleContainer.OpenStream(found.Path), ZipArchiveMode.Read, false).GetEntry (path).Open ();
		}
		#endregion
	}
}

