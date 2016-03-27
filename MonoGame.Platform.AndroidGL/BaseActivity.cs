using System;
using Android.Content;
using Android.Views;

namespace MonoGame.Platform.AndroidGL
{
	using Context = Android.Content.Context;

	public class BaseActivity : IBaseActivity
	{
		private Context mContext;
		private readonly IBroadcastReceiverRegistry mReceiverRegistry;
		private IOrientationListener mOrientationListener;
		private readonly IScreenLock mScreenLock;
		public BaseActivity (Context context, IBroadcastReceiverRegistry receiverRegistry, IOrientationListener orientation, IScreenLock screenLock)
		{
			mContext = context;
			mReceiverRegistry = receiverRegistry;
			mOrientationListener = orientation;
			mScreenLock = screenLock;
		}

		#region IBaseActivity implementation

		public void OnCreate ()
		{
			mReceiverRegistry.Register ();
		}

		public void OnResume ()
		{
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

