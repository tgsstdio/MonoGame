#region License
/*
Microsoft Public License (Ms-PL)
XnaTouch - Copyright © 2009 The XnaTouch Team

All rights reserved.

This license governs use of the accompanying software. If you use the software, you accept this license. If you do not
accept the license, do not use the software.

1. Definitions
The terms "reproduce," "reproduction," "derivative works," and "distribution" have the same meaning here as under 
U.S. copyright law.

A "contribution" is the original software, or any additions or changes to the software.
A "contributor" is any person that distributes its contribution under this license.
"Licensed patents" are a contributor's patent claims that read directly on its contribution.

2. Grant of Rights
(A) Copyright Grant- Subject to the terms of this license, including the license conditions and limitations in section 3, 
each contributor grants you a non-exclusive, worldwide, royalty-free copyright license to reproduce its contribution, prepare derivative works of its contribution, and distribute its contribution or any derivative works that you create.
(B) Patent Grant- Subject to the terms of this license, including the license conditions and limitations in section 3, 
each contributor grants you a non-exclusive, worldwide, royalty-free license under its licensed patents to make, have made, use, sell, offer for sale, import, and/or otherwise dispose of its contribution in the software or derivative works of the contribution in the software.

3. Conditions and Limitations
(A) No Trademark License- This license does not grant you rights to use any contributors' name, logo, or trademarks.
(B) If you bring a patent claim against any contributor over patents that you claim are infringed by the software, 
your patent license from such contributor to the software ends automatically.
(C) If you distribute any portion of the software, you must retain all copyright, patent, trademark, and attribution 
notices that are present in the software.
(D) If you distribute any portion of the software in source code form, you may do so only under this license by including 
a complete copy of this license with your distribution. If you distribute any portion of the software in compiled or object 
code form, you may only do so under a license that complies with this license.
(E) The software is licensed "as-is." You bear the risk of using it. The contributors give no express warranties, guarantees
or conditions. You may have additional consumer rights under your local laws which this license cannot change. To the extent
permitted under your local laws, the contributors exclude the implied warranties of merchantability, fitness for a particular
purpose and non-infringement.
*/
using MonoGame.Platform.DesktopGL.Input;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Core;


#endregion License

#region Using Statements
using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using Microsoft.Xna.Framework.Input;
using OpenTK;
using OpenTK.Graphics;
using Microsoft.Xna.Framework;

#endregion Using Statements

namespace MonoGame.Platform.DesktopGL
{
	using Rectangle = MonoGame.Core.Rectangle;

	public class OpenTKGameWindow : Microsoft.Xna.Framework.GameWindow, IOpenTKGameWindow
    {
        private bool _isResizable;
        private bool _isBorderless;

		//private DisplayOrientation _currentOrientation;
        private IntPtr _windowHandle;
        private INativeWindow window;

		protected IPresentationParameters PresentationParameters;
        private List<Microsoft.Xna.Framework.Input.Keys> keys;
		//private OpenTK.Graphics.GraphicsContext backgroundContext;

        // we need this variables to make changes beetween threads
        private WindowState windowState;
		///private Rectangle mClientBounds;
        private Rectangle targetBounds;
        private bool updateClientBounds;
        private int updateborder = 0;

        #region Internal Properties

        internal INativeWindow Window { get { return window; } }

        #endregion

        #region Public Properties

        public override IntPtr Handle { get { return _windowHandle; } }

		public override string ScreenDeviceName { get { return window.Title; } }

		//public override Rectangle ClientBounds { get { return mClientBounds; } }

        // TODO: this is buggy on linux - report to opentk team
		public override bool AllowUserResizing
        {
            get { return _isResizable; }
            set
            {
                if (_isResizable != value)
                    _isResizable = value;
                else
                    return;
                if (_isBorderless)
                    return;
                window.WindowBorder = _isResizable ? WindowBorder.Resizable : WindowBorder.Fixed;
            }
        }

//		public override DisplayOrientation CurrentOrientation
//        {
//            get { return DisplayOrientation.LandscapeLeft; }
//        }
#if DESKTOPGL
        public Microsoft.Xna.Framework.Point Position
        {
            get { return new Microsoft.Xna.Framework.Point(window.Location.X,window.Location.Y); }
            set { window.Location = new System.Drawing.Point(value.X,value.Y); }
        }

        public System.Drawing.Icon Icon
        {
            get
            {
                return window.Icon;
            }
            set
            {
                window.Icon = value;
            }
        }
#endif

		public override bool IsBorderless
        {
            get { return _isBorderless; }
            set
            {
                if (_isBorderless != value)
                    _isBorderless = value;
                else
                    return;
                if (_isBorderless)
                {
                    window.WindowBorder = WindowBorder.Hidden;
                }
                else
                    window.WindowBorder = _isResizable ? WindowBorder.Resizable : WindowBorder.Fixed;
            }
        }

        #endregion

		private IMouseListener mMouse;
		private IKeyboardInputListener mKeyboard;
		private IDrawSuppressor mSuppressor;
		private IClientWindowBounds mClient;
		public OpenTKGameWindow(
			IPresentationParameters parameters, 
			INativeWindow emptyWindow,
			IDrawSuppressor suppressor, 
			IMouseListener mouseListener,
			IKeyboardInputListener keyboard,
			IClientWindowBounds client
		)
        {
			mSuppressor = suppressor;
			PresentationParameters = parameters;
			mMouse = mouseListener;
			mKeyboard = keyboard;
			mClient = client;
			Initialize(emptyWindow);
        }

        #region Restricted Methods

        #region OpenTK GameWindow Methods

        #region Delegates

        private void OpenTkGameWindow_Closing(object sender, CancelEventArgs e)
        {
			mSuppressor.Cleanup ();
        }

        private void Keyboard_KeyUp(object sender, OpenTK.Input.KeyboardKeyEventArgs e)
        {
            Keys xnaKey = KeyboardUtil.ToXna(e.Key);
            if (keys.Contains(xnaKey)) keys.Remove(xnaKey);
        }
        
        private void Keyboard_KeyDown(object sender, OpenTK.Input.KeyboardKeyEventArgs e)
        {
            if (_allowAltF4 && e.Key == OpenTK.Input.Key.F4 && keys.Contains(Keys.LeftAlt))
            {
                window.Close();
                return;
            }
            Keys xnaKey = KeyboardUtil.ToXna(e.Key);
            if (!keys.Contains(xnaKey)) keys.Add(xnaKey);
        }

        #endregion

        private void OnResize(object sender, EventArgs e)
        {
            // Ignore resize events until intialization is complete
//            if (Backbone == null)
//                return;

            var winWidth = window.ClientRectangle.Width;
            var winHeight = window.ClientRectangle.Height;
            var winRect = new Rectangle(0, 0, winWidth, winHeight);

            // If window size is zero, leave bounds unchanged
            // OpenTK appears to set the window client size to 1x1 when minimizing
            if (winWidth <= 1 || winHeight <= 1) 
                return;

            //If we've already got a pending change, do nothing
            if (updateClientBounds)
                return;
            
			PresentationParameters.BackBufferWidth = winWidth;
			PresentationParameters.BackBufferHeight = winHeight;

			// TODO : resize viewport
            // Game.GraphicsDevice.Viewport = new Viewport(0, 0, winWidth, winHeight);

			mClient.ClientBounds = winRect;

			mClient.OnClientSizeChanged();
        }

		public void ProcessEvents()
        {
            UpdateBorder();
            Window.ProcessEvents();
            UpdateWindowState();
            HandleInput();
        }

        private void UpdateBorder()
        {
            if (updateborder == 1)
            {
                WindowBorder desired;
                if (_isBorderless)
                    desired = WindowBorder.Hidden;
                else
                    desired = _isResizable ? WindowBorder.Resizable : WindowBorder.Fixed;
            
                if (desired != window.WindowBorder && window.WindowState != WindowState.Fullscreen)
                    window.WindowBorder = desired;
            }

            if(updateborder > 0)
                updateborder--;
        }

        private void UpdateWindowState()
        {
            // we should wait until window's not fullscreen to resize

            if (updateClientBounds)
            {
                window.WindowBorder = WindowBorder.Resizable;
                updateClientBounds = false;
                window.ClientRectangle = new System.Drawing.Rectangle(targetBounds.X,
                                     targetBounds.Y, targetBounds.Width, targetBounds.Height);
                
                // if the window-state is set from the outside (maximized button pressed) we have to update it here.
                // if it was set from the inside (.IsFullScreen changed), we have to change the window.
                // this code might not cover all corner cases
                // window was maximized
                if ((windowState == WindowState.Normal && window.WindowState == WindowState.Maximized) ||
                    (windowState == WindowState.Maximized && window.WindowState == WindowState.Normal))
                    windowState = window.WindowState; // maximize->normal and normal->maximize are usually set from the outside
                else
                    window.WindowState = windowState; // usually fullscreen-stuff is set from the code

                // we need to create a small delay between resizing the window
                // and changing the border to avoid OpenTK Linux bug
                updateborder = 2;

                var context = GraphicsContext.CurrentContext;
                if (context != null)
                    context.Update(window.WindowInfo);
            }
        }

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

        private void HandleInput()
        {
            // mouse doesn't need to be treated here, Mouse class does it alone

            // keyboard
			mKeyboard.SetKeys(keys);
        }

        private void OnKeyPress(object sender, KeyPressEventArgs e)
        {
            OnTextInput(sender, new TextInputEventArgs(e.KeyChar));
        }
        
		protected void OnTextInput(object sender, TextInputEventArgs e)
		{
			if (TextInput != null)
				TextInput(sender, e);
		}

        #endregion

		private void Initialize(INativeWindow emptyWindow)
        {
            GraphicsContext.ShareContexts = true;

			window = emptyWindow;
            window.WindowBorder = WindowBorder.Resizable;
            window.Closing += new EventHandler<CancelEventArgs>(OpenTkGameWindow_Closing);
            window.Resize += OnResize;
            window.KeyDown += new EventHandler<OpenTK.Input.KeyboardKeyEventArgs>(Keyboard_KeyDown);
            window.KeyUp += new EventHandler<OpenTK.Input.KeyboardKeyEventArgs>(Keyboard_KeyUp);

            window.KeyPress += OnKeyPress;

            //make sure that the game is not running on linux
            //on Linux people may want to use mkbundle to
            //create native Linux binaries
            if (CurrentPlatform.OS != OS.Linux)
            {
                // Set the window icon.
                var assembly = Assembly.GetEntryAssembly();
                if (assembly != null)
                    window.Icon = Icon.ExtractAssociatedIcon(assembly.Location);
                Title = AssemblyHelper.GetDefaultWindowTitle();
            }

            updateClientBounds = false;
			mClient.ClientBounds = new Rectangle(window.ClientRectangle.X, window.ClientRectangle.Y,
                                         window.ClientRectangle.Width, window.ClientRectangle.Height);
            windowState = window.WindowState;            

            _windowHandle = window.WindowInfo.Handle;

            keys = new List<Keys>();

            // mouse
            // TODO review this when opentk 1.1 is released
#if DESKTOPGL || ANGLE
			mMouse.PrimaryWindow = this;
#else
            Mouse.UpdateMouseInfo(window.Mouse);
#endif

            // Default no resizing
            AllowUserResizing = false;

            // Default mouse cursor hidden 
            SetMouseVisible(false);
        }

		protected override void SetTitle(string title)
        {
            window.Title = title;            
        }

		public void ToggleFullScreen()
        {
            if (windowState == WindowState.Fullscreen)
                windowState = WindowState.Normal;
            else
                windowState = WindowState.Fullscreen;
            updateClientBounds = true;
        }

        public void ChangeClientBounds(Rectangle clientBounds)
        {
			if (mClient.ClientBounds != clientBounds)
            {
                updateClientBounds = true;
                targetBounds = clientBounds;
            }
        }

        #endregion

        #region Public Methods

		protected override void ReleaseManagedResources()
		{
			window.Dispose();
		}

		protected override void ReleaseUnmanagedResources()
		{
			_windowHandle = IntPtr.Zero;
		}

		public override void BeginScreenDeviceChange(bool willBeFullScreen)
        {
        }

		public override void EndScreenDeviceChange(string screenDeviceName, int clientWidth, int clientHeight)
        {

        }

		#endregion

		#region implemented abstract members of BaseOpenTKGameWindow

		public OpenTK.Platform.IWindowInfo GetWindowInfo ()
		{
			return window.WindowInfo;
		}

		public void SetWindowVisible (bool visible)
		{
			Window.Visible = visible;
		}

		public bool IsWindowFocused ()
		{
			return Window.Focused;
		}

		public void SetMouseVisible(bool visible)
        {
            window.Cursor = visible ? MouseCursor.Default : MouseCursor.Empty;
        }

        #endregion
    }
}

