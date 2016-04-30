// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Core;

namespace MonoGame.Graphics
{
	public partial class MgTexture2D : MgBaseTexture, ITexture2D
    {
		internal int width;
		internal int height;
        internal int ArraySize;

        public Rectangle Bounds
        {
            get
            {
				return new Rectangle(0, 0, this.width, this.height);
            }
        }

		private ITexture2DPlatform mTex2DPlatform;
		public MgTexture2D(
			Int32 sortingKey,
			IMgTexturePlatform texPlatform,
			ITexture2DPlatform tex2DPlatform,
			IWeakReferenceCollection owner,
			IGraphicsCapabilities capability,
			int width, int height)
			: this(sortingKey, texPlatform, tex2DPlatform, owner, capability, width, height, false, SurfaceFormat.Color, SurfaceType.Texture, false, 1)
        {
        }

		public MgTexture2D(
			Int32 sortingKey,
			IMgTexturePlatform texPlatform,
			ITexture2DPlatform tex2DPlatform,
			IWeakReferenceCollection owner,
			IGraphicsCapabilities capabilities,
			int width, int height, bool mipmap, SurfaceFormat format)
			: this(sortingKey, texPlatform, tex2DPlatform, owner, capabilities, width, height, mipmap, format, SurfaceType.Texture, false, 1)
        {
        }

		public MgTexture2D(
			Int32 sortingKey,			
			IMgTexturePlatform texPlatform,
			ITexture2DPlatform tex2DPlatform,
			IWeakReferenceCollection owner,
			IGraphicsCapabilities capabilities,
			int width, int height, bool mipmap, SurfaceFormat format, int arraySize)
			: this(sortingKey, texPlatform, tex2DPlatform, owner, capabilities , width, height, mipmap, format, SurfaceType.Texture, false, arraySize)
        {
            
        }

		internal MgTexture2D(
			Int32 sortingKey,
			IMgTexturePlatform texPlatform,
			ITexture2DPlatform tex2DPlatform,
			IWeakReferenceCollection owner,
			IGraphicsCapabilities capabilities,
			int width, int height, bool mipmap, SurfaceFormat format, SurfaceType type)
			: this(sortingKey, texPlatform, tex2DPlatform, owner,capabilities, width, height, mipmap, format, type, false, 1)
        {
        }

		private IGraphicsCapabilities mCapabilities;
		protected MgTexture2D(
			Int32 sortingKey,
			IMgTexturePlatform texPlatform,
			ITexture2DPlatform tex2DPlatform,
			IWeakReferenceCollection owner,
			IGraphicsCapabilities capability,
			int width, int height, bool mipmap,
			SurfaceFormat format, SurfaceType type, bool shared, int arraySize)
			: base(sortingKey, texPlatform)
		{
			mTex2DPlatform = tex2DPlatform;
//            if (owner == null)
//            {
//				throw new ArgumentNullException("owner", FrameworkResources.ResourceCreationWhenDeviceIsNull);
//            }
			mCapabilities = capability;

			if (arraySize > 1 && ! mCapabilities.SupportsTextureArrays)
                throw new ArgumentException("Texture arrays are not supported on this graphics device", "arraySize");

            //this.Owner = owner;
            this.width = width;
            this.height = height;
            this._format = format;
			this._levelCount = mipmap ? CalculateMipLevels(width, height) : 1;
            this.ArraySize = arraySize;

            // Texture will be assigned by the swap chain.
		    if (type == SurfaceType.SwapChainRenderTarget)
		        return;
			// Add proper format
			mTex2DPlatform.Construct(width, height, mipmap, 0, type, shared);
        }

        public int Width
        {
            get
            {
                return width;
            }
        }

        public int Height
        {
            get
            {
                return height;
            }
        }

		public void SetData<T>(int level, int arraySlice, Rectangle? rect, T[] data, int startIndex, int elementCount)
			where T : struct
        {
            if (data == null)
                throw new ArgumentNullException("data");

			if (arraySlice > 0 && !mCapabilities.SupportsTextureArrays)
                throw new ArgumentException("Texture arrays are not supported on this graphics device", "arraySlice");

			mTex2DPlatform.SetData<T>(level, arraySlice, rect, data, startIndex, elementCount);
        }

		public void SetData<T>(int level, Rectangle? rect, T[] data, int startIndex, int elementCount)
			where T : struct 
        {
            this.SetData(level, 0, rect, data, startIndex, elementCount);
        }

		public void SetData<T>(T[] data, int startIndex, int elementCount) where T : struct
        {
            this.SetData(0, null, data, startIndex, elementCount);
        }
		
		public void SetData<T>(T[] data) where T : struct
        {
			this.SetData(0, null, data, 0, data.Length);
        }

		public void GetData<T>(int level, int arraySlice, Rectangle? rect, T[] data, int startIndex, int elementCount)
			where T : struct
        {
            if (data == null || data.Length == 0)
                throw new ArgumentException("data cannot be null");
            if (data.Length < startIndex + elementCount)
                throw new ArgumentException("The data passed has a length of " + data.Length + " but " + elementCount + " pixels have been requested.");
			if (arraySlice > 0 && !mCapabilities.SupportsTextureArrays)
                throw new ArgumentException("Texture arrays are not supported on this graphics device", "arraySlice");

			mTex2DPlatform.GetData<T>(level, arraySlice, rect, data, startIndex, elementCount);
        }
		
		public void GetData<T>(int level, Rectangle? rect, T[] data, int startIndex, int elementCount) 
			where T : struct
        {
            this.GetData(level, 0, rect, data, startIndex, elementCount);
        }

		public void GetData<T>(T[] data, int startIndex, int elementCount) where T : struct
		{
			this.GetData(0, null, data, startIndex, elementCount);
		}
		
		public void GetData<T> (T[] data) where T : struct
		{
			this.GetData(0, null, data, 0, data.Length);
		}
		
//		public static Texture2D FromStream(GraphicsDevice graphicsDevice, Stream stream)
//		{
//			return mTex2DPlatform.FromStream(graphicsDevice, stream);
//        }

        public void SaveAsJpeg(Stream stream, int width, int height)
        {
			mTex2DPlatform.SaveAsJpeg(stream, width, height);
        }

        public void SaveAsPng(Stream stream, int width, int height)
        {
			mTex2DPlatform.SaveAsPng(stream, width, height);
        }

        // This method allows games that use Texture2D.FromStream 
        // to reload their textures after the GL context is lost.
		public void Reload(Stream textureStream)
        {
			//mTex2DPlatform.Reload(textureStream);
        }

        //Converts Pixel Data from ARGB to ABGR
        private static void ConvertToABGR(int pixelHeight, int pixelWidth, int[] pixels)
        {
            int pixelCount = pixelWidth * pixelHeight;
            for (int i = 0; i < pixelCount; ++i)
            {
                uint pixel = (uint)pixels[i];
                pixels[i] = (int)((pixel & 0xFF00FF00) | ((pixel & 0x00FF0000) >> 16) | ((pixel & 0x000000FF) << 16));
            }
        }
	}
}

