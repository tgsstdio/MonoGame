using OpenTK;
using System.Diagnostics;
using Magnesium;

namespace HelloMagnesium
{
	public class Win32PresentationSurface : IMgPresentationSurface
	{
		private readonly IMgDriver mDriver;
		private readonly INativeWindow mWindow;
		public Win32PresentationSurface (IMgDriver driver, INativeWindow window)
		{
			mDriver = driver;
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
			var err = mDriver.Instance.CreateWin32SurfaceKHR (createInfo, null, out mSurface);
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
				mSurface.DestroySurfaceKHR (mDriver.Instance, null);
			}

			mIsDisposed = true;
		}

		#endregion
	}
}

