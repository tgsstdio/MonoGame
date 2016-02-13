using Microsoft.Xna.Framework.Graphics;
using System.Collections.Specialized;
using OpenTK.Graphics.ES20;
using MonoGame.Platform.DesktopGL.Graphics;

namespace MonoGame.Platform.DesktopGL
{
	public class Es20GLSpecificExtensionLookup : IGraphicsCapabilitiesLookup
	{
		public StringCollection Extensions {get; private set;}

		public void Initialise()
		{
			Extensions = new StringCollection();

			ProirToVersion3_0 ();
		}

		private void ProirToVersion3_0 ()
		{
			string extension_string = GL.GetString (StringName.Extensions);
			foreach (string extension in extension_string.Split (' '))
			{
				Extensions.Add (extension);
			}
		}

		#region IGraphicsCapabilitiesLookup implementation

		public bool SupportsNonPowerOfTwo ()
		{
			return Extensions.Contains("GL_OES_texture_npot") ||
				Extensions.Contains("GL_ARB_texture_non_power_of_two") ||
				Extensions.Contains("GL_IMG_texture_npot") ||
				Extensions.Contains("GL_NV_texture_npot_2D_mipmap");
		}

		public bool SupportsTextureMaxLevel ()
		{
			return Extensions.Contains("GL_APPLE_texture_max_level");
		}

		public bool SupportsTextureFilterAnisotropic ()
		{
			return Extensions.Contains("GL_EXT_texture_filter_anisotropic");
		}

		public bool SupportsDepth24 ()
		{
			return Extensions.Contains("GL_OES_depth24");
		}

		public bool SupportsPackedDepthStencil ()
		{
			return Extensions.Contains("GL_OES_packed_depth_stencil");
		}

		public bool SupportsDepthNonLinear ()
		{
			return Extensions.Contains("GL_NV_depth_nonlinear");
		}

		public bool SupportsS3tc ()
		{
			return Extensions.Contains("GL_EXT_texture_compression_s3tc") ||
				Extensions.Contains("GL_OES_texture_compression_S3TC") ||
				Extensions.Contains("GL_EXT_texture_compression_dxt3") ||
				Extensions.Contains("GL_EXT_texture_compression_dxt5");
		}

		public bool SupportsDxt1 ()
		{
			return SupportsS3tc() || Extensions.Contains("GL_EXT_texture_compression_dxt1");
		}

		public bool SupportsPvrtc ()
		{
			return Extensions.Contains("GL_IMG_texture_compression_pvrtc");
		}

		public bool SupportsEtc1 ()
		{
			return Extensions.Contains("GL_OES_compressed_ETC1_RGB8_texture");
		}

		public bool SupportsAtitc ()
		{
			return Extensions.Contains("GL_ATI_texture_compression_atitc") ||
				Extensions.Contains("GL_AMD_compressed_ATC_texture");
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
			return Extensions.Contains("GL_EXT_sRGB");
		}

		public bool SupportsTextureArrays ()
		{
			return false;
		}

		public bool SupportsDepthClamp ()
		{
			return Extensions.Contains("GL_ARB_depth_clamp");
		}

		public bool SupportsVertexTextures ()
		{
			return false;
		}

		#endregion
	}
}

