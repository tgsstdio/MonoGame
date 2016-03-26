using Android.Views;

namespace MonoGame.Platform.Android.Input.Touch
{
	public interface IAndroidTouchEventManager
	{
		void OnTouchEvent (MotionEvent e);

		bool Enabled {
			get;
			set;
		}
	}
}

