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
			var blockId = mLocator.GetSource (assetId);

			foreach (var ext in extensions)
			{
				var path = mLocator.GetLocalPath (assetId) + ext;

				if (mFileSystem.Exists (blockId, path))
				{
					if (!mFileSystem.IsRegistered (blockId))
					{
						mFileSystem.Register (blockId);
					}

					return mFileSystem.OpenStream (blockId, path);
				}
			}

			// IF REACHES HERE
			throw new Exception ("Stream not found.");
		}
	}
}

