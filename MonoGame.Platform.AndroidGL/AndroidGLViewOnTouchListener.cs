using System;
using Android.InputMethodServices;
using Android.Views;
using MonoGame.Platform.AndroidGL.Input;
using Android.Content;
using MonoGame.Platform.AndroidGL.Input.Touch;
using Android.Media;

namespace MonoGame.Platform.AndroidGL
{
	public class AndroidGLViewOnTouchListener : View.IOnTouchListener
	{
		#if !OUYA
		private bool backPressed;
		#endif

		private readonly IAndroidKeyboardListener mKeyboard;
		private Context mContext;
		IAndroidTouchEventManager _touchManager;

		
		public AndroidGLViewOnTouchListener (
			Context context,
			IAndroidKeyboardListener keyboard,
			IAndroidTouchEventManager touchManager
		)
		{
			mContext = context;
			mKeyboard = keyboard;
			_touchManager = touchManager;
		}

		#region IOnTouchListener implementation

		bool Android.Views.View.IOnTouchListener.OnTouch(View v, MotionEvent e)
		{
			_touchManager.OnTouchEvent(e);
			return true;
		}

		#endregion

		#region Key and Motion

		public override bool OnKeyDown(Android.Views.Keycode keyCode, KeyEvent e)
		{
			#if OUYA
			if (GamePad.OnKeyDown(keyCode, e))
			return true;
			#endif

			mKeyboard.KeyDown(keyCode);
			// we need to handle the Back key here because it doesnt work any other way
			#if !OUYA
			if (keyCode == Android.Views.Keycode.Back && !this.backPressed)
			{
				this.backPressed = true;
				// 
				// GamePad mGamePad. Back = true;
				return true;
			}
			#endif

			if (keyCode == Android.Views.Keycode.VolumeUp)
			{
				AudioManager audioManager = (AudioManager)mContext.GetSystemService(Context.AudioService);
				audioManager.AdjustStreamVolume(Stream.Music, Adjust.Raise, VolumeNotificationFlags.ShowUi);
				return true;
			}

			if (keyCode == Android.Views.Keycode.VolumeDown)
			{
				AudioManager audioManager = (AudioManager)mContext.GetSystemService(Context.AudioService);
				audioManager.AdjustStreamVolume(Stream.Music, Adjust.Lower, VolumeNotificationFlags.ShowUi);
				return true;
			}

			return true;
		}

		public override bool OnKeyUp(Android.Views.Keycode keyCode, KeyEvent e)
		{
			#if OUYA
			if (GamePad.OnKeyUp(keyCode, e))
			return true;
			#endif
			mKeyboard.KeyUp(keyCode);

			#if !OUYA
			if (keyCode == Android.Views.Keycode.Back)
				this.backPressed = false;
			#endif

			return true;
		}

		#if OUYA
		public override bool OnGenericMotionEvent(MotionEvent e)
		{
		if (GamePad.OnGenericMotionEvent(e))
		return true;

		return base.OnGenericMotionEvent(e);
		}
		#endif

		#endregion
	}
}

