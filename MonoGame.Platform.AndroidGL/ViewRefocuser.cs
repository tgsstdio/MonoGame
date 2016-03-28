using OpenTK.Platform.Android;
using MonoGame.Platform.AndroidGL.Input.Touch;

namespace MonoGame.Platform.AndroidGL
{
	public class ViewRefocuser : IViewRefocuser
	{
		private readonly AndroidGameView mGameView;
		private IAndroidTouchEventManager mTouchManager;
		public ViewRefocuser (AndroidGameView view, IAndroidTouchEventManager touchEventManager)
		{
			mGameView = view;
			mTouchManager = touchEventManager;
		}

		#region IViewRefocuser implementation

		public void Run ()
		{
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
		#endregion
	}
}

