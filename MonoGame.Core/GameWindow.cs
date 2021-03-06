// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System;
using System.ComponentModel;
using MonoGame.Core;

namespace Microsoft.Xna.Framework {
	public abstract class GameWindow : IGameWindow {
		#region Properties

		[DefaultValue(false)]
		public abstract bool AllowUserResizing { get; set; }

		//public abstract Rectangle ClientBounds { get; }

		public bool _allowAltF4 = true;

        /// <summary>
        /// Gets or sets a bool that enables usage of Alt+F4 for window closing on desktop platforms. Value is true by default.
        /// </summary>
        public virtual bool AllowAltF4 { get { return _allowAltF4; } set { _allowAltF4 = value; } }
//
//#if (WINDOWS && !WINRT) || DESKTOPGL
//        /// <summary>
//        /// The location of this window on the desktop, eg: global coordinate space
//        /// which stretches across all screens.
//        /// </summary>
//        public abstract Point Position { get; set; }
//#endif
//
//#if DESKTOPGL
//        public abstract System.Drawing.Icon Icon { get; set; }
//#endif
//
		//public abstract DisplayOrientation CurrentOrientation { get; }

		public abstract IntPtr Handle { get; }

		public abstract string ScreenDeviceName { get; }

		private string _title;
		public string Title {
			get { return _title; }
			set {
				if (_title != value) {
					SetTitle(value);
					_title = value;
				}
			}
		}

        /// <summary>
        /// Determines whether the border of the window is visible. Currently only supported on the WinDX and WinGL/Linux platforms.
        /// </summary>
        /// <exception cref="System.NotImplementedException">
        /// Thrown when trying to use this property on a platform other than the WinDX and WinGL/Linux platforms.
        /// </exception>
        public virtual bool IsBorderless
        {
            get
            {
                return false;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

		public MouseState LastMouseState { get; set; }
	 //   internal TouchPanelState TouchPanelState;

		public ITouchListener Touch {
			get;
			private set;
		}

        protected GameWindow()
        {
            //TouchPanelState = new TouchPanelState(this);
        }

		#endregion Properties

		#region Events

		//public event EventHandler<EventArgs> ClientSizeChanged;
		public event EventHandler<EventArgs> OrientationChanged;
		public event EventHandler<EventArgs> ScreenDeviceNameChanged;

#if WINDOWS || WINDOWS_UAP || DESKTOPGL|| ANGLE

        /// <summary>
		/// Use this event to retrieve text for objects like textbox's.
		/// This event is not raised by noncharacter keys.
		/// This event also supports key repeat.
		/// For more information this event is based off:
		/// http://msdn.microsoft.com/en-AU/library/system.windows.forms.control.keypress.aspx
		/// </summary>
		/// <remarks>
		/// This event is only supported on the Windows DirectX, Windows OpenGL and Linux platforms.
		/// </remarks>
		public event EventHandler<TextInputEventArgs> TextInput;
#endif

		#endregion Events

		public abstract void BeginScreenDeviceChange (bool willBeFullScreen);

		public abstract void EndScreenDeviceChange (
			string screenDeviceName, int clientWidth, int clientHeight);

//		public void EndScreenDeviceChange (string screenDeviceName)
//		{
//			EndScreenDeviceChange(screenDeviceName, ClientBounds.Width, ClientBounds.Height);
//		}

		protected void OnActivated ()
		{
		}

//		protected void OnClientSizeChanged ()
//		{
//			if (ClientSizeChanged != null)
//				ClientSizeChanged (this, EventArgs.Empty);
//		}

		protected void OnDeactivated ()
		{
		}
         
		protected void OnOrientationChanged ()
		{
			if (OrientationChanged != null)
				OrientationChanged (this, EventArgs.Empty);
		}

		protected void OnPaint ()
		{
		}

		protected void OnScreenDeviceNameChanged ()
		{
			if (ScreenDeviceNameChanged != null)
				ScreenDeviceNameChanged (this, EventArgs.Empty);
		}

#if WINDOWS || WINDOWS_UAP || DESKTOPGL || ANGLE
		protected void OnTextInput(object sender, TextInputEventArgs e)
		{
			if (TextInput != null)
				TextInput(sender, e);
		}
#endif

		//public abstract void SetSupportedOrientations (DisplayOrientation orientations);
		protected abstract void SetTitle (string title);

#if DIRECTX && WINDOWS
        public static GameWindow Create(Game game, int width, int height)
        {
            var window = new MonoGame.Framework.WinFormsGameWindow((MonoGame.Framework.WinFormsGamePlatform)game.Platform);
            window.Initialize(width, height);

            return window;
        }
#endif

		#region IDisposable implementation

		/// <summary>
		/// Performs application-defined tasks associated with freeing,
		/// releasing, or resetting unmanaged resources.
		/// </summary>

		~GameWindow()
		{
			Dispose(false);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		private bool mDisposed = false;
		protected virtual void Dispose(bool disposing)
		{
			if (mDisposed)
				return;

			ReleaseUnmanagedResources ();
			if (disposing)
			{
				ReleaseManagedResources ();
			}

			mDisposed = true;
		}

		protected abstract void ReleaseUnmanagedResources();

		protected abstract void ReleaseManagedResources ();

		#endregion

    }
}
