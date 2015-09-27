using OpenTK.Graphics.OpenGL;
using System;
using BirdNest.MonoGame.Blocks;
using BirdNest.MonoGame.Graphics;

namespace BirdNest.MonoGame
{
	public class SparseTextureChapterAllocator : ITextureChapterAllocator
	{
		private readonly ISparseTexturePageAllocator mSparsePageAllocator;
		private readonly ISinglePageAllocator mSinglePageAllocator;
		private readonly ITextureLookup mLookup;
		private readonly ISparseCubeMapPageAllocator mCubeMapPageAllocator;
		public SparseTextureChapterAllocator (
			ITextureLookup lookup,
			ISparseTexturePageAllocator sparsePageAllocator,
			ISinglePageAllocator singlePageAllocator,
			ISparseCubeMapPageAllocator cubeMapPageAllocator)
		{
			this.mLookup = lookup;
			this.mSparsePageAllocator = sparsePageAllocator;
			this.mSinglePageAllocator = singlePageAllocator;
			this.mCubeMapPageAllocator = cubeMapPageAllocator;
		}

		#region ISparseTextureChapterAllocator implementation
		public ITextureChapter Generate (TextureCatalog catalog, AtlasTextureType imageType, ImageDimensions dims, AtlasTextureTarget texTarget)
		{
			int textureId = GL.GenTexture ();
			TextureTarget glTarget = TextureTarget.Texture2D;
			switch (texTarget)
			{
			case AtlasTextureTarget.Texture1D:
				glTarget = TextureTarget.Texture1DArray;
				break;
			case AtlasTextureTarget.Texture2D:
				glTarget = TextureTarget.Texture2DArray;
				break;
			case AtlasTextureTarget.TextureCubeMap:
				glTarget = TextureTarget.TextureCubeMapArray;
				break;			
			}

			GL.Ext.TextureParameter (textureId, glTarget, (TextureParameterName)All.TextureSparseArb, (int)All.True);

			// from apitest/sparse_bindless_texarray.cpp

			// TODO: This could be done once per internal format. For now, just do it every time.
			int[] indexCount = new int[1];
			int xSize = 0;
			int ySize = 0;
			int zSize = 0;

			int bestIndex = -1,
			bestXSize = 0,
			bestYSize = 0;

			GL.GetInternalformat (
				(ImageTarget)glTarget
				,(SizedInternalFormat)imageType.GlInternalFormat
				,(InternalFormatParameter)All.NumVirtualPageSizesArb
				,1
				,indexCount);
			var status = GL.GetError ();
			if (status != GL.GetError ())
			{
				throw new Exception (status.ToString());
			}			
			
			for (int i = 0; i < indexCount[0]; ++i) 
			{
				GL.Ext.TextureParameter(textureId, glTarget, (TextureParameterName)All.VirtualPageSizeIndexArb, i);
				status = GL.GetError ();
				if (status != GL.GetError ())
				{
					throw new Exception (status.ToString());
				}

				GL.GetInternalformat (
					(ImageTarget)glTarget
					,(SizedInternalFormat) imageType.GlInternalFormat
					,(InternalFormatParameter)All.VirtualPageSizeXArb
					,1
					,out xSize);
				status = GL.GetError ();
				if (status != GL.GetError ())
				{
					throw new Exception (status.ToString());
				}

				GL.GetInternalformat (
					(ImageTarget)glTarget
					,(SizedInternalFormat) imageType.GlInternalFormat
					,(InternalFormatParameter)All.VirtualPageSizeYArb
					,1
					,out ySize);
				status = GL.GetError ();
				if (status != GL.GetError ())
				{
					throw new Exception (status.ToString());
				}				

				GL.GetInternalformat (
					(ImageTarget)glTarget
					,(SizedInternalFormat) imageType.GlInternalFormat
					,(InternalFormatParameter)All.VirtualPageSizeZArb
					,1
					,out zSize);
				status = GL.GetError ();
				if (status != GL.GetError ())
				{
					throw new Exception (status.ToString());
				}				

				// For our purposes, the "best" format is the one that winds up with Z=1 and the largest x and y sizes.
				if (zSize == 1) {
					if (xSize >= bestXSize && ySize >= bestYSize) {
						bestIndex = i;
						bestXSize = xSize;
						bestYSize = ySize;
					}
				}
			}

			// This would mean the implementation has no valid sizes for us, or that this format doesn't actually support sparse
			// texture allocation. Need to implement the fallback. TODO: Implement that.
			if (bestIndex == -1)
			{
				throw new Exception ("bestIndex != -1");
			}

			GL.Ext.TextureParameter (textureId, glTarget, (TextureParameterName)All.VirtualPageSizeIndexArb, bestIndex);
			status = GL.GetError ();
			if (status != GL.GetError ())
			{
				throw new Exception ("Invalid TextureMinFilter value");
			}

			int maxPages;
			GL.GetInteger ((GetPName) All.MaxSparseArrayTextureLayers, out maxPages);
			status = GL.GetError ();
			if (status != GL.GetError ())
			{
				throw new Exception (" Invalid GetInteger value");
			}

			// We've set all the necessary parameters, now it's time to create the sparse texture.
			GL.Ext.TextureStorage3D (
				textureId
				, (ExtDirectStateAccess)glTarget
				, imageType.NoOfMipmapLevels
				, (ExtDirectStateAccess)imageType.GlInternalFormat
				, dims.Width
				, dims.Height
				, maxPages);
			status = GL.GetError ();
			if (status != GL.GetError ())
			{
				throw new Exception ("Invalid TextureMinFilter value");
			}


			GL.Ext.TextureParameter (textureId, glTarget, TextureParameterName.TextureMinFilter, catalog.MinFilter);
			status = GL.GetError ();
			if (status != GL.GetError ())
			{
				throw new Exception ("Invalid TextureMinFilter value");
			}

			GL.Ext.TextureParameter (textureId, glTarget, TextureParameterName.TextureMagFilter, catalog.MagFilter);
			status = GL.GetError ();
			if (status != GL.GetError ())
			{
				throw new Exception ("Invalid TextureMagFilter value");
			}

			GL.Ext.TextureParameter (textureId, glTarget, TextureParameterName.TextureWrapS, catalog.TextureWrapS);
			status = GL.GetError ();
			if (status != GL.GetError ())
			{
				throw new Exception ("Invalid TextureWrapS value");
			}

			GL.Ext.TextureParameter (textureId, glTarget, TextureParameterName.TextureWrapT, catalog.TextureWrapT);
			status = GL.GetError ();
			if (status != GL.GetError ())
			{
				throw new Exception (" Invalid TextureWrapT value");
			}

			long texHandle = 0;
			//long texHandle = GL.Arb.GetTextureHandle (textureId);
//			status = GL.GetError ();
//			if (status != GL.GetError ())
//			{
//				throw new Exception (" Invalid GetTextureHandle value");
//			}



			if (glTarget == TextureTarget.Texture1DArray || glTarget == TextureTarget.Texture2DArray)
			{								
				var chapter = new SparseTextureChapter (mLookup, mSparsePageAllocator);
				chapter.Initialise (catalog, imageType, dims, textureId, texHandle, maxPages);
				return chapter;
			} 
			else if (glTarget == TextureTarget.TextureCubeMapArray)
			{
				var chapter = new SparseCubeMapChapter (mLookup, mCubeMapPageAllocator);
				chapter.Initialise (catalog, imageType, dims, textureId, texHandle, maxPages);
				return chapter;	
			}
			else
			{
				var chapter = new SingleTextureChapter (mLookup, mSinglePageAllocator);
				chapter.Initialise (catalog, imageType, dims, textureId, texHandle);
				return chapter;
			}
		}
		#endregion
	}
}

