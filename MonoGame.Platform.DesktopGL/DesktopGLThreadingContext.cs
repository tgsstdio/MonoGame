#region License
/*
Microsoft Public License (Ms-PL)
MonoGame - Copyright © 2009 The MonoGame Team

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
#endregion License

using Microsoft.Xna.Framework;
using System;
using System.Threading;
using OpenTK.Graphics;
using OpenTK.Platform;
using OpenTK.Graphics.OpenGL;

namespace MonoGame.Platform.DesktopGL
{
	public class DesktopGLThreadingContext : IThreadingContext
	{
		public const int MAX_WAIT_FOR_UI_THREAD = 750; // In milliseconds

		private int mainThreadId;

		public IGraphicsContext BackgroundContext;
		public IWindowInfo WindowInfo;

		public DesktopGLThreadingContext ()
		{
			mainThreadId = Thread.CurrentThread.ManagedThreadId;
		}

		public bool IsOnUIThread ()
		{
			return mainThreadId == Thread.CurrentThread.ManagedThreadId;
		}

		public void EnsureUIThread ()
		{
			if (!IsOnUIThread())
				throw new InvalidOperationException("Operation not called on UI thread.");
		}

		/// <summary>
		/// Runs the given action on the UI thread and blocks the current thread while the action is running.
		/// If the current thread is the UI thread, the action will run immediately.
		/// </summary>
		/// <param name="action">The action to be run on the UI thread</param>
		public void BlockOnUIThread(Action action)
		{
			if (action == null)
				throw new ArgumentNullException("action");

			// If we are already on the UI thread, just call the action and be done with it
			if (IsOnUIThread())
			{
				action();
				return;
			}

			lock (BackgroundContext)
			{
				// Make the context current on this thread
				BackgroundContext.MakeCurrent(WindowInfo);
				// Execute the action
				action();
				// Must flush the GL calls so the texture is ready for the main context to use
				GL.Flush();
				//GraphicsExtensions.CheckGLError();
				// Must make the context not current on this thread or the next thread will get error 170 from the MakeCurrent call
				BackgroundContext.MakeCurrent(null);
			}
		}


	}
}


