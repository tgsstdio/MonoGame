using System;
using System.IO;
using KtxSharp;

namespace KTXArchiver
{
	public class ASTCPacker : BaseKTXPacker
	{
		private readonly ASTCEncoder mEncoder;
		public ASTCPacker (ASTCEncoder encoder)
		{
			mEncoder = encoder;
		}

		static FileStream OpenStream (EncoderStartInfo map)
		{
			return File.OpenRead (map.OutputFile);
		}

		#region implemented abstract members of BaseKTXArchiver

		protected override uint GetFileSize (EncoderStartInfo map)
		{
			using (var fs = OpenStream (map))
			{
				var compressedHeader = mEncoder.ReadHeader (fs);
				return (uint)compressedHeader.FileSize;
			}
		}

		protected override void SetupHeaderValues (KTXHeader header, BlockImageInfo image)
		{
			header.GlInternalFormat = (uint)mEncoder.GetInternalFormat ();
		}

		#endregion

		protected override void WriteImageData (Stream ktx, BlockImageInfo image)
		{
			// write imagedata
			foreach (var map in image.Mipmaps)
			{
				int offset = ToBytes32(map.ImageSize, mBuffer, 0);
				using (var fs = File.OpenRead (map.OutputFile))
				{
					mEncoder.SkipHeader (fs);
					int bytesLeft = (int) map.ImageSize;

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
					// TODO : array textures + cube padding
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
}

