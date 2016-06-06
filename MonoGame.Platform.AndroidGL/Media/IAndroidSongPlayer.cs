using Microsoft.Xna.Framework.Media;

namespace MonoGame.Platform.AndroidGL
{
	public interface IAndroidSongPlayer
	{
		void Seek (int i);

		int CurrentPosition {
			get;
		}

		bool IsPlaying {
			get;
		}

		void SetVolume (float value);

		void Stop ();

		void Resume ();

		void Pause ();

		ISong Current { get; }
		void Play (Android.Net.Uri assetUri, ISong song, string name);

		bool IsLooping { get; set; }
	}
}

