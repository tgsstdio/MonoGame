// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using MonoGame.Core;

namespace Microsoft.Xna.Framework.Graphics
{
	public partial class RenderTarget2D : IRenderTarget2D
	{
		public void Reload (System.IO.Stream textureStream)
		{
			throw new NotImplementedException ();
		}

		public void SaveAsPng (System.IO.Stream stream, int width, int height)
		{
			throw new NotImplementedException ();
		}

		public void SaveAsJpeg (System.IO.Stream stream, int width, int height)
		{
			throw new NotImplementedException ();
		}

		public void SetData<T> (T[] data) where T : struct
		{
			throw new NotImplementedException ();
		}

		public void SetData<T> (T[] data, int startIndex, int elementCount) where T : struct
		{
			throw new NotImplementedException ();
		}

		public void SetData<T> (int level, Rectangle? rect, T[] data, int startIndex, int elementCount) where T : struct
		{
			throw new NotImplementedException ();
		}

		public void SetData<T> (int level, int arraySlice, Rectangle? rect, T[] data, int startIndex, int elementCount) where T : struct
		{
			throw new NotImplementedException ();
		}

		public void GetData<T> (T[] data) where T : struct
		{
			throw new NotImplementedException ();
		}

		public void GetData<T> (T[] data, int startIndex, int elementCount) where T : struct
		{
			throw new NotImplementedException ();
		}

		public void GetData<T> (int level, Rectangle? rect, T[] data, int startIndex, int elementCount) where T : struct
		{
			throw new NotImplementedException ();
		}

		public void GetData<T> (int level, int arraySlice, Rectangle? rect, T[] data, int startIndex, int elementCount) where T : struct
		{
			throw new NotImplementedException ();
		}

		public int Width {
			get {
				throw new NotImplementedException ();
			}
		}

		public int Height {
			get {
				throw new NotImplementedException ();
			}
		}

		public Rectangle Bounds {
			get {
				throw new NotImplementedException ();
			}
		}

		public int SortingKey {
			get {
				throw new NotImplementedException ();
			}
		}

		public SurfaceFormat Format {
			get {
				throw new NotImplementedException ();
			}
		}

		public int LevelCount {
			get {
				throw new NotImplementedException ();
			}
		}

		public DepthFormat DepthStencilFormat { get; private set; }
		
		public int MultiSampleCount { get; private set; }
		
		public RenderTargetUsage RenderTargetUsage { get; private set; }
		
		public bool IsContentLost { get { return false; } }
		
		public event EventHandler<EventArgs> ContentLost;
		
        private bool SuppressEventHandlerWarningsUntilEventsAreProperlyImplemented()
        {
            return ContentLost != null;
        }

	    public RenderTarget2D(
			Int32 baseTextureKey,
			ITexture2DPlatform tex2D,
			IRenderTarget2DPlatform platform,
			IWeakReferenceCollection owner,
			IGraphicsCapabilities capabilities,
			int width, int height, bool mipMap, SurfaceFormat preferredFormat, DepthFormat preferredDepthFormat, int preferredMultiSampleCount, RenderTargetUsage usage, bool shared, int arraySize)
		//	: base(baseTexture, tex2D, owner, capabilities, width, height, mipMap, preferredFormat, SurfaceType.RenderTarget, shared, arraySize)
	    {
			mPlatform = platform;

            DepthStencilFormat = preferredDepthFormat;
            MultiSampleCount = preferredMultiSampleCount;
            RenderTargetUsage = usage;

			mPlatform.Construct(owner, width, height, mipMap, preferredFormat, preferredDepthFormat, preferredMultiSampleCount, usage, shared);
	    }

		public RenderTarget2D (
			Int32 baseTextureKey,
			ITexture2DPlatform tex2D,
			IRenderTarget2DPlatform platform,
			IWeakReferenceCollection owner,
			IGraphicsCapabilities capabilities,
			int width, int height, bool mipMap, SurfaceFormat preferredFormat, DepthFormat preferredDepthFormat, int preferredMultiSampleCount, RenderTargetUsage usage, bool shared)
			: this(baseTextureKey, tex2D, platform, owner, capabilities, width, height, mipMap, preferredFormat, preferredDepthFormat, preferredMultiSampleCount, usage, shared, 1)
        {
			
        }

		public RenderTarget2D (	
			Int32 baseTextureKey,
			ITexture2DPlatform tex2D,
			IRenderTarget2DPlatform platform,
			IWeakReferenceCollection owner,
			IGraphicsCapabilities capabilities,
			int width, int height, bool mipMap, SurfaceFormat preferredFormat, DepthFormat preferredDepthFormat, int preferredMultiSampleCount, RenderTargetUsage usage)
			:this (baseTextureKey, tex2D, platform, owner, capabilities, width, height, mipMap, preferredFormat, preferredDepthFormat, preferredMultiSampleCount, usage, false)
        {}

		public RenderTarget2D(
			Int32 baseTextureKey,
			ITexture2DPlatform tex2D,
			IRenderTarget2DPlatform platform,
			IWeakReferenceCollection owner,
			IGraphicsCapabilities capabilities,
			int width, int height, bool mipMap, SurfaceFormat preferredFormat, DepthFormat preferredDepthFormat)
			:this (baseTextureKey, tex2D, platform, owner, capabilities, width, height, mipMap, preferredFormat, preferredDepthFormat, 0, RenderTargetUsage.DiscardContents) 
		{}
		
		public RenderTarget2D(
			Int32 baseTextureKey,
			ITexture2DPlatform tex2D,
			IRenderTarget2DPlatform platform,
			IWeakReferenceCollection owner,
			IGraphicsCapabilities capabilities,
			int width, int height)
			: this(baseTextureKey, tex2D, platform, owner, capabilities, width, height, false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.DiscardContents) 
		{}

		private IRenderTarget2DPlatform mPlatform;

        /// <summary>
        /// Allows child class to specify the surface type, eg: a swap chain.
        /// </summary>        
        protected RenderTarget2D(
			Int32 baseTextureKey,
			ITexture2DPlatform tex2D,
			IRenderTarget2DPlatform platform,
			IWeakReferenceCollection owner,
			IGraphicsCapabilities capabilities,
            int width,
            int height,
            bool mipMap,
            SurfaceFormat format,
            DepthFormat depthFormat,
            int preferredMultiSampleCount,
            RenderTargetUsage usage,
            SurfaceType surfaceType)
		//	: base(baseTexture, tex2D, owner, capabilities, width, height, mipMap, format, surfaceType)
        {
			mPlatform = platform;
            DepthStencilFormat = depthFormat;
            MultiSampleCount = preferredMultiSampleCount;
            RenderTargetUsage = usage;
		}

        public void GraphicsDeviceResetting()
        {
			mPlatform.GraphicsDeviceResetting();
            base.GraphicsDeviceResetting();
        }
	}
}
