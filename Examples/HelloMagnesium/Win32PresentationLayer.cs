using OpenTK;
using System.Diagnostics;
using Magnesium;

namespace HelloMagnesium
{
	public class Win32PresentationLayer : IMgPresentationLayer
	{
		private readonly IMagnesiumDriver mPlatform;
		private readonly INativeWindow mWindow;
		public Win32PresentationLayer (IMagnesiumDriver platform, INativeWindow window)
		{
			mPlatform = platform;
			mWindow = window;
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
			var createInfo = new MgWin32SurfaceCreateInfoKHR {
				// DOUBLE CHECK 
				Hinstance = Process.GetCurrentProcess ().Handle,
				Hwnd = mWindow.WindowInfo.Handle,
			};
			var err = mPlatform.Instance.CreateWin32SurfaceKHR (createInfo, null, out mSurface);
			Debug.Assert (err == Result.SUCCESS);
		}

		#endregion

		#region IDisposable implementation
		private bool mIsDisposed = false;
		public void Dispose ()
		{
			if (mIsDisposed)
				return;

			if (mSurface != null)
			{
				mSurface.DestroySurfaceKHR (mPlatform.Instance, null);
			}

			mIsDisposed = true;
		}

		#endregion
	}
}

