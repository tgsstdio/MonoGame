using System;
using System.Collections.Generic;
using System.IO;
using KtxSharp;

namespace KTXArchiver
{
	public class FasTCPacker : BaseKTXPacker
	{
		public FasTCPacker ()
		{
		}

		#region implemented abstract members of BaseKTXPacker

		protected override void WriteImageData (Stream ktx, BlockImageInfo image)
		{
			foreach (var map in image.Mipmaps)
			{
				using (var fs = File.OpenRead (map.OutputFile))
				{
					var topHeader = new KTXHeader ();

					fs.Read (mBuffer, 0, topHeader.KTX_HEADER_SIZE);
					topHeader.Populate (mBuffer);

					long skipOffset = topHeader.BytesOfKeyValueData + topHeader.KTX_HEADER_SIZE + sizeof(UInt32);
					fs.Seek (skipOffset, SeekOrigin.Begin);

					int offset = ToBytes32(map.ImageSize, mBuffer, 0);
					int bytesLeft = (int)map.ImageSize;
					int totalBytes = offset;
					while (bytesLeft > 0)
					{
						int toRead = Math.Min(mBuffer.Length - offset, bytesLeft);
						totalBytes += toRead;

						int bytesRead = fs.Read(mBuffer, offset, toRead);
						ktx.Write (mBuffer, 0, totalBytes);

						bytesLeft -= toRead;
						totalBytes = 0;
						offset = 0;
					}
					if (map.MipPadding > 0)
					{
						mBuffer[0] = 0;
						mBuffer[1] = 0;
						mBuffer[2] = 0;
						ktx.Write (mBuffer, 0, (int)map.MipPadding);
					}
				}
			}
		}

		protected override uint GetFileSize (EncoderStartInfo map)
		{
			using (var fs = File.OpenRead (map.OutputFile))
			{
				var topHeader = new KTXHeader ();

				fs.Read (mBuffer, 0, topHeader.KTX_HEADER_SIZE);
				topHeader.Populate (mBuffer);
				if (topHeader.Instructions.Result != KTXError.Success)
				{
					throw new InvalidDataException ("KTX not found");
				}
				long skipOffset = topHeader.BytesOfKeyValueData + topHeader.KTX_HEADER_SIZE;
				fs.Seek (skipOffset, SeekOrigin.Begin);

				UInt32 faceLodSize;
				if (topHeader.ExtractUInt32 (fs, out faceLodSize))
				{
					return faceLodSize;
				}
				else
				{
					throw new InvalidDataException ();
				}

			}
		}

		protected override void SetupHeaderValues (KtxSharp.KTXHeader header, BlockImageInfo image)
		{			
			using (var fs = File.OpenRead (image.Mipmaps [0].OutputFile))
			{
				var topHeader = new KTXHeader ();

				fs.Read (mBuffer, 0, topHeader.KTX_HEADER_SIZE);
				topHeader.Populate (mBuffer);
				if (topHeader.Instructions.Result != KTXError.Success)
				{
					throw new InvalidDataException ("KTX not found");
				}
				header.GlInternalFormat = topHeader.GlInternalFormat;
			}
		}

		#endregion
	}
}

