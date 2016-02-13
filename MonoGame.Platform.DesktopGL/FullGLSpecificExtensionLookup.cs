using System.Collections.Specialized;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Platform.DesktopGL.Graphics;
using OpenTK.Graphics.OpenGL;

namespace MonoGame.Platform.DesktopGL
{
	public class FullGLSpecificExtensionLookup : IGraphicsCapabilitiesLookup
	{
		#region IGraphicsCapabilitiesLookup implementation
		private IGraphicsDevice mGraphicsDevice;
		public FullGLSpecificExtensionLookup (IGraphicsDevice device)
		{
			mGraphicsDevice = device;
		}

		public StringCollection Extensions {get; private set;}

		private int mMaxTextureSize;
		public void Initialise()
		{
			Extensions = new StringCollection();
			AfterVersion3_0 ();

			ProirToVersion3_0 ();

			int _maxTextureSize;
			GL.GetInteger(GetPName.MaxTextureSize, out _maxTextureSize);
			GraphicsExtensions.CheckGLError();
			mMaxTextureSize = _maxTextureSize;
		}

		public bool HasExtension (string extension)
		{
			return Extensions.Contains(extension);
		}

		void AfterVersion3_0 ()
		{
			int count = GL.GetInteger (GetPName.NumExtensions);
			for (int i = 0; i < count; i++)
			{
				string extension = GL.GetString (StringNameIndexed.Extensions, i);
				Extensions.Add (extension);
			}
		}

		private void ProirToVersion3_0 ()
		{
			string extension_string = GL.GetString (StringName.Extensions);
			foreach (string extension in extension_string.Split (' '))
			{
				Extensions.Add (extension);
			}
		}

		public bool SupportsNonPowerOfTwo ()
		{
            // Unfortunately non PoT texture support is patchy even on desktop systems and we can't
            // rely on the fact that GL2.0+ supposedly supports npot in the core.
            // Reference: http://aras-p.info/blog/2012/10/17/non-power-of-two-textures/
			return mMaxTextureSize >= 8192;
		}

		public bool SupportsTextureMaxLevel ()
		{
			return true;
		}

		public bool SupportsTextureFilterAnisotropic ()
		{
			return Extensions.Contains("GL_EXT_texture_filter_anisotropic");
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
			return  Extensions.Contains("GL_EXT_texture_compression_s3tc") ||
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
			return Extensions.Contains("GL_ARB_framebuffer_object");
		}

		public bool SupportsFramebufferObjectEXT ()
		{
			return Extensions.Contains("GL_EXT_framebuffer_object");
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
			return Extensions.Contains("GL_EXT_texture_sRGB") && Extensions.Contains("GL_EXT_framebuffer_sRGB");
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
			return Extensions.Contains("GL_ARB_depth_clamp");
		}

		public bool SupportsVertexTextures ()
		{
			return false;
		}

		#endregion
	}
}

