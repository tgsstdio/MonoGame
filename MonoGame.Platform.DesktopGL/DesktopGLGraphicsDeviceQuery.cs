using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Platform.DesktopGL
{
	public class DesktopGLGraphicsDeviceQuery : IGraphicsDeviceQuery
	{
		private readonly IBackBufferPreferences mBackbuffer;
		private readonly PresentationParameters mPresentation;

		public DesktopGLGraphicsDeviceQuery (IBackBufferPreferences backbuffer, PresentationParameters presentation)
		{
			mBackbuffer = backbuffer;
			mPresentation = presentation;
			mPresentation.IsFullScreen = false;
			PreferredBackBufferHeight = mBackbuffer.DefaultBackBufferHeight;
			PreferredBackBufferWidth = mBackbuffer.DefaultBackBufferWidth;			
		}

		#region IGraphicsDeviceQuery implementation

		public int PreferredBackBufferHeight {
			get;
			set;
		}

		public int PreferredBackBufferWidth {
			get;
			set;
		}


		public bool IsFullScreen
		{
			get
			{
				return mPresentation.IsFullScreen;
			}
			set
			{
				mPresentation.IsFullScreen = value;
			}
		}

		#endregion
	}
}

