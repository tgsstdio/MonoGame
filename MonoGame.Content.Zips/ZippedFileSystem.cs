using System.IO;
using System.IO.Compression;
using Microsoft.Xna.Framework;
using System;

namespace MonoGame.Content.Zips
{
	public class ZippedFileSystem : IFileSystem
	{
		private readonly ITitleContainer mTitleContainer;
		public ZippedFileSystem (ITitleContainer container)
		{
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

		private static string GetZipFileName (string identifier)
		{
			return identifier + ".zip";
		}

		public Stream OpenStream (string identifier, string path)
		{
			return new ZipArchive (mTitleContainer.OpenStream(GetZipFileName(identifier)), ZipArchiveMode.Read, false).GetEntry (path).Open ();
		}

		public bool Exists (string blockPath, string localPath)
		{
			using (var zip = new ZipArchive (mTitleContainer.OpenStream (GetZipFileName(blockPath)), ZipArchiveMode.Read, false))
			{
				// NEED TO HANDLE DUPLICATES YOURSELF
				return (zip.GetEntry(localPath) != null);
			}
		}

		#endregion
	}
}

