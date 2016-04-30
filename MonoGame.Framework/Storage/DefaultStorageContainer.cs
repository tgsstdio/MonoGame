using System;
using System.IO;

namespace Microsoft.Xna.Framework.Storage
{
	public class DefaultStorageContainer : BaseStorageContainer
	{
		public DefaultStorageContainer (string basePath)
			: base(basePath)
		{
			
		}

		#region implemented abstract members of StorageContainer

		protected override void PlatformCreateDirectoryAbsolute (string dirPath)
        {
			if (!Directory.Exists(dirPath))
            {
				Directory.CreateDirectory(dirPath);
            }
        }

		protected override Stream PlatformCreateFile (string filePath)
		{
			return File.Create(filePath);				
		}

		protected override void PlatformDeleteDirectory (string dirPath)
		{
            Directory.Delete(dirPath);
		}

		protected override void PlatformDeleteFile (string filePath)
		{
			File.Delete(filePath);		
		}

		protected override bool PlatformDirectoryExists (string dirPath)
		{
            return Directory.Exists(dirPath);
		}

		protected override bool PlatformFileExists (string filePath)
		{
			return File.Exists(filePath);		
		}

		protected override string[] PlatformGetFileNames (string storagePath, string searchPattern)
		{
			return Directory.GetFiles(_storagePath, searchPattern);
		}

		protected override string[] PlatformGetDirectoryNames (string storagePath)
		{
			return Directory.GetDirectories(_storagePath);
		}

		protected override string[] PlatformGetFileNames (string storagePath)
		{
			return Directory.GetFiles(storagePath);
		}

		protected override Stream PlatformOpenFile (string filePath, StorageFileMode fileMode, StorageFileAccess fileAccess, StorageFileShare fileShare)
		{
			return File.Open(filePath, (FileMode) fileMode, (FileAccess) fileAccess, (FileShare) fileShare);
		}

		#endregion


	}
}

