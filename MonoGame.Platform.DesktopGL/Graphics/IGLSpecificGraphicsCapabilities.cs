using System;

namespace Microsoft.Xna.Framework.DesktopGL
{
	public interface IGLSpecificGraphicsCapabilities
	{
		#if OPENGL
		/// <summary>
		/// True, if GL_ARB_framebuffer_object is supported; false otherwise.
		/// </summary>
		bool SupportsFramebufferObjectARB { get; }

		/// <summary>
		/// True, if GL_EXT_framebuffer_object is supported; false otherwise.
		/// </summary>
		bool SupportsFramebufferObjectEXT { get;  }

		/// <summary>
		/// Gets the max texture anisotropy. This value typically lies
		/// between 0 and 16, where 0 means anisotropic filtering is not
		/// supported.
		/// </summary>
		int MaxTextureAnisotropy { get; }
		#endif
	}
}

