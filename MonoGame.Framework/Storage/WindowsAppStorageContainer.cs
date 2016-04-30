using System;
using System.IO;

namespace Microsoft.Xna.Framework.Storage
{
	public class WindowsAppStorageContainer : BaseStorageContainer
	{
		public WindowsAppStorageContainer (string basePath)
			: base(basePath)
		{
			
		}

		#region implemented abstract members of StorageContainer

		protected override void PlatformCreateDirectoryAbsolute (string dirPath)
        {
			// Now let's try to create it
#if WINDOWS_STOREAPP || WINDOWS_UAP
			var folder = ApplicationData.Current.LocalFolder;
            var task = folder.CreateFolderAsync(path, CreationCollisionOption.OpenIfExists);
            task.AsTask().Wait();
#else
			throw new NotSupportedException();
//            if (!Directory.Exists(path))
//            {
//                Directory.CreateDirectory(path);
//            }
#endif
        }

		protected override Stream PlatformCreateFile (string filePath)
		{
#if WINDOWS_STOREAPP || WINDOWS_UAP
			var folder = ApplicationData.Current.LocalFolder;
            var awaiter = folder.OpenStreamForWriteAsync(filePath, CreationCollisionOption.ReplaceExisting).GetAwaiter();
            return awaiter.GetResult();
#else
            // return A new file with read/write access.
			throw new NotSupportedException();
			//return File.Create(filePath);				
#endif				
		}

		protected override void PlatformDeleteDirectory (string dirPath)
		{			
#if WINDOWS_STOREAPP || WINDOWS_UAP
			var folder = ApplicationData.Current.LocalFolder;
            var deleteFolder = folder.GetFolderAsync(dirPath).AsTask().GetAwaiter().GetResult();
            deleteFolder.DeleteAsync().AsTask().Wait();
#else
			throw new NotSupportedException();
            //Directory.Delete(dirPath);
#endif
		}

		protected override void PlatformDeleteFile (string filePath)
		{
#if WINDOWS_STOREAPP || WINDOWS_UAP
			var folder = ApplicationData.Current.LocalFolder;
            var deleteFile = folder.GetFileAsync(filePath).AsTask().GetAwaiter().GetResult();
            deleteFile.DeleteAsync().AsTask().Wait();
#else
            // Now let's try to delete it
			throw new NotSupportedException();
			//File.Delete(filePath);		
#endif	
		}

		protected override bool PlatformDirectoryExists (string dirPath)
		{
#if WINDOWS_STOREAPP || WINDOWS_UAP
			var folder = ApplicationData.Current.LocalFolder;

            try
            {
                var result = folder.GetFolderAsync(dirPath).GetResults();
            return result != null;
            }
            catch
            {
                return false;
            }
#else            
			throw new NotSupportedException();
            //return Directory.Exists(dirPath);
#endif
		}

		protected override bool PlatformFileExists (string filePath)
		{
#if WINDOWS_STOREAPP || WINDOWS_UAP
			var folder = ApplicationData.Current.LocalFolder;
            // GetFile returns an exception if the file doesn't exist, so we catch it here and return the boolean.
            try
            {
                var existsFile = folder.GetFileAsync(filePath).GetAwaiter().GetResult();
                return existsFile != null;
            }
            catch
            {
                return false;
            }
#else
            // return A new file with read/write access.
			throw new NotSupportedException();
			//return File.Exists(filePath);		
#endif	
		}

		protected override string[] PlatformGetFileNames (string storagePath, string searchPattern)
		{
#if WINDOWS_STOREAPP || WINDOWS_UAP
			var folder = ApplicationData.Current.LocalFolder;
			var results = folder.GetFilesAsync().AsTask().GetAwaiter().GetResult();
			return results.Select<StorageFile, string>(e => e.Name).ToArray();
#else
			throw new NotSupportedException();
			return Directory.GetFiles(_storagePath);
#endif
		}

		protected override string[] PlatformGetDirectoryNames (string storagePath)
		{
#if WINDOWS_STOREAPP || WINDOWS_UAP
			var folder = ApplicationData.Current.LocalFolder;
			var results = folder.GetFoldersAsync().AsTask().GetAwaiter().GetResult();
			return results.Select<StorageFolder, string>(e => e.Name).ToArray();
#else
			throw new NotSupportedException();
			//return Directory.GetDirectories(_storagePath);
#endif
		}

		protected override string[] PlatformGetFileNames (string storagePath)
		{
#if WINDOWS_STOREAPP || WINDOWS_UAP
			var folder = ApplicationData.Current.LocalFolder;
            var results = folder.GetFilesAsync().AsTask().GetAwaiter().GetResult();
            return results.Select<StorageFile, string>(e => e.Name).ToArray();
#else
			throw new NotSupportedException();
			return Directory.GetFiles(storagePath);
#endif
		}

		protected override Stream PlatformOpenFile (string filePath, StorageFileMode fileMode, StorageFileAccess fileAccess, StorageFileShare fileShare)
		{
#if WINDOWS_STOREAPP || WINDOWS_UAP
				var folder = ApplicationData.Current.LocalFolder;
	            if (fileMode == FileMode.Create || fileMode == FileMode.CreateNew)
	            {
	                return folder.OpenStreamForWriteAsync(filePath, CreationCollisionOption.ReplaceExisting).GetAwaiter().GetResult();
	            }
	            else if (fileMode == FileMode.OpenOrCreate)
	            {
	                if (fileAccess == FileAccess.Read && FileExists(file))
	                    return folder.OpenStreamForReadAsync(filePath).GetAwaiter().GetResult();
	                else
	                {
	                    // Not using OpenStreamForReadAsync because the stream position is placed at the end of the file, instead of the beginning
	                    var f = folder.CreateFileAsync(filePath, CreationCollisionOption.OpenIfExists).AsTask().GetAwaiter().GetResult();
	                    return f.OpenAsync(FileAccessMode.ReadWrite).AsTask().GetAwaiter().GetResult().AsStream();
	                }
	            }
	            else if (fileMode == FileMode.Truncate)
	            {
	                return folder.OpenStreamForWriteAsync(filePath, CreationCollisionOption.ReplaceExisting).GetAwaiter().GetResult();
	            }
	            else
	            {
	                //if (fileMode == FileMode.Append)
	                // Not using OpenStreamForReadAsync because the stream position is placed at the end of the file, instead of the beginning
	                folder.CreateFileAsync(filePath, CreationCollisionOption.OpenIfExists).AsTask().GetAwaiter().GetResult().OpenAsync(FileAccessMode.ReadWrite).AsTask().GetAwaiter().GetResult().AsStream();
	                var f = folder.CreateFileAsync(filePath, CreationCollisionOption.OpenIfExists).AsTask().GetAwaiter().GetResult();
	                return f.OpenAsync(FileAccessMode.ReadWrite).AsTask().GetAwaiter().GetResult().AsStream();
	            }
#else
			throw new NotSupportedException();
				//return File.Open(filePath, (FileMode) fileMode, (FileAccess) fileAccess, (FileShare) fileShare);
#endif
		}

		#endregion


	}
}

