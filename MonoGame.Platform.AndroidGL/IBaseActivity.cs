using System;

namespace MonoGame.Platform.AndroidGL
{
	public interface IBaseActivity
	{
		void OnCreate();
		void OnResume();
		void OnPause();
		void OnDestroy();
	}
}

