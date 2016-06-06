// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.
using Microsoft.Xna.Framework.Media;
using System;
using MonoGame.Platform.AndroidGL.Media;


namespace MonoGame.Platform.AndroidGL
{
	public class NullMediaPlayer : BaseMediaPlayer
	{
		private IAndroidSongPlayer mPlayer;
		public NullMediaPlayer (IMediaQueue queue, IAndroidSongPlayer player) : base(queue)
		{
			mPlayer = player;
		}

		#region implemented abstract members of BaseMediaPlayer
		private bool _isMuted = false;
		protected override bool GetIsMuted ()
		{
			return _isMuted;
		}
		protected override void SetIsMuted (bool value)
		{
			_isMuted = value;

			if (Queue.Count == 0)
				return;

			var newVolume = _isMuted ? 0.0f : _volume;
			mPlayer.SetVolume (newVolume);
			//Queue.SetVolume(newVolume);
		}
		private bool _isRepeating = false;
		protected override bool GetIsRepeating ()
		{
			return _isRepeating;
		}
		protected override void SetIsRepeating (bool value)
		{
			_isRepeating = value;
		}
		private bool _isShuffled = false;
		protected override bool GetIsShuffled ()
		{
			return _isShuffled;
		}
		protected override void SetIsShuffled (bool value)
		{
			_isShuffled = value;
		}
		protected override TimeSpan GetPlayPosition ()
		{
			var position = TimeSpan.Zero;
			if (Queue.ActiveSong != null && mPlayer.IsPlaying)
			{
				position = TimeSpan.FromMilliseconds (mPlayer.CurrentPosition);
			}

			return position;
		}
		protected override void SetPlayPosition (TimeSpan value)
		{
			if (Queue.ActiveSong != null)
				mPlayer.Seek((int)value.TotalMilliseconds); 
		}

		public event EventHandler<EventArgs> MediaStateChanged;
		protected override void OnMediaStateChange ()
		{
			if (MediaStateChanged != null)
				MediaStateChanged (null, EventArgs.Empty);
		}

		protected override MediaState GetState ()
		{
			return mState;
		}
		protected override bool GetGameHasControl ()
		{
			return true;
		}
		private float _volume;
		protected override float GetVolume ()
		{
			return _volume;
		}
		protected override void SetVolume (float value)
		{
			_volume = value;

			if (Queue.ActiveSong == null)
				return;

			var finalVolume = _isMuted ? 0.0f : _volume;

			foreach (var song in Queue.Songs)
				song.Volume = finalVolume;
		}
		protected override void PlatformPause ()
		{
			if (Queue.ActiveSong != null && mPlayer.Current != null)
				mPlayer.Pause();

			//Queue.ActiveSong.Pause();

		}
		protected override void PlatformPlaySong (ISong song)
		{
			if (Queue.ActiveSong == null)
				return;

			var droidSong = song as AndroidSong;
			droidSong.SetEventHandler(OnSongFinishedPlaying);
			droidSong.Volume = _isMuted ? 0.0f : _volume;
			mPlayer.IsLooping = this.IsRepeating;
			mPlayer.Play (droidSong.AssetUri, droidSong, droidSong.Name);

		}

		protected override void RepeatCurrentSong ()
		{

		}

		protected override void PlatformResume ()
		{
			if (Queue.ActiveSong != null && mPlayer.Current != null)
				mPlayer.Resume();
		}

		protected override void PlatformStop ()
		{
			// Loop through so that we reset the PlayCount as well
			foreach (var song in Queue.Songs)
				song.Stop();
		}
		#endregion
		
	}
}
