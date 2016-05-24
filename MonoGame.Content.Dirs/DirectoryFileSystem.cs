using System.IO;
using System.Collections.Generic;
using MonoGame.Content.Blocks;
using System;
using Microsoft.Xna.Framework;

namespace MonoGame.Content.Dirs
{
	public class DirectoryFileSystem : IFileSystem
	{
		private ITitleContainer mContainer;
		public DirectoryFileSystem (ITitleContainer container)
		{
			mContainer = container;
			mBlocks = new Dictionary<uint, DirectoryBlockEntry> ();
		}

		~DirectoryFileSystem()
		{
			Dispose (false);
		}

		public void Dispose ()
		{
			Dispose (true);
			GC.SuppressFinalize(this);
		}

		protected virtual void ReleaseUnmanagedResources ()
		{

		}

		protected virtual void ReleaseManagedResources ()
		{

		}

		private bool mDisposed = false;
		protected virtual void Dispose(bool dispose)
		{
			if (mDisposed)
				return;

			ReleaseUnmanagedResources ();

			if (dispose)
			{
				ReleaseManagedResources ();
			}

			mDisposed = true;
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
			return mContainer.OpenStream(fullPath);
		}
		private readonly Dictionary<uint, DirectoryBlockEntry> mBlocks;

		public bool Register (BlockIdentifier identifier)
		{
			string dirPath = identifier.BlockId.ToString();

			if (!Directory.Exists (dirPath))
			{
				return false;
			}

			var archive = new DirectoryBlockEntry();
			archive.Id = identifier;
			archive.Path = dirPath;
			archive.BlockFile = dirPath;

			mBlocks.Add (archive.Id.BlockId, archive);
			return true;
		}

		public bool IsRegistered (BlockIdentifier identifier)
		{
			return mBlocks.ContainsKey (identifier.BlockId);
		}

		public bool Exists (BlockIdentifier blockId, string path)
		{
			string fullPath = System.IO.Path.Combine (blockId.BlockId.ToString(), path);
			return mContainer.Exists (fullPath);
		}
	}
}

