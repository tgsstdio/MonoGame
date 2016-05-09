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

		public Stream LoadContent(AssetIdentifier assetId, string extension)
		{
			var blockId = mLocator.GetSource (assetId);
			var path = mLocator.GetLocalPath (assetId) + extension;

			if (!mFileSystem.IsRegistered (blockId))
			{
				mFileSystem.Register (blockId);
			}

			return mFileSystem.OpenStream (blockId, path);
		}
	}
}

