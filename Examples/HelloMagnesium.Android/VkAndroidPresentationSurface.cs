using System;
using System.Runtime.InteropServices;
using Android.Views;
using Java.Interop;
using Magnesium;

namespace HelloMagnesium.Android
{
	public class VkAndroidPresentationSurface : IMgPresentationSurface
	{
		// FROM VulkanView.cs (2016) - Mono/VulkanSharp
		const string ANDROID_RUNTIME_LIBRARY = "android";

		[DllImport(ANDROID_RUNTIME_LIBRARY)]
		internal static unsafe extern IntPtr ANativeWindow_fromSurface(IntPtr jniEnv, IntPtr handle);

		[DllImport(ANDROID_RUNTIME_LIBRARY)]
		internal static unsafe extern void ANativeWindow_release(IntPtr window);

		private IntPtr mNativeWindow = IntPtr.Zero;
		private readonly Surface mSurface;
		private readonly IMgDriver mDriver;
		public VkAndroidPresentationSurface(Surface surface, IMgDriver driver)
		{
			mSurface = surface;
			mDriver = driver;
		}

		private IMgSurfaceKHR mVkSurface;
		public IMgSurfaceKHR Surface
		{
			get
			{
				return mVkSurface;
			}
		}

		private bool mIsDisposed = false;
		public void Dispose()
		{
			if (mIsDisposed)
				return;
			
			mVkSurface.DestroySurfaceKHR(mDriver.Instance, null);
			ANativeWindow_release(mNativeWindow);

			mIsDisposed = true;
		}

		public void Initialize()
		{
			mNativeWindow = ANativeWindow_fromSurface(JniEnvironment.EnvironmentPointer, mSurface.Handle);

			var createInfo = new MgAndroidSurfaceCreateInfoKHR
			{
				Window = mNativeWindow
			};
			IMgSurfaceKHR surface;
			var error = mDriver.Instance.CreateAndroidSurfaceKHR(createInfo, null, out surface);
			System.Diagnostics.Debug.Assert(error == Result.SUCCESS);
			mVkSurface = Surface;
		}
	}
}