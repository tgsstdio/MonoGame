// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;

namespace Microsoft.Xna.Framework.Media
{
    public abstract class BaseMediaPlayer : IMediaPlayer
    {
		// Need to hold onto this to keep track of how many songs
		// have played when in shuffle mode
		private int _numSongsInQueuePlayed = 0;


#if WINDOWS_PHONE
        // PlayingInternal should default to true to be to work with the user's default playing music
        private static bool playingInternal = true;
#endif

		public event EventHandler<EventArgs> ActiveSongChanged;

		protected BaseMediaPlayer(IMediaQueue queue)
        {
			Queue = queue;
        }

        #region Properties
		public IMediaQueue Queue { get; }

		protected abstract bool GetIsMuted ();
		protected abstract void SetIsMuted (bool value);

		public bool IsMuted
        {
			get { return GetIsMuted(); }
			set { SetIsMuted(value); }
        }

		protected abstract bool GetIsRepeating ();
		protected abstract void SetIsRepeating (bool value);

        public bool IsRepeating 
        {
			get { return GetIsRepeating(); }
			set { SetIsRepeating(value); }
        }

		protected abstract bool GetIsShuffled ();
		protected abstract void SetIsShuffled (bool value);

        public bool IsShuffled
        {
			get { return GetIsShuffled(); }
			set { SetIsShuffled(value); }
        }

        public bool IsVisualizationEnabled { get { return false; } }

		protected abstract TimeSpan GetPlayPosition ();
		protected abstract void SetPlayPosition (TimeSpan value);

        public TimeSpan PlayPosition
        {
			get { return GetPlayPosition(); }
			set { SetPlayPosition(value); }
        }

		protected abstract void OnMediaStateChange ();
//		{
//#if WINDOWS_PHONE
//                      // Playing music using XNA, we shouldn't fire extra state changed events
//                        if (!playingInternal)
//#endif
//			MediaStateChanged (null, EventArgs.Empty);
//		}

		protected abstract MediaState GetState ();

		protected MediaState mState = MediaState.Stopped;
        public MediaState State
        {
			get { return GetState(); }
            private set
            {
				if (mState != value)
                {
                    mState = value;
					OnMediaStateChange ();
                }
            }
        }

		protected abstract bool GetGameHasControl ();

        public bool GameHasControl
        {
            get
            {
				return GetGameHasControl();
            }
        }
		
		protected abstract float GetVolume();
		protected abstract void SetVolume (float value);

        public float Volume
        {
			get { return GetVolume(); }
            set
            {
                var volume = MathHelper.Clamp(value, 0, 1);

				SetVolume(volume);
            }
        }

		#endregion

		protected abstract void PlatformPause();

        public void Pause()
        {
            if (State != MediaState.Playing || Queue.ActiveSong == null)
                return;

			PlatformPause();

            State = MediaState.Paused;
        }
		
		/// <summary>
		/// Play clears the current playback queue, and then queues up the specified song for playback. 
		/// Playback starts immediately at the beginning of the song.
		/// </summary>
        public void Play(ISong song)
        {
			ISong previousSong = Queue.Count > 0 ? Queue.At(0) : null;
            Queue.Clear();
            _numSongsInQueuePlayed = 0;
            Queue.Add(song);
			Queue.ActiveSongIndex = 0;
            
            PlaySong(song);

            if (previousSong != song && ActiveSongChanged != null)
                ActiveSongChanged.Invoke(null, EventArgs.Empty);
        }
		
		public void Play(StandardSongCollection collection, int index = 0)
		{
            Queue.Clear();
            _numSongsInQueuePlayed = 0;

			foreach(var song in collection)
				Queue.Add(song);
			
			Queue.ActiveSongIndex = index;
			
			PlaySong(Queue.ActiveSong);
		}

		protected abstract void PlatformPlaySong (ISong song);

        private void PlaySong(ISong song)
        {
			PlatformPlaySong(song);
            State = MediaState.Playing;
        }

		protected abstract void RepeatCurrentSong ();

        public void OnSongFinishedPlaying(object sender, EventArgs args)
		{
			// TODO: Check args to see if song sucessfully played
			_numSongsInQueuePlayed++;
			
			if (_numSongsInQueuePlayed >= Queue.Count)
			{
				_numSongsInQueuePlayed = 0;
				if (!IsRepeating)
				{
					Stop();

					if (ActiveSongChanged != null)
					{
						ActiveSongChanged.Invoke(null, null);
					}

					return;
				}
			}

			RepeatCurrentSong ();
			
			MoveNext();
		}

		protected abstract void PlatformResume ();

        public void Resume()
        {
            if (State != MediaState.Paused)
                return;

			PlatformResume();
			State = MediaState.Playing;
        }

		protected abstract void PlatformStop ();

        public void Stop()
        {
            if (State == MediaState.Stopped)
                return;

			PlatformStop();
			State = MediaState.Stopped;
		}
		
		public void MoveNext()
		{
			NextSong(1);
		}
		
		public void MovePrevious()
		{
			NextSong(-1);
		}
		
		private void NextSong(int direction)
		{
            Stop();

            if (IsRepeating && Queue.ActiveSongIndex >= Queue.Count - 1)
            {
                Queue.ActiveSongIndex = 0;
                
                // Setting direction to 0 will force the first song
                // in the queue to be played.
                // if we're on "shuffle", then it'll pick a random one
                // anyway, regardless of the "direction".
                direction = 0;
            }

			var nextSong = Queue.GetNextSong(direction, IsShuffled);

            if (nextSong != null)
                PlaySong(nextSong);

            if (ActiveSongChanged != null)
            {
                ActiveSongChanged.Invoke(null, null);
            }
		}
    }
}

