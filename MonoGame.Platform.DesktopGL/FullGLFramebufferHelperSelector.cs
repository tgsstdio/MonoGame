using System;
using MonoGame.Graphics;
using MonoGame.Platform.DesktopGL.Graphics;

namespace MonoGame.Platform.DesktopGL
{
	public class FullGLFramebufferHelperSelector : IGLFramebufferHelperSelector
	{
		private readonly IGraphicsCapabilities mCapabilities;
		public FullGLFramebufferHelperSelector (IGraphicsCapabilities capabilities)
		{
			mCapabilities = capabilities;
		}

		#region IGLFramebufferHelperSelector implementation

		public void Initialize ()
		{
			if (mCapabilities.SupportsFramebufferObjectARB)
			{
				Helper = new FullGLFramebufferHelper ();
			}
			//#if !(GLES || MONOMAC)
			else if (mCapabilities.SupportsFramebufferObjectEXT)
			{
				Helper = new FullGLFramebufferHelperEXT();
			}
			//#endif
			else
			{
				throw new PlatformNotSupportedException(
					"MonoGame requires either ARB_framebuffer_object or EXT_framebuffer_object." +
					"Try updating your graphics drivers.");
			}
		}

		public IGLFramebufferHelper Helper {
			get;
			private set;
		}

		#endregion
	}
}

