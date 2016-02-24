using MonoGame.Platform.DesktopGL.Graphics;
using OpenTK.Graphics.OpenGL;
using MonoGame.Graphics;

namespace MonoGame.Platform.DesktopGL
{
	public class FullGLSpecificGraphicsCapabilitiesLookup : IGraphicsCapabilitiesLookup
	{
		private IGLExtensionLookup mExtensions;
		public FullGLSpecificGraphicsCapabilitiesLookup (IGLExtensionLookup extensions)
		{
			mExtensions = extensions;
		}

		#region IGraphicsCapabilitiesLookup implementation

		public bool SupportsNonPowerOfTwo ()
		{
			int maxTextureSize;
			GL.GetInteger(GetPName.MaxTextureSize, out maxTextureSize);
			GraphicsExtensions.CheckGLError();

            // Unfortunately non PoT texture support is patchy even on desktop systems and we can't
            // rely on the fact that GL2.0+ supposedly supports npot in the core.
            // Reference: http://aras-p.info/blog/2012/10/17/non-power-of-two-textures/
			return maxTextureSize >= 8192;
		}

		public bool SupportsTextureMaxLevel ()
		{
			return true;
		}

		public bool SupportsTextureFilterAnisotropic ()
		{
			return mExtensions.HasExtension("GL_EXT_texture_filter_anisotropic");
		}

		public bool SupportsDepth24 ()
		{
			return true;
		}

		public bool SupportsPackedDepthStencil ()
		{
			return true;
		}

		public bool SupportsDepthNonLinear ()
		{
			return false;
		}

		public bool SupportsS3tc ()
		{
			return  mExtensions.HasExtension("GL_EXT_texture_compression_s3tc") ||
				mExtensions.HasExtension("GL_OES_texture_compression_S3TC") ||
				mExtensions.HasExtension("GL_EXT_texture_compression_dxt3") ||
				mExtensions.HasExtension("GL_EXT_texture_compression_dxt5");
		}

		public bool SupportsDxt1 ()
		{
			return SupportsS3tc() || mExtensions.HasExtension("GL_EXT_texture_compression_dxt1");
		}

		public bool SupportsPvrtc ()
		{
			return mExtensions.HasExtension("GL_IMG_texture_compression_pvrtc");
		}

		public bool SupportsEtc1 ()
		{
			return mExtensions.HasExtension("GL_OES_compressed_ETC1_RGB8_texture");
		}

		public bool SupportsAtitc ()
		{
			return mExtensions.HasExtension("GL_ATI_texture_compression_atitc") ||
				mExtensions.HasExtension("GL_AMD_compressed_ATC_texture");
		}

		public bool SupportsFramebufferObjectARB ()
		{
			return mExtensions.HasExtension("GL_ARB_framebuffer_object");
		}

		public bool SupportsFramebufferObjectEXT ()
		{
			return mExtensions.HasExtension("GL_EXT_framebuffer_object");
		}

		public int GetMaxTextureAnisotropy ()
		{
			int anisotropy = 0;
			if (SupportsTextureFilterAnisotropic())
			{
				GL.GetInteger((GetPName)All.MaxTextureMaxAnisotropyExt, out anisotropy);
				GraphicsExtensions.CheckGLError();
			}
			return anisotropy;
		}

		public bool SupportsSRgb ()
		{
			return mExtensions.HasExtension("GL_EXT_texture_sRGB") && mExtensions.HasExtension("GL_EXT_framebuffer_sRGB");
		}

		public bool SupportsTextureArrays ()
		{
			// TODO: Implement OpenGL support for texture arrays
			// once we can author shaders that use texture arrays.		

			// TODO: BIRDNEST - we have sparse texture array now
			return false;
		}

		public bool SupportsDepthClamp ()
		{
			return mExtensions.HasExtension("GL_ARB_depth_clamp");
		}

		public bool SupportsVertexTextures ()
		{
			return false;
		}

		#endregion
	}
}

