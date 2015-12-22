// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.
using Microsoft.Xna.Framework.Graphics;

#if MONOMAC
using MonoMac.OpenGL;
#elif DESKTOPGL
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
#elif GLES
using OpenTK.Graphics.ES20;
#endif

namespace Microsoft.Xna.Framework.DesktopGL.Graphics
{
	public class RenderTarget2DPlatform : IRenderTarget2DPlatform
    {
        internal int glColorBuffer;
        internal int glDepthBuffer;
        internal int glStencilBuffer;

        public void Construct(GraphicsDevice graphicsDevice, int width, int height, bool mipMap,
            SurfaceFormat preferredFormat, DepthFormat preferredDepthFormat, int preferredMultiSampleCount, RenderTargetUsage usage, bool shared)
        {
            Threading.BlockOnUIThread(() =>
            {
                graphicsDevice.Platform.CreateRenderTarget(this, width, height, mipMap, preferredFormat, preferredDepthFormat, preferredMultiSampleCount, usage);
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
                Threading.BlockOnUIThread(() =>
                {
                    this.GraphicsDevice.PlatformDeleteRenderTarget(this);
                });
				mIsDisposed = true;				
            }
        }
    }
}
