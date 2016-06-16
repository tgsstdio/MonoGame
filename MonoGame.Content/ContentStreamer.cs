using System;
using System.IO;

namespace MonoGame.Content
{
	public class ContentStreamer : IContentStreamer
	{
		private readonly IBlockLocator mLocator;
		private readonly IFileSystem mFileSystem;
		public ContentStreamer (IBlockLocator locator, IFileSystem fileSystem)
		{
			mLocator = locator;
			mFileSystem = fileSystem;
		}

		public Stream LoadContent(AssetIdentifier assetId, string[] extensions)
		{
			var blockPath = mLocator.GetBlockPath (assetId);

			foreach (var ext in extensions)
			{
				var path = mLocator.GetLocalPath (assetId) + ext;

				if (mFileSystem.Exists (blockPath, path))
				{
//					if (!mFileSystem.IsRegistered (blockPath))
//					{
//						mFileSystem.Register (blockPath);
//					}

					return mFileSystem.OpenStream (blockPath, path);
				}
			}

			// IF REACHES HERE
			throw new Exception ("Stream not found.");
		}
	}
}

