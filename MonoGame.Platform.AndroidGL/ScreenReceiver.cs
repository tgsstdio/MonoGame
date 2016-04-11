using Android.Content;
using Microsoft.Xna.Framework.Media;
using Android.App;
using OpenTK.Platform.Android;

namespace MonoGame.Platform.AndroidGL
{
	public class ScreenReceiver : BroadcastReceiver
	{
		private readonly IViewResumer mViewResume;
		private readonly IMediaPlayer mMediaPlayer;
		private readonly KeyguardManager mKeyguard;
		private readonly IScreenLock mScreenLock;
		public ScreenReceiver (IViewResumer resumer, IMediaPlayer mediaPlayer, KeyguardManager keyGuard, IScreenLock screenLock)
		{
			mViewResume = resumer;
			mMediaPlayer = mediaPlayer;
			mKeyguard = keyGuard;
			mScreenLock = screenLock;
		}

		public override void OnReceive(Context context, Intent intent)
		{
			
			//Android.Util.Log.Info("MonoGame", intent.Action.ToString());

			if(intent.Action == Intent.ActionScreenOff)
			{
                OnLocked();
			}
			else if(intent.Action == Intent.ActionScreenOn)
			{
				// TODO : TAKE THIS OUT OF THIS CLASS
                // If the user turns the screen on just after it has automatically turned off, 
                // the keyguard will not have had time to activate and the ActionUserPreset intent
                // will not be broadcast. We need to check if the lock is currently active
                // and if not re-enable the game related functions.
                // http://stackoverflow.com/questions/4260794/how-to-tell-if-device-is-sleeping
                //KeyguardManager keyguard = (KeyguardManager)context.GetSystemService(Context.KeyguardService);
				if (!mKeyguard.InKeyguardRestrictedInputMode())
                    OnUnlocked();
			}
			else if(intent.Action == Intent.ActionUserPresent)
			{
                // This intent is broadcast when the user unlocks the phone
                OnUnlocked();
            }
		}

        private void OnLocked()
        {
			mScreenLock.ScreenLocked = true;
			mMediaPlayer.IsMuted = true;
        }

        private void OnUnlocked()
        {
			mScreenLock.ScreenLocked = false;
			mMediaPlayer.IsMuted = false;
			mViewResume.Resume();
        }
    }
}

