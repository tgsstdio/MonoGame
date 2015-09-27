﻿using System;
using System.IO;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using BirdNest.MonoGame.Blocks;
using BirdNest.MonoGame.FileSystem;
using BirdNest.MonoGame.Core;

namespace BirdNest.MonoGame.Graphics.Ktx
{
	public class KTXTextureManager : ITextureManager
	{
		private readonly IAssetManager mAssetManager;
		private readonly IFileSystem mFileSystem;
		private readonly ITexturePageLookup mPageLookup;
		private readonly IETCUnpacker mETCUnpacker;
		private readonly ITextureAtlas mTextureAtlas;

		public KTXTextureManager		(
			IAssetManager am,
			IFileSystem fs,
			ITexturePageLookup pageLookup,
			IETCUnpacker unpacker,
			ITextureAtlas atlas
		)
		{
			mAssetManager = am;
			mFileSystem = fs;
			mPageLookup = pageLookup;
			mETCUnpacker = unpacker;
			mTextureAtlas = atlas;
		}

		public bool Load(AssetIdentifier identifier)
		{
			TexturePageInfo result = null;
			if (mPageLookup.TryGetValue (identifier, out result))
			{
				string imageFileName = identifier.AssetId + ".ktx";
				using (var fs = mFileSystem.OpenStream (result.Asset.Block, imageFileName))
				{
					KTXHeader header = null;

					var headerChunk = new byte[header.KTX_HEADER_SIZE];
					int count = fs.Read (headerChunk, 0, header.KTX_HEADER_SIZE);
					if (count != header.KTX_HEADER_SIZE)
					{
						return false;
					}

					var status = ReadHeader (headerChunk, out header);
					if (status != KTXError.Success)
					{
						return false;
					}
					ReadKeyValueDataSection (fs, header.KeyValueData, (int)header.BytesOfKeyValueData);

					//KeyValueArrayData[] inputData = GenerateKeyValueArray (destHeader);
					bool isMipmapped;
					ErrorCode glErrorCode;

					status = LoadTexture(fs, result, header, out isMipmapped, out glErrorCode);
					return (status == KTXError.Success);
				}
			}
			else
			{
				return false;
			}
		}

		static KeyValueArrayData[] GenerateKeyValueArray (KTXHeader destHeader)
		{
			var output = new List<KeyValueArrayData> ();

			int offset = 0;
			do
			{
				var keyValue = new KeyValueArrayData ();
				var keyValueByteSize = destHeader.GetEndian32 (destHeader.KeyValueData, ref offset);
				keyValue.Id = destHeader.GetEndian64 (destHeader.KeyValueData, ref offset);
				keyValue.Offsets = new ulong[destHeader.NumberOfMipmapLevels];
				for (int j = 0; j < keyValue.Offsets.Length; ++j)
				{
					keyValue.Offsets [j] = destHeader.GetEndian64 (destHeader.KeyValueData, ref offset);
				}
				output.Add (keyValue);
			} 
			while(offset < destHeader.KeyValueData.Length);

			return output.ToArray();
		}

		static bool IsGenerateMipmapMissing ()
		{
			//return (glGenerateMipmap == null);
			return false;
		}

	//		/**
	//		 * @internal
	//		 * @~English
	//		 * @brief Check a KTX file header.
	//		 *
	//		 * As well as checking that the header identifies a KTX file, the function
	//		 * sanity checks the values and returns information about the texture in a
	//		 * KTX_texinfo structure.
	//		 *
	//		 * @param header	pointer to the KTX header to check
	//		 * @param texinfo	pointer to a KTX_texinfo structure in which to return
	//		 *                  information about the texture.
	//		 * 
	//		 * @author Georg Kolling, Imagination Technology
	//		 * @author Mark Callow, HI Corporation
	//		 */
		static KTXError ReadKeyValueDataSection (Stream stream, byte[] buffer, int count)
		{
			if (count <= 0)
			{
				return KTXError.Success;
			}

			if (buffer == null)
			{
				/* skip key/value metadata */
				stream.Seek ((long)count, SeekOrigin.Current);
				return KTXError.Success;
			}

			int kvdCount = stream.Read (buffer, 0, count);
			if (kvdCount != count)
			{
				return KTXError.InvalidOperation;
			} 

			return KTXError.Success;
		}

		private static KTXError ReadHeader (byte[] headerChunk, out KTXHeader header)
		{			
			KTXError errorCode = KTXError.Success;
			header = new KTXHeader ();
			header.Populate (headerChunk);
			if (header.Instructions.Result != KTXError.Success)
			{
				return header.Instructions.Result;
			}
			header.KeyValueData = new byte[header.BytesOfKeyValueData];
			if (header.KeyValueData == null)
			{
				return KTXError.OutOfMemory;
			}
			return errorCode;
		}

		const int KTX_GL_UNPACK_ALIGNMENT = 4;
		private int previousUnpackAlignment;
		private void SetKTXUnpackedAlignment ()
		{

			/* KTX files require an unpack alignment of 4 */
			GL.GetInteger (GetPName.UnpackAlignment, out previousUnpackAlignment);
			if (previousUnpackAlignment != KTX_GL_UNPACK_ALIGNMENT)
			{
				GL.PixelStore (PixelStoreParameter.UnpackAlignment, KTX_GL_UNPACK_ALIGNMENT);
			}
		}

		private void RestoreUnpackedAlignment ()
		{
			/* restore previous GL state */
			if (previousUnpackAlignment != KTX_GL_UNPACK_ALIGNMENT)
			{
				GL.PixelStore (PixelStoreParameter.UnpackAlignment, previousUnpackAlignment);
			}
		}

		private static void SetAutomaticMipmapCreation (KTXLoadInstructions texinfo)
		{
			// DISABLED ON PURPOSE
			//			// Prefer glGenerateMipmaps over GL_GENERATE_MIPMAP
			//			if (texinfo.GenerateMipmaps && IsGenerateMipmapMissing ())
			//			{
			//				GL.TexParameter ((TextureTarget)texinfo.GlTarget, TextureParameterName.GenerateMipmap, (int)All.True);
			//			}
		}

		static void ReleaseTexture (int textureId)
		{
			GL.DeleteTexture (textureId);
		}

		private ITexturePage GenerateTexturePage (TexturePageInfo pageInfo, KTXHeader header)
		{
			TextureTarget glTarget = (TextureTarget)header.Instructions.GlTarget;
			if (header.Instructions.GlTarget == (uint) TextureTarget.TextureCubeMap)
			{
				header.Instructions.GlTarget = (uint) TextureTarget.TextureCubeMapPositiveX;
			}

			header.Instructions.GlInternalFormat = header.GlInternalFormat;
			header.Instructions.GlFormat = header.GlFormat;
			if (!header.Instructions.IsCompressed)
			{
				//		#if SUPPORT_LEGACY_FORMAT_CONVERSION
				//				// If sized legacy formats are supported there is no need to convert.
				//				// If only unsized formats are supported, there is no point in converting
				//				// as the modern formats aren't supported either.
				//				if (sizedFormats == _NON_LEGACY_FORMATS && supportsSwizzle) {
				//					convertFormat(texinfo.glTarget, &glFormat, &glInternalFormat);
				//					errorTmp = glGetError();
				//				} else if (sizedFormats == _NO_SIZED_FORMATS)
				//					glInternalFormat = header.glBaseInternalFormat;
				//		#else
				// When no sized formats are supported, or legacy sized formats are not
				// supported, must change internal format.
				if (mETCUnpacker.Profile.SizedFormats == GLSizedFormats.None
					|| (((mETCUnpacker.Profile.SizedFormats & GLSizedFormats.Legacy) != 0) &&
						(header.GlBaseInternalFormat == (UInt32) PixelInternalFormat.Alpha
							|| header.GlBaseInternalFormat == (UInt32) PixelInternalFormat.Luminance
							|| header.GlBaseInternalFormat == (UInt32) PixelInternalFormat.LuminanceAlpha
							|| header.GlBaseInternalFormat == (UInt32) PixelInternalFormat.Intensity)))
				{
					header.Instructions.GlInternalFormat = header.GlBaseInternalFormat;
				}
				//	#endif
			}

			var imageType = new GLImageType {
				GlType = (int) header.GlType,
				GlTypeSize = (int) header.GlTypeSize,
				GlFormat = (int) header.Instructions.GlFormat,
				GlInternalFormat = (int) header.Instructions.GlInternalFormat,
				GlBaseInternalFormat = (int) header.GlBaseInternalFormat,
				NoOfMipmapLevels = (int) header.NumberOfMipmapLevels
			};

			// TODO: header.Instructions.GlTarget == TextureTarget.TextureCubeMapPositiveX cubemaps

			var dims = new ImageDimensions {
				Width = (int) header.PixelWidth,
				Height = (int) header.PixelHeight,
				Depth = (int) header.PixelDepth,
			};

			// added a new chapter if required
			var chapter = mTextureAtlas.Add (pageInfo.Catalog, imageType, glTarget, dims);
			var page = chapter.GeneratePage (pageInfo);
			return page;
		}

		//		/*
		//		 * @~English
		//		 * @brief Load a GL texture object from a ktxStream.
		//		 *
		//		 * This function will unpack compressed GL_ETC1_RGB8_OES and GL_ETC2_* format
		//		 * textures in software when the format is not supported by the GL context,
		//		 * provided the library has been compiled with SUPPORT_SOFTWARE_ETC_UNPACK
		//		 * defined as 1.
		//		 *
		//		 * It will also convert textures with legacy formats to their modern equivalents
		//		 * when the format is not supported by the GL context, provided that the library
		//		 * has been compiled with SUPPORT_LEGACY_FORMAT_CONVERSION defined as 1.
		//		 *
		//		 * @param [in] stream		pointer to the ktxStream from which to load.
		//		 * @param [in,out] pTexture	name of the GL texture to load. If NULL or if
		//		 *                          <tt>*pTexture == 0</tt> the function will generate
		//		 *                          a texture name. The function binds either the
		//		 *                          generated name or the name given in @p *pTexture
		//		 * 						    to the texture target returned in @p *pTarget,
		//		 * 						    before loading the texture data. If @p pTexture
		//		 *                          is not NULL and a name was generated, the generated
		//		 *                          name will be returned in *pTexture.
		//		 * @param [out] pTarget 	@p *pTarget is set to the texture target used. The
		//		 * 						    target is chosen based on the file contents.
		//		 * @param [out] pDimensions	If @p pDimensions is not NULL, the width, height and
		//		 *							depth of the texture's base level are returned in the
		//		 *                          fields of the KTX_dimensions structure to which it points.
		//		 * @param [out] pIsMipmapped
		//		 *	                        If @p pIsMipmapped is not NULL, @p *pIsMipmapped is set
		//		 *                          to GL_TRUE if the KTX texture is mipmapped, GL_FALSE
		//		 *                          otherwise.
		//		 * @param [out] pGlerror    @p *pGlerror is set to the value returned by
		//		 *                          glGetError when this function returns the error
		//		 *                          KTX_GL_ERROR. glerror can be NULL.
		//		 * @param [in,out] pKvdLen	If not NULL, @p *pKvdLen is set to the number of bytes
		//		 *                          of key-value data pointed at by @p *ppKvd. Must not be
		//		 *                          NULL, if @p ppKvd is not NULL.
		//		 * @param [in,out] ppKvd	If not NULL, @p *ppKvd is set to the point to a block of
		//		 *                          memory containing key-value data read from the file.
		//		 *                          The application is responsible for freeing the memory.
		//		 *
		//		 *
		//		 * @return	KTX_SUCCESS on success, other KTX_* enum values on error.
		//		 *
		//		 * @exception KTX_INVALID_VALUE @p target is @c NULL or the size of a mip
		//		 * 							    level is greater than the size of the
		//		 * 							    preceding level.
		//		 * @exception KTX_INVALID_OPERATION @p ppKvd is not NULL but pKvdLen is NULL.
		//		 * @exception KTX_UNEXPECTED_END_OF_FILE the file does not contain the
		//		 * 										 expected amount of data.
		//		 * @exception KTX_OUT_OF_MEMORY Sufficient memory could not be allocated to store
		//		 *                              the requested key-value data.
		//		 * @exception KTX_GL_ERROR      A GL error was raised by glBindTexture,
		//		 * 								glGenTextures or gl*TexImage*. The GL error
		//		 *                              will be returned in @p *glerror, if glerror
		//		 *                              is not @c NULL.
		//		 */

		/// <summary>
		/// Load a GL texture object from a ktxStream.
		/// </summary>
		/// <returns>KTX_SUCCESS on success, other KTX_* enum values on error.</returns>
		/// <param name="stream">Stream.</param>
		/// <param name ="pageInfo"></param>
		/// <param name ="header"></param>
		/// <param name="pIsMipmapped">P is mipmapped.</param>
		/// <param name="pGlerror">P glerror.</param>
		KTXError LoadTexture (
			Stream stream,
			TexturePageInfo pageInfo,
			KTXHeader header,
		//	ref int pTexture,
		//	out int pTarget,
		//	out KTX_dimensions pDimensions,
			out bool pIsMipmapped,
			out ErrorCode pGlerror)
		{
			// default out values
			//pDimensions = null;
			pGlerror = ErrorCode.NoError;
			pIsMipmapped = false;

			KTXError errorCode = KTXError.Success;

			SetKTXUnpackedAlignment ();

//		#ifdef GL_TEXTURE_MAX_LEVEL
//			if (!texinfo.generateMipmaps)
//				glTexParameteri(texinfo.glTarget, GL_TEXTURE_MAX_LEVEL, header.numberOfMipmapLevels - 1);
//		#endif

			var texPage = GenerateTexturePage (pageInfo, header);
			SetAutomaticMipmapCreation (header.Instructions);

			try
			{				
				bool isFirstTime = true;
				int previousLodSize = 0;
				byte[] data = null;

				for (int level = 0; level < header.NumberOfMipmapLevels; ++level)
				{
					UInt32 faceLodSize;
					if (!header.ExtractUInt32 (stream, out faceLodSize))
					{
						errorCode = KTXError.InvalidOperation;
						break;
					}

					int faceLodSizeRounded = ((int)faceLodSize + 3) & ~3;

					// array texture is this correct ?
					if (isFirstTime)
					{
						isFirstTime = false;
						previousLodSize = faceLodSizeRounded;

						/* allocate memory sufficient for the first level */
						data = new byte[faceLodSizeRounded];
						if (data == null)
						{
							errorCode = KTXError.OutOfMemory;
							break;
						}
					}
					else
					{
						if (previousLodSize < faceLodSizeRounded)
						{
							/* subsequent levels cannot be larger than the first level */
							errorCode = KTXError.InvalidValue;
							break;
						}
					}

					var mipmap = new MipmapData();
					mipmap.Level = level;
					mipmap.Data = data;
					mipmap.Size = (int)faceLodSize;
					mipmap.SizeRounded = faceLodSizeRounded;

					var loadError = ExtractFace (stream, header, mipmap, texPage);
					if (loadError != KTXError.Success)
					{
						errorCode = loadError;
						break;
					}

				}
			}
			finally
			{
				RestoreUnpackedAlignment ();

				if (errorCode == KTXError.Success)
				{
					// MANUAL 
//					if (texinfo.generateMipmaps > 0 && !IsGenerateMipmapMissing ()) 
//					{
//						GL.GenerateMipmap(texinfo.glTarget);
//					}

					if (header.Instructions.GenerateMipmaps || (header.NumberOfMipmapLevels > 1))
					{
						pIsMipmapped = true;
					} 
					else
					{
						pIsMipmapped = false;
					}

				}
				else
				{
					//ReleaseTexture (textureId);				
				}
			}
			return errorCode;
		}

//		void PUTBITS<TValue, TValue2>(ref TValue dest, TValue2 data, int size,int startpos)
//		{
//			int bitMask = MASK (size, startpos);
//			int bitShift = SHIFT (size, startpos);
//
//			dest = dest & ((TValue) ~bitMask);
//			dest |= ((data << bitShift) & bitMask);
//		}

//		byte GETBITS<TValue>(TValue source, int size, int startpos) 
//		{
//			return (byte)(((source) >> ((startpos) - (size) + 1)) & ((1 << (size)) - 1));
//		}

		KTXError ExtractFace (
			Stream stream,
			//int level,
			KTXHeader header,
			MipmapData mipmap,
			ITexturePage texPage
			//byte[] data,
			//UInt32 faceLodSize,
			//int faceLodSizeRounded,
			//ref ErrorCode pGlerror
			)
		{
			mipmap.PixelWidth = (int)Math.Max (1, header.PixelWidth >> mipmap.Level);
			mipmap.PixelHeight = (int)Math.Max (1, header.PixelHeight >> mipmap.Level);
			mipmap.PixelDepth = (int)Math.Max (1, header.PixelDepth >> mipmap.Level);

			for (int face = 0; face < header.NumberOfFaces; ++face)
			{
				var bytesRead = stream.Read (mipmap.Data, 0,  mipmap.SizeRounded);
				if (bytesRead != mipmap.SizeRounded)
				{
					return KTXError.UnexpectedEndOfFile;
				}

				/* Perform endianness conversion on texture data */
				if (header.RequiresSwap() && header.GlTypeSize == 2)
				{
					KTXHeader.SwapEndian16 (mipmap.Data, (int)mipmap.Size);
				}
				else if (header.RequiresSwap() && header.GlTypeSize == 4)
				{
					KTXHeader.SwapEndian32 (mipmap.Data, (int)mipmap.Size);
				}

				mipmap.NumberOfFaces = (int)header.NumberOfFaces;
				mipmap.Target = (TextureTarget) header.Instructions.GlTarget;
				mipmap.IsCompressed = header.Instructions.IsCompressed;
				mipmap.TextureDimensions = header.Instructions.TextureDimensions;
				mipmap.FaceIndex = face;

				if (header.Instructions.TextureDimensions == 1)
				{					
//					if (header.Instructions.IsCompressed)
//					{
//						GL.CompressedTexImage1D<byte> (
//							(TextureTarget)(header.Instructions.GlTarget + face),
//							mipmap.Level,
//							(PixelInternalFormat)header.Instructions.GlInternalFormat,
//							mipmap.PixelWidth,
//							0,
//							mipmap.Size,
//							mipmap.Data);
//					}
//					else
//					{
//						GL.TexImage1D<byte> (
//							(TextureTarget)(header.Instructions.GlTarget + face),
//							mipmap.Level,
//							(PixelInternalFormat)header.Instructions.GlInternalFormat,
//							mipmap.PixelWidth,
//							0,
//							(PixelFormat)header.Instructions.GlFormat,
//							(PixelType)header.GlType,
//							mipmap.Data);
//					}
				}
				else if (header.Instructions.TextureDimensions == 2)
				{
					if (header.NumberOfArrayElements > 0)
					{
						mipmap.PixelHeight = (int)header.NumberOfArrayElements;
					}
//					if (header.Instructions.IsCompressed)
//					{
//						// It is simpler to just attempt to load the format, rather than divine which
//						// formats are supported by the implementation. In the event of an error,
//						// software unpacking can be attempted.
//						GL.CompressedTexImage2D<byte> (
//							(TextureTarget)(header.Instructions.GlTarget + face),
//							mipmap.Level,
//							(PixelInternalFormat)header.Instructions.GlInternalFormat,
//							mipmap.PixelWidth,
//							mipmap.PixelHeight,
//							0,
//							mipmap.Size,
//							mipmap.Data);
//					}
//					else
//					{
//						GL.TexImage2D<byte> (
//							(TextureTarget)(header.Instructions.GlTarget + face),
//							mipmap.Level,
//							(PixelInternalFormat)header.Instructions.GlInternalFormat,
//							mipmap.PixelWidth,
//							mipmap.PixelHeight,
//							0,
//							(PixelFormat)header.Instructions.GlFormat,
//							(PixelType)header.GlType,
//							mipmap.Data);
//					}
				}
				else if (header.Instructions.TextureDimensions == 3)
				{
					if (header.NumberOfArrayElements > 0)
					{
						mipmap.PixelDepth = (int)header.NumberOfArrayElements;
					}
//					if (header.Instructions.IsCompressed)
//					{
//						GL.CompressedTexImage3D<byte> (
//							(TextureTarget)(header.Instructions.GlTarget + face),
//							mipmap.Level,
//							(PixelInternalFormat)header.Instructions.GlInternalFormat,
//							mipmap.PixelWidth,
//							mipmap.PixelHeight,
//							mipmap.PixelDepth,
//							0,
//							mipmap.Size,
//							mipmap.Data);
//					}
//					else
//					{
//						GL.TexImage3D<byte> (
//							(TextureTarget)(header.Instructions.GlTarget + face),
//							mipmap.Level,
//							(PixelInternalFormat)header.Instructions.GlInternalFormat,
//							mipmap.PixelWidth,
//							mipmap.PixelHeight,
//							mipmap.PixelDepth,
//							0,
//							(PixelFormat)header.Instructions.GlFormat,
//							(PixelType)header.GlType,
//							mipmap.Data);
//					}
				}
				texPage.Initialise(mipmap);
				mipmap.GLError = GL.GetError ();

				// Renderion is returning INVALID_VALUE. Oops!!
				if (mETCUnpacker.IsRequired (header.Instructions, mipmap.GLError))
				{
					var result = mETCUnpacker.UnpackCompressedTexture (
						header.Instructions,
						mipmap.Level,
						face,
						mipmap.PixelWidth,
						mipmap.PixelHeight,
						mipmap.Data);
					
					if (result != KTXError.Success)
					{
						return result;
					}					

					mipmap.GLError = GL.GetError ();
				}

				if (mipmap.GLError != ErrorCode.NoError)
				{
					return KTXError.GLErrorFound;
				}
			}
			return KTXError.Success;
		}

	}
}

