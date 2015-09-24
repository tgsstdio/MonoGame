using System;
using System.Collections.Generic;
using System.IO;
using KtxSharp;
using System.Text;

namespace KTXArchiver
{
	public abstract class BaseKTXPacker : IKTXPacker
	{
		protected BaseKTXPacker ()
		{
		}

		#region IKTXArchiver implementation

		protected byte[] mBuffer;
		public void Initialise (byte[] buffer)
		{
			mBuffer = buffer;
		}

		public List<string> PackImages (List<BlockImageInfo> images)
		{
			var result = new List<string> ();

			foreach (var image in images)
			{
				string ktxFileName = string.Format ("{0}.ktx", image.Id);

				var header = InitialiseKTXHeader (image);
				// compressed, maybe
				foreach (var map in image.Mipmaps)
				{
					map.ImageSize = GetFileSize (map);
				}

				using (var ktx = File.OpenWrite (ktxFileName))
				{				
					CompressToKtx (ktx, image, header);
				}

				result.Add (ktxFileName);
			}
			return result;
		}

		protected static int ToBytes32(UInt32 value, byte[] array, int offset)
		{
			byte[] valueBytes = BitConverter.GetBytes(value);
			Array.Copy (valueBytes, 0, array, offset, valueBytes.Length);
			return valueBytes.Length;
		}

		protected static int ToBytes64(ulong value, byte[] array, int offset)
		{
			byte[] valueBytes = BitConverter.GetBytes(value);
			Array.Copy (valueBytes, 0, array, offset, valueBytes.Length);
			return valueBytes.Length;
		}

		int CopyArray (byte[] str1, int count)
		{
			Array.Copy (str1, 0, mBuffer, count, str1.Length);
			return str1.Length;
		}

		static void GenerateOffsetArray (BlockImageInfo image, KTXHeader header)
		{
			// TODO : cube maps for key value table
			ulong previousOffset = (ulong)(header.KTX_HEADER_SIZE + header.BytesOfKeyValueData);
			foreach (var map in image.Mipmaps)
			{
				// each image is involves 1 UInt32 (image size) + image data in bytes + mipPadding
				map.Offset = previousOffset;
				previousOffset += sizeof(UInt32);
				// TODO : array textures + cube padding
				map.MipPadding = 3 - ((map.ImageSize + 3) % 4);
				// mipPadding 
				previousOffset += map.ImageSize;
				previousOffset += map.MipPadding;
			}
		}

		void WriteKeyValueData (Stream ktx, BlockImageInfo image, KTXHeader header)
		{
			header.BytesOfKeyValueData = 0;

			const byte ZEROCHAR = 0x00;

			uint orientationCount = sizeof(UInt32);
			var str1 = Encoding.UTF8.GetBytes ("KTXorientation");
			orientationCount += (uint) str1.Length;
			orientationCount += 1; // ZEROCHAR
			var str2 = Encoding.UTF8.GetBytes ("S=r,T=u");
			orientationCount += (uint) str2.Length;
			orientationCount += 1; // ZEROCHAR
			// PADDING
			uint orientationPadding =  3 - ((orientationCount + 3) % 4);
			header.BytesOfKeyValueData += orientationCount;
			header.BytesOfKeyValueData += orientationPadding;

			uint offsetsCount = sizeof(UInt32);
			var str3 = Encoding.UTF8.GetBytes ("OFFSETS");
			offsetsCount += (uint) str3.Length;
			offsetsCount += 1; // ZEROCHAR
			UInt32 KEY_AND_VALUE_BYTESIZE = sizeof(long) + (header.NumberOfMipmapLevels * sizeof(long));			
			offsetsCount += Math.Max(header.NumberOfArrayElements, 1) * KEY_AND_VALUE_BYTESIZE;
			// PADDING
			uint offsetsPadding =  3 - ((offsetsCount + 3) % 4);
			header.BytesOfKeyValueData += offsetsCount;
			header.BytesOfKeyValueData += offsetsPadding;

			GenerateOffsetArray (image, header);

			int count = header.Write (mBuffer, 0);
			count += ToBytes32(orientationCount, mBuffer, count);
			count += CopyArray (str1, count);
			mBuffer [count++] = ZEROCHAR;
			count += CopyArray (str2, count);
			mBuffer [count++] = ZEROCHAR;
			for (int i = 0; i < orientationPadding; ++i)
			{
				mBuffer [count++] = ZEROCHAR;
			}

			count += ToBytes32(offsetsCount, mBuffer, count);
			count += CopyArray (str3, count);
			mBuffer [count++] = ZEROCHAR;
			count += ToBytes64(image.Id, mBuffer, count);
			foreach (var map in image.Mipmaps)
			{
				count += ToBytes64(map.Offset, mBuffer, count);
			}
			for (int i = 0; i < offsetsPadding; ++i)
			{
				mBuffer [count++] = ZEROCHAR;
			}

			ktx.Write (mBuffer, 0, count);

		}

		private KTXHeader InitialiseKTXHeader (BlockImageInfo image)
		{
			var header = new KTXHeader ();
			header.GlType = 0;
			// always for compressed
			header.GlFormat = 0;
			// always for compressed
			header.GlBaseInternalFormat = header.GlInternalFormat;
			// TODO : double check
			// TODO: 3d textures
			header.PixelDepth = 0;
			// always for texture_2D
			header.PixelWidth = (uint)image.Width;
			header.PixelHeight = (uint)image.Height;
			// TODO : 1d textures
			header.NumberOfMipmapLevels = (uint)image.Mipmaps.Count;
			// TODO : array textures
			header.NumberOfArrayElements = 0;
			// TODO : array texture
			header.NumberOfFaces = 1;
			// TODO : cube maps
			header.GlTypeSize = 1;

			SetupHeaderValues (header, image);

			return header;
		}

		protected abstract void WriteImageData (Stream ktx, BlockImageInfo image);
		protected abstract uint GetFileSize (EncoderStartInfo map);
		protected abstract void SetupHeaderValues (KTXHeader header, BlockImageInfo image);

		private void CompressToKtx (Stream ktx, BlockImageInfo image, KTXHeader header)
		{
			WriteKeyValueData (ktx, image, header);
			WriteImageData (ktx, image);
		}

		#endregion
	}
}

