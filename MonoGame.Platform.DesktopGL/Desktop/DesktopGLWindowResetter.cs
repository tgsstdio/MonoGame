﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Core;

namespace MonoGame.Platform.DesktopGL
{
	public class DesktopGLWindowResetter : IOpenTKWindowResetter
	{
		private readonly IOpenTKGameWindow mWindow;
		private readonly IGraphicsDeviceQuery mDeviceQuery;
		private readonly IPresentationParameters mPresentation;
		private readonly IClientWindowBounds mClient;
		private bool isCurrentlyFullScreen = false;

		public DesktopGLWindowResetter (IOpenTKGameWindow window, IGraphicsDeviceQuery deviceQuery, IPresentationParameters presentation, IClientWindowBounds client)
		{
			mWindow = window;
			mDeviceQuery = deviceQuery;
			mPresentation = presentation;
			mClient = client;
		}

		public void ResetWindowBounds()
		{
			Rectangle bounds;

			bounds = mClient.ClientBounds;

			//Changing window style forces a redraw. Some games
			//have fail-logic and toggle fullscreen in their draw function,
			//so temporarily become inactive so it won't execute.

			//bool wasActive = IsActive;
			//IsActive = false;

			if (mPresentation.IsFullScreen)
			{
				bounds = new Rectangle(0, 0, mDeviceQuery.PreferredBackBufferWidth, mDeviceQuery.PreferredBackBufferHeight);

				if (OpenTK.DisplayDevice.Default.Width != mDeviceQuery.PreferredBackBufferWidth ||
					OpenTK.DisplayDevice.Default.Height != mDeviceQuery.PreferredBackBufferHeight)
				{
					OpenTK.DisplayDevice.Default.ChangeResolution(mDeviceQuery.PreferredBackBufferWidth,
						mDeviceQuery.PreferredBackBufferHeight,
						OpenTK.DisplayDevice.Default.BitsPerPixel,
						OpenTK.DisplayDevice.Default.RefreshRate);
				}
			}
			else
			{

				// switch back to the normal screen resolution
				OpenTK.DisplayDevice.Default.RestoreResolution();
				// now update the bounds 
				bounds.Width = mDeviceQuery.PreferredBackBufferWidth;
				bounds.Height = mDeviceQuery.PreferredBackBufferHeight;
			}


			// Now we set our Presentation Parameters
			// FIXME: Eliminate the need for null checks by only calling
			//        ResetWindowBounds after the device is ready.  Or,
			//        possibly break this method into smaller methods.
			mPresentation.BackBufferHeight = (int)bounds.Height;
			mPresentation.BackBufferWidth = (int)bounds.Width;

			if (mPresentation.IsFullScreen != isCurrentlyFullScreen)
			{                
				mWindow.ToggleFullScreen();
			}

			// we only change window bounds if we are not fullscreen
			// or if fullscreen mode was just entered
			if (!mPresentation.IsFullScreen || (mPresentation.IsFullScreen != isCurrentlyFullScreen))
				mWindow.ChangeClientBounds(bounds);

			// store the current fullscreen state
			isCurrentlyFullScreen = mPresentation.IsFullScreen;

			//IsActive = wasActive;
		}
	}
}

