using MonoGame.Graphics;
using MonoGame.Content;

namespace MonoGame.Models
{
	public class DefaultModelLoader : IModelLoader
	{
		private readonly IFileSystem mFileSystem;
		private readonly IBlockLocator mLocator;
		public DefaultModelLoader (IFileSystem fs, IBlockLocator locator)
		{
			mFileSystem = fs;
			mLocator = locator;
		}

		#region IModelLoader implementation

		public bool Load (AssetIdentifier identifier)
		{
			var blockId = mLocator.GetSource (identifier);

			string imageFileName = identifier.AssetId + ".bsm";
			using (var fs = mFileSystem.OpenStream (blockId, imageFileName))
			{

			}
			return true;
		}

		#endregion
	}
}

