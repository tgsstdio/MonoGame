using Microsoft.Xna.Framework.Graphics;

using System;
#if OPENGL
#if MONOMAC
using MonoMac.OpenGL;
#elif GLES
using OpenTK.Graphics.ES20;
#else
using OpenTK.Graphics.OpenGL;
#endif
#endif

namespace Microsoft.Xna.Framework.DesktopGL.Graphics
{
	public class GLSpecificGraphicsCapabilities
	{
		public GLSpecificGraphicsCapabilities (GraphicsDevice device)
		{
			var SupportsTextureFilterAnisotropic = device._extensions.Contains("GL_EXT_texture_filter_anisotropic");

			// OpenGL framebuffer objects
			#if GLES
			SupportsFramebufferObjectARB = true; // always supported on GLES 2.0+
			SupportsFramebufferObjectEXT = false;
			#else
			SupportsFramebufferObjectARB = device._extensions.Contains("GL_ARB_framebuffer_object");
			SupportsFramebufferObjectEXT = device._extensions.Contains("GL_EXT_framebuffer_object");
			#endif

			// Anisotropic filtering
			#if OPENGL
			int anisotropy = 0;
			if (SupportsTextureFilterAnisotropic)
			{
				GL.GetInteger((GetPName)All.MaxTextureMaxAnisotropyExt, out anisotropy);
				DesktopGLGraphicsExtensions.CheckGLError();
			}
			MaxTextureAnisotropy = anisotropy;
			#endif

		}

		#if OPENGL
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
		#endif
	}
}

