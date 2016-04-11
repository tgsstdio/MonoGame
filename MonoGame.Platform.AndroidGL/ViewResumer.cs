using System;

namespace MonoGame.Platform.AndroidGL
{
	public class ViewResumer : IViewResumer
	{
		private readonly MonoGameAndroidGameView mGameView;
		public ViewResumer (MonoGameAndroidGameView gameView)
		{
			mGameView = gameView;
		}

		#region IViewResumer implementation

		public void Resume ()
		{
			mGameView.Resume ();
		}

		#endregion


	}
}

