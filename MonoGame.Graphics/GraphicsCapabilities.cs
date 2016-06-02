// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.Collections.Generic;
using MonoGame.Graphics;
using MonoGame.Core;


#if OPENGL
#if MONOMAC
using MonoMac.OpenGL;
#elif GLES
using OpenTK.Graphics.ES20;
#else
using OpenTK.Graphics.OpenGL;
#endif
#endif

namespace MonoGame.Graphics
{
    /// <summary>
    /// Provides information about the capabilities of the
    /// current graphics device. A very useful thread for investigating GL extenion names
    /// http://stackoverflow.com/questions/3881197/opengl-es-2-0-extensions-on-android-devices
    /// </summary>
	public class GraphicsCapabilities : IGraphicsCapabilities
    {
		private readonly IGraphicsCapabilitiesLookup mLookup;
		public GraphicsCapabilities(IGraphicsCapabilitiesLookup lookup)
        {
			mLookup = lookup;
        }

        /// <summary>
        /// Whether the device fully supports non power-of-two textures, including
        /// mip maps and wrap modes other than CLAMP_TO_EDGE
        /// </summary>
        public bool SupportsNonPowerOfTwo { get; private set; }

        /// <summary>
        /// Whether the device supports anisotropic texture filtering
        /// </summary>
		public bool SupportsTextureFilterAnisotropic { get; private set; }

		public bool SupportsDepth24 { get; private set; }

		public bool SupportsPackedDepthStencil { get; private set; }

		public bool SupportsDepthNonLinear { get; private set; }

        /// <summary>
        /// Gets the support for DXT1
        /// </summary>
		public bool SupportsDxt1 { get; private set; }

        /// <summary>
        /// Gets the support for S3TC (DXT1, DXT3, DXT5)
        /// </summary>
		public bool SupportsS3tc { get; private set; }

        /// <summary>
        /// Gets the support for PVRTC
        /// </summary>
		public bool SupportsPvrtc { get; private set; }

        /// <summary>
        /// Gets the support for ETC1
        /// </summary>
		public bool SupportsEtc1 { get; private set; }

        /// <summary>
        /// Gets the support for ATITC
        /// </summary>
		public bool SupportsAtitc { get; private set; }

//#if OPENGL
        /// <summary>
        /// True, if GL_ARB_framebuffer_object is supported; false otherwise.
        /// </summary>
        public bool SupportsFramebufferObjectARB { get; private set; }

        /// <summary>
        /// True, if GL_EXT_framebuffer_object is supported; false otherwise.
        /// </summary>
		public bool SupportsFramebufferObjectEXT { get; private set; }

        /// <summary>
        /// Gets the max texture anisotropy. This value typically lies
        /// between 0 and 16, where 0 means anisotropic filtering is not
        /// supported.
        /// </summary>
		public int MaxTextureAnisotropy { get; private set; }
//#endif

		public bool SupportsTextureMaxLevel { get; private set; }

        /// <summary>
        /// True, if sRGB is supported. On Direct3D platforms, this is always <code>true</code>.
        /// On OpenGL platforms, it is <code>true</code> if both framebuffer sRGB
        /// and texture sRGB are supported.
        /// </summary>
		public bool SupportsSRgb { get; private set; }
        
		public bool SupportsTextureArrays { get; private set; }

		public bool SupportsDepthClamp { get; private set; }

		public bool SupportsVertexTextures { get; private set; }

		public void Initialize()
        {
			SupportsNonPowerOfTwo = mLookup.SupportsNonPowerOfTwo();

#if OPENGL
            SupportsTextureFilterAnisotropic = device._extensions.Contains("GL_EXT_texture_filter_anisotropic");
#else
			//  SupportsTextureFilterAnisotropic = true;
			SupportsTextureFilterAnisotropic = mLookup.SupportsTextureFilterAnisotropic();
#endif
#if GLES
			SupportsDepth24 = device._extensions.Contains("GL_OES_depth24");
			SupportsPackedDepthStencil = device._extensions.Contains("GL_OES_packed_depth_stencil");
			SupportsDepthNonLinear = device._extensions.Contains("GL_NV_depth_nonlinear");
            SupportsTextureMaxLevel = device._extensions.Contains("GL_APPLE_texture_max_level");
#else
//            SupportsDepth24 = true;
//			SupportsPackedDepthStencil = true;
//			SupportsDepthNonLinear = false;
//            SupportsTextureMaxLevel = true;

			SupportsDepth24 = mLookup.SupportsDepth24();
			SupportsPackedDepthStencil = mLookup.SupportsPackedDepthStencil();
			SupportsDepthNonLinear = mLookup.SupportsDepthNonLinear();
			SupportsTextureMaxLevel = mLookup.SupportsTextureMaxLevel();
#endif

            // Texture compression
#if DIRECTX
            SupportsDxt1 = true;
            SupportsS3tc = true;
#elif OPENGL
            SupportsS3tc = device._extensions.Contains("GL_EXT_texture_compression_s3tc") ||
                device._extensions.Contains("GL_OES_texture_compression_S3TC") ||
                device._extensions.Contains("GL_EXT_texture_compression_dxt3") ||
                device._extensions.Contains("GL_EXT_texture_compression_dxt5");
            SupportsDxt1 = SupportsS3tc || device._extensions.Contains("GL_EXT_texture_compression_dxt1");
            SupportsPvrtc = device._extensions.Contains("GL_IMG_texture_compression_pvrtc");
            SupportsEtc1 = device._extensions.Contains("GL_OES_compressed_ETC1_RGB8_texture");
            SupportsAtitc = device._extensions.Contains("GL_ATI_texture_compression_atitc") ||
                device._extensions.Contains("GL_AMD_compressed_ATC_texture");
#endif
			SupportsS3tc = mLookup.SupportsS3tc ();
			SupportsDxt1 = SupportsS3tc || mLookup.SupportsDxt1();
			SupportsPvrtc = mLookup.SupportsPvrtc();
			SupportsEtc1 = mLookup.SupportsEtc1 ();
			SupportsAtitc = mLookup.SupportsAtitc ();

            // OpenGL framebuffer objects
#if OPENGL
#if GLES
            SupportsFramebufferObjectARB = true; // always supported on GLES 2.0+
            SupportsFramebufferObjectEXT = false;
#else
            SupportsFramebufferObjectARB = device._extensions.Contains("GL_ARB_framebuffer_object");
            SupportsFramebufferObjectEXT = device._extensions.Contains("GL_EXT_framebuffer_object");
#endif
#endif
			SupportsFramebufferObjectARB = mLookup.SupportsFramebufferObjectARB ();
			SupportsFramebufferObjectEXT = mLookup.SupportsFramebufferObjectEXT ();


            // Anisotropic filtering
#if OPENGL
            int anisotropy = 0;
            if (SupportsTextureFilterAnisotropic)
            {
                GL.GetInteger((GetPName)All.MaxTextureMaxAnisotropyExt, out anisotropy);
                GraphicsExtensions.CheckGLError();
            }
            MaxTextureAnisotropy = anisotropy;
#endif
			MaxTextureAnisotropy = mLookup.GetMaxTextureAnisotropy();

            // sRGB
#if DIRECTX
            SupportsSRgb = true;
#elif OPENGL
#if GLES
            SupportsSRgb = device._extensions.Contains("GL_EXT_sRGB");
#else
            SupportsSRgb = device._extensions.Contains("GL_EXT_texture_sRGB") && device._extensions.Contains("GL_EXT_framebuffer_sRGB");
#endif
#endif
			SupportsSRgb = mLookup.SupportsSRgb();

#if DIRECTX
            SupportsTextureArrays = device.GraphicsProfile == GraphicsProfile.HiDef;
#elif OPENGL
            // TODO: Implement OpenGL support for texture arrays
            // once we can author shaders that use texture arrays.
            SupportsTextureArrays = false;
#endif
			SupportsTextureArrays = mLookup.SupportsTextureArrays();

#if DIRECTX
            SupportsDepthClamp = device.GraphicsProfile == GraphicsProfile.HiDef;
#elif OPENGL
            SupportsDepthClamp = device._extensions.Contains("GL_ARB_depth_clamp");
#endif
			SupportsTextureArrays = mLookup.SupportsDepthClamp();

#if DIRECTX
            SupportsVertexTextures = device.GraphicsProfile == GraphicsProfile.HiDef;
#elif OPENGL
            SupportsVertexTextures = false; // For now, until we implement vertex textures in OpenGL.
#endif

			SupportsVertexTextures = mLookup.SupportsVertexTextures ();
        }

//        bool GetNonPowerOfTwo(IGraphicsDevice device)
//        {
//#if OPENGL
//#if GLES
//            return device._extensions.Contains("GL_OES_texture_npot") ||
//                   device._extensions.Contains("GL_ARB_texture_non_power_of_two") ||
//                   device._extensions.Contains("GL_IMG_texture_npot") ||
//                   device._extensions.Contains("GL_NV_texture_npot_2D_mipmap");
//#else
//            // Unfortunately non PoT texture support is patchy even on desktop systems and we can't
//            // rely on the fact that GL2.0+ supposedly supports npot in the core.
//            // Reference: http://aras-p.info/blog/2012/10/17/non-power-of-two-textures/
//            return device._maxTextureSize >= 8192;
//#endif
//
//#else
//            return device.GraphicsProfile == GraphicsProfile.HiDef;
//#endif
//        }
    }
}