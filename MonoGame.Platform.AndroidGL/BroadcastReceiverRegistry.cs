using Android.Content;

namespace MonoGame.Platform.AndroidGL
{
	public class BroadcastReceiverRegistry : IBroadcastReceiverRegistry
	{
		private readonly BroadcastReceiver mReceiver;
		private readonly Context mContext;
		public BroadcastReceiverRegistry (BroadcastReceiver receiver, Context context)
		{
			mReceiver = receiver;
			mContext = context;
		}

		#region IBroadcastReceiverRegister implementation

		public void Register ()
		{
			IntentFilter filter = new IntentFilter();
			filter.AddAction(Intent.ActionScreenOff);
			filter.AddAction(Intent.ActionScreenOn);
			filter.AddAction(Intent.ActionUserPresent);

			//screenReceiver = new ScreenReceiver();
			mContext.RegisterReceiver(mReceiver, filter);
		}

		public void Unregister ()
		{
			mContext.UnregisterReceiver(mReceiver);
		}

		#endregion
	}
}

