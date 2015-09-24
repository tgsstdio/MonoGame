using System;
using KtxSharp;
using System.Collections.Generic;
using FreeImageAPI;
using System.IO;
using BirdNest.Core;

namespace KTXArchiver
{
	public class MipmapGenerator : IMipmapGenerator
	{
		public MipmapGenerator ()
		{
			
		}

		#region IMipmapGenerator implementation
		public string MipmapExtension {
			get;
			private set;
		}
		public void Initialise (string extension)
		{
			MipmapExtension = extension;
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
								using (var fs = File.OpenRead (image.RelativePath))
								{									
									var dib = FreeImage.LoadFromStream (fs);
									var format = FreeImage.GetFIFFromFilename (image.RelativePath);
									// fix before rescale
									FreeImage.FlipVertical (dib);

									for (int level = 0; level < noOfMipmaps; ++level)
									{
										int mipmapWidth = (int)Math.Max (1, chapter.Catalog.Width >> level);
										int mipmapHeight = (int)Math.Max (1, chapter.Catalog.Height >> level);

										var startInfo = new EncoderStartInfo ();
										startInfo.InputFile = string.Format ("{0}_{1}{2}", imageData.Id, level, extension); 
										startInfo.OutputFile = string.Format ("{0}_{1}{2}", imageData.Id, level, MipmapExtension);
										imageData.Mipmaps.Add (startInfo);

										using (var outFile = File.OpenWrite (startInfo.InputFile))
										{
											var clone = FreeImage.Rescale (dib, mipmapWidth, mipmapHeight, FREE_IMAGE_FILTER.FILTER_CATMULLROM);
											FreeImage.SaveToStream (clone, outFile, format);
											FreeImage.UnloadEx (ref clone);
										}
									}

									FreeImage.UnloadEx (ref dib);
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

