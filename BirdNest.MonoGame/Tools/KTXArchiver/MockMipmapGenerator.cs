using System;
using System.Collections.Generic;
using KtxSharp;
using System.IO;
using BirdNest.Core;

namespace KTXArchiver
{
	public class MockMipmapGenerator : IMipmapGenerator
	{
		public MockMipmapGenerator ()
		{
		}

		#region IMipmapGenerator implementation
		private string mMipmapExtension;
		public string MipmapExtension {
			get {
				return mMipmapExtension;
			}
		}

		public void Initialise (string extension)
		{
			mMipmapExtension = extension;
		}

		public List<BlockImageInfo> GenerateMipmaps (BlockFile[] blocks)
		{
			var images = new List<BlockImageInfo>();			
			if (blocks != null)
			{
				foreach (var block in blocks)
				{
					if (block.Chapters != null)
					{
						foreach (var chapter in block.Chapters)
						{
							double log2Base = Math.Log10 (2);
							int levelX = (int)Math.Ceiling (Math.Log10 (chapter.Catalog.Width) / log2Base);
							int levelY = (int)Math.Ceiling (Math.Log10 (chapter.Catalog.Height) / log2Base);
							int noOfMipmaps = Math.Max (levelX, levelY);
							foreach (var image in chapter.Pages)
							{
								var imageData = new BlockImageInfo ();
								imageData.Width = chapter.Catalog.Width;
								imageData.Height = chapter.Catalog.Height;
								images.Add (imageData);
								imageData.Mipmaps = new List<EncoderStartInfo> ();
								imageData.Id = image.Asset.Identifier.Value;
								string extension = Path.GetExtension (image.RelativePath);

								for (int level = 0; level < noOfMipmaps; ++level)
								{
									int mipmapWidth = (int)Math.Max (1, chapter.Catalog.Width >> level);
									int mipmapHeight = (int)Math.Max (1, chapter.Catalog.Height >> level);

									var startInfo = new EncoderStartInfo ();
									startInfo.InputFile = string.Format ("{0}_{1}{2}", imageData.Id, level, extension); 
									startInfo.OutputFile = string.Format ("{0}_{1}{2}", imageData.Id, level, MipmapExtension);
									imageData.Mipmaps.Add (startInfo);

								}

							}
						}
					}
				}
			}
			return images;
		}

		#endregion
	}

}

