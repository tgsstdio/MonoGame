using OpenTK;
using System.Diagnostics;
using Magnesium;
using Microsoft.Xna.Framework.Graphics;

namespace HelloMagnesium
{
    // HANDLES THE WINDOWING ISSUE
	public class MgOpenTKPresentationSurface : IMgPresentationSurface
	{
		private readonly MgDriver mDriver;
		private readonly INativeWindow mWindow;
		public MgOpenTKPresentationSurface(MgDriver driver, IPresentationParameters presentationParameters, INativeWindow window)
		{
			mDriver = driver;
			mWindow = window;
            mPresentationParameters = presentationParameters;
		}

		#region IMgPresentationLayer implementation

		public IMgSurfaceKHR Surface {
			get {
				return mSurface;
			}
		}

		private IMgSurfaceKHR mSurface;
		public void Initialize ()
		{
            /// SEEMS THE WINDOW DIMENSIONS (WIDTH, HEIGHT) MUST BE SET PROIR TO BEING PASSED INTO VULKAN
            /// DIMENSIONS TAKEN FROM IPresentationParameters
            mWindow.ClientRectangle = new System.Drawing.Rectangle
                (mWindow.ClientRectangle.X, mWindow.ClientRectangle.Y, mPresentationParameters.BackBufferWidth, mPresentationParameters.BackBufferHeight);          

            var createInfo = new MgWin32SurfaceCreateInfoKHR {
				// DOUBLE CHECK 
				Hinstance = Process.GetCurrentProcess ().Handle,
				Hwnd = mWindow.WindowInfo.Handle,
			};
			var err = mDriver.Instance.CreateWin32SurfaceKHR (createInfo, null, out mSurface);
			Debug.Assert (err == Result.SUCCESS);
		}

		#endregion

		#region IDisposable implementation
		private bool mIsDisposed = false;
        private IPresentationParameters mPresentationParameters;

        public void Dispose ()
		{
			if (mIsDisposed)
				return;

			if (mSurface != null)
			{
				mSurface.DestroySurfaceKHR (mDriver.Instance, null);
			}

			mIsDisposed = true;
		}

		#endregion
	}
}

