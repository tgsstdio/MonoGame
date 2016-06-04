using System;
using MonoGame.Graphics;
using MonoGame.Core;
using MonoGame.Platform.AndroidGL.Graphics;

namespace MonoGame.Platform.AndroidGL
{
	public class Es20GLFramebufferHelperSelector : IGLFramebufferHelperSelector
	{
		private readonly IGraphicsCapabilities mCapabilities;
		private readonly IGLExtensionLookup mLookup;
		public Es20GLFramebufferHelperSelector  (IGraphicsCapabilities capabilities, IGLExtensionLookup lookup)
		{
			mCapabilities = capabilities;
			mLookup = lookup;
		}

		#region IGLFramebufferHelperSelector implementation

		public void Initialize ()
		{
			if (mCapabilities.SupportsFramebufferObjectARB)
			{
				Helper = new AndroidGLFramebufferHelper(mLookup);
			}
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

