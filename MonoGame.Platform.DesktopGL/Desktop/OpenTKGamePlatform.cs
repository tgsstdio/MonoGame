#region License
/*
Microsoft Public License (Ms-PL)
MonoGame - Copyright © 2009-2011 The MonoGame Team

All rights reserved.

This license governs use of the accompanying software. If you use the software,
you accept this license. If you do not accept the license, do not use the
software.

1. Definitions

The terms "reproduce," "reproduction," "derivative works," and "distribution"
have the same meaning here as under U.S. copyright law.

A "contribution" is the original software, or any additions or changes to the
software.

A "contributor" is any person that distributes its contribution under this
license.

"Licensed patents" are a contributor's patent claims that read directly on its
contribution.

2. Grant of Rights

(A) Copyright Grant- Subject to the terms of this license, including the
license conditions and limitations in section 3, each contributor grants you a
non-exclusive, worldwide, royalty-free copyright license to reproduce its
contribution, prepare derivative works of its contribution, and distribute its
contribution or any derivative works that you create.

(B) Patent Grant- Subject to the terms of this license, including the license
conditions and limitations in section 3, each contributor grants you a
non-exclusive, worldwide, royalty-free license under its licensed patents to
make, have made, use, sell, offer for sale, import, and/or otherwise dispose of
its contribution in the software or derivative works of the contribution in the
software.

3. Conditions and Limitations

(A) No Trademark License- This license does not grant you rights to use any
contributors' name, logo, or trademarks.

(mGraphicsu bring a patent claim against any contributor over patents that you
claim mGraphicsinged by the software, your patent license from such contributor
to the software ends automatically.

(C) If you distribute any portion of the software, you must retain all
copyright, patent, trademark, and attribution notices that are present in the
software.

(D) If you distribute any portion of the software in source code form, you may
do so only under this license by including a complete copy of this license with
your distribution. If you distribute any portion of the software in compiled or
object code form, you may only do so under a license that complies with this
license.

(E) The software is licensed "as-is." You bear the risk of using it. The
contributors give no express warranties, guarantees or conditions. You may have
additional consumer rights under your local laws which this license cannot
change. To the extent permitted under your local laws, the contributors exclude
the implied warranties of merchantability, fitness for a particular purpose and
non-infringement.
*/
using Microsoft.Xna.Framework;
using MonoGame.Audio.OpenAL;
using Microsoft.Xna.Framework.Input;

#endregion License

using System;
using System.Threading;


using OpenTK;

namespace MonoGame.Platform.DesktopGL
{
    public class OpenTKGamePlatform : BaseGamePlatform
    {
        private IOpenTKGameWindow _view;
		private IOpenALSoundController soundControllerInstance = null;
        // stored the current screen state, so we can check if it has changed.
        private Toolkit toolkit;
        private int isExiting; // int, so we can use Interlocked.Increment
        
		//private IGameBackbone mBackbone;
		private IGraphicsDeviceManager mGraphics;
		private IOpenTKWindowResetter mWindowReset;
		public OpenTKGamePlatform(IPlatformActivator activator, IGraphicsDeviceManager graphics, IOpenTKGameWindow view, IOpenTKWindowResetter resetter, IOpenALSoundController soundController, IMouseListener mouseListener)
			: base(activator)
        {
			mGraphics = graphics;
			mWindowReset = resetter;
            toolkit = Toolkit.Init();
			_view = view;
           // this.Window = _view;

			// Setup our OpenALSoundController to handle our SoundBuffer pools
//            try
//            {
				soundControllerInstance = soundController;
//            }
//            catch (DllNotFoundException ex)
//            {
//                throw (new NoAudioHardwareException("Failed to init OpenALSoundController", ex));
//            }
        }

        public override GameRunBehavior DefaultRunBehavior
        {
            get { return GameRunBehavior.Synchronous; }
        }

        protected override void OnIsMouseVisibleChanged()
        {
            _view.SetMouseVisible(IsMouseVisible);
        }

		public override void RunLoop(Action doTick)
        {
			ResetWindowBounds();
            while (true)
            {
                _view.ProcessEvents();

                // Stop the main loop iff Game.Exit() has been called.
                // This can happen under the following circumstances:
                // 1. Game.Exit() is called programmatically.
                // 2. The GameWindow is closed through the 'X' (close) button
                // 3. The GameWindow is closed through Alt-F4 or Cmd-Q
                // Note: once Game.Exit() is called, we must stop raising 
                // Update or Draw events as the GameWindow and/or OpenGL context
                // may no longer be available. 
                // Note 2: Game.Exit() can be called asynchronously from
                // _view.ProcessEvents() (cases #2 and #3 above), so the
                // isExiting check must be placed *after* _view.ProcessEvents()
                if (isExiting > 0)
                {
                    break;
                }

				//mBackbone.Tick();
				doTick();
            }
        }

        public override void StartRunLoop()
        {
            throw new NotSupportedException("The desktop platform does not support asynchronous run loops");
        }

        public override void Exit()
        {
            //(SJ) Why is this called here when it's not in any other project
            //Net.NetworkSession.Exit();
            Interlocked.Increment(ref isExiting);

            // sound controller must be disposed here
            // so that it doesn't stop the game from disposing
            if (soundControllerInstance != null)
            {
                soundControllerInstance.Dispose();
                soundControllerInstance = null;
            }
            OpenTK.DisplayDevice.Default.RestoreResolution();
        }

        public override void BeforeInitialize()
        {
            //_view.Window.Visible = true;
			_view.SetWindowVisible (true);
            base.BeforeInitialize();
        }

        public override bool BeforeUpdate(GameTime gameTime)
        {
			Activator.IsActive = _view.IsWindowFocused();

            // Update our OpenAL sound buffer pools
            if (soundControllerInstance != null)
                soundControllerInstance.Update();
            return true;
        }

        public override bool BeforeDraw(GameTime gameTime)
        {
            return true;
        }

        public override void EnterFullScreen()
        {
			ResetWindowBounds();
        }

        public override void ExitFullScreen()
        {
			ResetWindowBounds();
        }

		private void ResetWindowBounds()
		{
			bool wasActive = Activator.IsActive;
			Activator.IsActive = false;
			mWindowReset.ResetWindowBounds();
			Activator.IsActive = wasActive;
		}

        public override void EndScreenDeviceChange(string screenDeviceName, int clientWidth, int clientHeight)
        {
            
        }

        public override void BeginScreenDeviceChange(bool willBeFullScreen)
        {
            
        }
        
        public override void Log(string message)
        {
            Console.WriteLine(message);
        }

        public override void Present()
        {
			var device = mGraphics.GraphicsDevice;
            if (device != null)
                device.Present();
        }
		
        protected override void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                if (toolkit != null)
                {
                    toolkit.Dispose();
                    toolkit = null;
                }

                if (_view != null)
                {
                    _view.Dispose();
                    _view = null;
                }
            }

			base.Dispose(disposing);
        }
    }
}
