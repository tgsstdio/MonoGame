using System;
using Android.OS;

namespace MonoGame.Platform.AndroidGL
{
	public interface IBaseActivity
	{
		void OnCreate(Bundle savedInstanceState);
		void OnResume();
		void OnPause();
		void OnDestroy();
	}
}

