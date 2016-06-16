using System.IO;
using System.Collections.Generic;
using MonoGame.Content.Blocks;
using System;
using Microsoft.Xna.Framework;

namespace MonoGame.Content.Dirs
{
	public class DirectoryFileSystem : IFileSystem
	{
		private readonly ITitleContainer mContainer;
		public DirectoryFileSystem (ITitleContainer container)
		{
			mContainer = container;
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

		public Stream OpenStream (string blockPath, string localPath)
		{
			string fullPath = Path.Combine (blockPath, localPath);
			return mContainer.OpenStream(fullPath);
		}

		public bool Exists (string blockId, string path)
		{
			string fullPath = Path.Combine (blockId, path);
			return mContainer.Exists (fullPath);
		}
	}
}

