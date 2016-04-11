using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Core
{
	public class DefaultGraphicsDeviceQuery : IGraphicsDeviceQuery
	{
		private readonly IBackBufferPreferences mBackbuffer;
		private readonly IPresentationParameters mPresentation;

		public DefaultGraphicsDeviceQuery (IBackBufferPreferences backbuffer, IPresentationParameters presentation)
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

