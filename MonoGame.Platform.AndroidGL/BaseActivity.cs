using System;
using Android.Content;
using Android.Views;
using Android.OS;

namespace MonoGame.Platform.AndroidGL
{
	using Context = Android.Content.Context;

	public class BaseActivity : IBaseActivity
	{
		private Context mContext;
		private readonly IBroadcastReceiverRegistry mReceiverRegistry;
		private IOrientationListener mOrientationListener;
		private readonly IScreenLock mScreenLock;
		private IForceFullScreenToggle mFullScreenToggle;
		private IViewRefocuser mViewRefocuser;

		public BaseActivity (
			Context context,
			IBroadcastReceiverRegistry receiverRegistry,
			IOrientationListener orientation,
			IScreenLock screenLock,
			IForceFullScreenToggle fullScreenToggle,
			IViewRefocuser viewRefocuser
			)
		{
			mContext = context;
			mReceiverRegistry = receiverRegistry;
			mOrientationListener = orientation;
			mScreenLock = screenLock;
			mFullScreenToggle = fullScreenToggle;
			mViewRefocuser = viewRefocuser;
		}

		#region IBaseActivity implementation

		public void OnCreate (Bundle savedInstanceState)
		{
			mReceiverRegistry.Register ();
		}

		public void OnResume ()
		{
			mFullScreenToggle.ForceSetFullScreen ();
			mViewRefocuser.Refocus();			
			mOrientationListener.On();
		}

		public void OnPause ()
		{
			mOrientationListener.Off();
		}

		public void OnDestroy ()
		{
			mReceiverRegistry.Unregister ();
			mScreenLock.ScreenLocked = false;
			mOrientationListener = null;
		}

		#endregion


	}
}

