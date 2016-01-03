// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

#if MONOMAC
using MonoMac.OpenGL;
#elif DESKTOPGL
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
#elif GLES
using OpenTK.Graphics.ES20;
#endif

namespace MonoGame.Platform.DesktopGL.Graphics
{
	public class RenderTarget2DPlatform : IRenderTarget2DPlatform
    {
        internal int glColorBuffer;
        internal int glDepthBuffer;
        internal int glStencilBuffer;

		private readonly IThreadingContext mThreadContext;
		private readonly IGraphicsDevice mDevice;
		public RenderTarget2DPlatform (IThreadingContext instance, IGraphicsDevice device)
		{
			mThreadContext = instance;
			mDevice = device;
		}


        public void Construct(IGraphicsDevice graphicsDevice, int width, int height, bool mipMap,
            SurfaceFormat preferredFormat, DepthFormat preferredDepthFormat, int preferredMultiSampleCount, RenderTargetUsage usage, bool shared)
        {
			mThreadContext.BlockOnUIThread(() =>
            {
				mDevice.PlatformCreateRenderTarget(this, width, height, mipMap, preferredFormat, preferredDepthFormat, preferredMultiSampleCount, usage);
            });            
            
        }

        public void GraphicsDeviceResetting()
        {
        }

		private bool mIsDisposed = false;
        public void Dispose(bool disposing)
        {
            if (!mIsDisposed)
            {
				mThreadContext.BlockOnUIThread(() =>
                {
                    this.GraphicsDevice.PlatformDeleteRenderTarget(this);
                });
				mIsDisposed = true;				
            }
        }
    }
}
