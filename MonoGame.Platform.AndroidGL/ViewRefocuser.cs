using OpenTK.Platform.Android;
using MonoGame.Platform.AndroidGL.Input.Touch;
using Android.Views;

namespace MonoGame.Platform.AndroidGL
{
	public class ViewRefocuser : IViewRefocuser
	{
		private readonly AndroidGameView mGameView;
		private readonly IAndroidTouchEventManager mTouchManager;
		readonly View.IOnTouchListener mOnTouchListener;

		public ViewRefocuser (MonoGameAndroidGameView view, IAndroidTouchEventManager touchEventManager, View.IOnTouchListener onTouchListener)
		{
			mGameView = view;
			mTouchManager = touchEventManager;
			mOnTouchListener = onTouchListener;
		}

		#region IViewRefocuser implementation

		public void Run ()
		{
			// GameViewBase
			mGameView.Run ();
		}

		public void Pause ()
		{
			mGameView.Pause ();
		}

		public void Resume ()
		{
			mGameView.Resume ();
		}

		public bool IsFocused {
			get {
				return mGameView.IsFocused;
			}
		}

		public void SwapBuffers()
		{
			mGameView.SwapBuffers ();
		}

		public void Clear()
		{
			mGameView.ClearFocus ();
		}

		public void Refocus ()
		{
			mGameView.RequestFocus ();
		}

		public void MakeCurrent ()
		{
			mGameView.MakeCurrent ();
		}

		public bool TouchEnabled
		{
			get { return mTouchManager.Enabled; }
			set
			{
				mTouchManager.Enabled = value;
				mGameView.SetOnTouchListener(value ? mOnTouchListener : null);
			}
		}
		#endregion
	}
}

