using System;
using Microsoft.Xna.Framework.Media;
using Android.Content;
using Android.Content.Res;

namespace MonoGame.Platform.AndroidGL
{
	public class AndroidSongPlayer : IAndroidSongPlayer
	{
		Android.Media.MediaPlayer mAndroidPlayer;
		ISong _playingSong;
		private readonly Context mContext;
		private IMediaPlayer mSystem;
		private AssetManager mAssetManager;

		public AndroidSongPlayer (Android.Media.MediaPlayer player, Context context, IMediaPlayer mediaPlayer, AssetManager assetManager)
		{
			mAndroidPlayer = player;
			mAndroidPlayer.Completion += this.AndroidPlayer_Completion;
			mContext = context;
			mSystem = mediaPlayer;
			mAssetManager = assetManager;
		}

		private void AndroidPlayer_Completion(object sender, EventArgs e)
		{
			var playingSong = _playingSong;
			_playingSong = null;

			if (playingSong != null)
				playingSong.SongCompleted(sender, e);
		}

		public void Play(Android.Net.Uri assetUri, ISong song, string name)
		{
			// Prepare the player
			mAndroidPlayer.Reset();

			if (assetUri != null)
			{
				mAndroidPlayer.SetDataSource(mContext, assetUri);
			}
			else
			{
				var afd = mAssetManager.OpenFd(name);
				if (afd == null)
					return;

				mAndroidPlayer.SetDataSource(afd.FileDescriptor, afd.StartOffset, afd.Length);
			}


			mAndroidPlayer.Prepare();
			mAndroidPlayer.Looping = mSystem.IsRepeating;
			_playingSong = song;

			mAndroidPlayer.Start();
			//song.PlayCount++;
		}

		public void Seek (int mSecs)
		{
			mAndroidPlayer.SeekTo (mSecs);
		}

		public void SetVolume (float value)
		{
			mAndroidPlayer.SetVolume (value, value);
		}

		public void Stop ()
		{
			mAndroidPlayer.Stop ();
		}

		public void Resume ()
		{
			mAndroidPlayer.Start ();
		}

		public void Pause ()
		{
			mAndroidPlayer.Pause ();
		}

		public int CurrentPosition {
			get {
				return mAndroidPlayer.CurrentPosition;
			}
		}

		public bool IsPlaying {
			get {
				return mAndroidPlayer.IsPlaying;
			}
		}

		public ISong Current {
			get {
				return _playingSong;
			}
		}
	}
}

