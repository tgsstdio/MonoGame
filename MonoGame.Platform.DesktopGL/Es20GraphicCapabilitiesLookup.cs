using MonoGame.Graphics;
using MonoGame.Platform.DesktopGL.Graphics;
using OpenTK.Graphics.ES20;

namespace MonoGame.Platform.DesktopGL
{
	public class Es20GraphicCapabilitiesLookup : IGraphicsCapabilitiesLookup
	{
		private IGLExtensionLookup mExtensions;
		public Es20GraphicCapabilitiesLookup (IGLExtensionLookup extensions)
		{
			mExtensions = extensions;
		}

		#region IGraphicsCapabilitiesLookup implementation

		public bool SupportsNonPowerOfTwo ()
		{
			return mExtensions.HasExtension("GL_OES_texture_npot") ||
				mExtensions.HasExtension("GL_ARB_texture_non_power_of_two") ||
				mExtensions.HasExtension("GL_IMG_texture_npot") ||
				mExtensions.HasExtension("GL_NV_texture_npot_2D_mipmap");
		}

		public bool SupportsTextureMaxLevel ()
		{
			return mExtensions.HasExtension("GL_APPLE_texture_max_level");
		}

		public bool SupportsTextureFilterAnisotropic ()
		{
			return mExtensions.HasExtension("GL_EXT_texture_filter_anisotropic");
		}

		public bool SupportsDepth24 ()
		{
			return mExtensions.HasExtension("GL_OES_depth24");
		}

		public bool SupportsPackedDepthStencil ()
		{
			return mExtensions.HasExtension("GL_OES_packed_depth_stencil");
		}

		public bool SupportsDepthNonLinear ()
		{
			return mExtensions.HasExtension("GL_NV_depth_nonlinear");
		}

		public bool SupportsS3tc ()
		{
			return mExtensions.HasExtension("GL_EXT_texture_compression_s3tc") ||
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
			return true;
		}

		public bool SupportsFramebufferObjectEXT ()
		{
			return false;
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
			return mExtensions.HasExtension("GL_EXT_sRGB");
		}

		public bool SupportsTextureArrays ()
		{
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

