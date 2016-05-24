using System;
using MonoGame.Core;
using MonoGame.Content;
using FreeImageAPI;

namespace MonoGame.Textures.FreeImageNET
{
	public class FITexture2DLoader : ITexture2DLoader
	{
		private readonly IContentStreamer mContentStreamer;
		private readonly ITextureSortingKeyGenerator mKeyGenerator;
		public FITexture2DLoader (IContentStreamer cStreamer, ITextureSortingKeyGenerator keyGenerator)
		{
			mContentStreamer = cStreamer;
			mKeyGenerator = keyGenerator;

			// Check if FreeImage is available
			if (!FreeImage.IsAvailable())
			{
				throw new Exception("FreeImage is not available!");
			}
		}

		#region ITexture2DLoader implementation

		public ITexture2D Load (AssetIdentifier assetId)
		{
			FIBITMAP dib = new FIBITMAP();
			try
			{
				using (var fs = mContentStreamer.LoadContent (assetId, new []{".png", ".jpg"}))
				{
					dib = FreeImage.LoadFromStream(fs);
					// Check success
					if (dib.IsNull)
					{
						throw new Exception("FITexture2DLoader image load error");
					}

					// stuff here
					int key = mKeyGenerator.GenerateSortingKey();
					return new FITexture2D(key);
				}
			}
			finally
			{
				FreeImage.UnloadEx (ref dib);
			}
		}

		#endregion
	}
}

