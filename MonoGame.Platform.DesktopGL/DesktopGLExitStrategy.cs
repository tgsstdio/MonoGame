using System;
using Microsoft.Xna.Framework;

namespace MonoGame.Platform.DesktopGL
{
	public class DesktopGLExitStrategy : IWindowExitStrategy
	{
		private readonly IDrawSuppressor mSuppression;
		private readonly IGamePlatform mPlatform;
		public DesktopGLExitStrategy (IGamePlatform platform, IDrawSuppressor suppression)
		{
			mSuppression = suppression;
			mPlatform = platform;
		}

		public void Initialise()
		{
			mSuppression.AddBeforeExit (mPlatform.Exit);
		}
	}
}

