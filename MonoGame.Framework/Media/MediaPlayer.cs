// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;

namespace Microsoft.Xna.Framework.Media
{
    public partial class MediaPlayer : IMediaPlayer
    {
		// Need to hold onto this to keep track of how many songs
		// have played when in shuffle mode
		private int _numSongsInQueuePlayed = 0;
		private MediaState _state = MediaState.Stopped;
		private float _volume = 1.0f;
		private bool _isMuted;
        private bool _isRepeating;
        private bool _isShuffled;
		private readonly MediaQueue _queue = new MediaQueue();

#if WINDOWS_PHONE
        // PlayingInternal should default to true to be to work with the user's default playing music
        private static bool playingInternal = true;
#endif

		public event EventHandler<EventArgs> ActiveSongChanged;
        public event EventHandler<EventArgs> MediaStateChanged;

		private IMediaPlayerPlatform mPlatform;
        public MediaPlayer(IMediaPlayerPlatform platform)
        {
			mPlatform = platform;
			mPlatform.Initialize();
        }

        #region Properties

        public MediaQueue Queue { get { return _queue; } }

		public bool IsMuted
        {
			get { return mPlatform.GetIsMuted(); }
			set { mPlatform.SetIsMuted(value); }
        }

        public bool IsRepeating 
        {
			get { return mPlatform.GetIsRepeating(); }
			set { mPlatform.SetIsRepeating(value); }
        }

        public bool IsShuffled
        {
			get { return mPlatform.GetIsShuffled(); }
			set { mPlatform.SetIsShuffled(value); }
        }

        public bool IsVisualizationEnabled { get { return false; } }

        public TimeSpan PlayPosition
        {
			get { return mPlatform.GetPlayPosition(); }
//#if IOS || ANDROID
			set { mPlatform.SetPlayPosition(value); }
//#endif
        }

        public MediaState State
        {
			get { return mPlatform.GetState(); }
            private set
            {
                if (_state != value)
                {
                    _state = value;
                    if (MediaStateChanged != null)
#if WINDOWS_PHONE
                        // Playing music using XNA, we shouldn't fire extra state changed events
                        if (!playingInternal)
#endif
                            MediaStateChanged(null, EventArgs.Empty);
                }
            }
        }

        public bool GameHasControl
        {
            get
            {
				return mPlatform.GetGameHasControl();
            }
        }
		

        public float Volume
        {
			get { return mPlatform.GetVolume(); }
            set
            {
                var volume = MathHelper.Clamp(value, 0, 1);

				mPlatform.SetVolume(volume);
            }
        }

		#endregion
		
        public void Pause()
        {
            if (State != MediaState.Playing || _queue.ActiveSong == null)
                return;

			mPlatform.Pause();

            State = MediaState.Paused;
        }
		
		/// <summary>
		/// Play clears the current playback queue, and then queues up the specified song for playback. 
		/// Playback starts immediately at the beginning of the song.
		/// </summary>
        public void Play(ISong song)
        {
            var previousSong = _queue.Count > 0 ? _queue[0] : null;
            _queue.Clear();
            _numSongsInQueuePlayed = 0;
            _queue.Add(song);
			_queue.ActiveSongIndex = 0;
            
            PlaySong(song);

            if (previousSong != song && ActiveSongChanged != null)
                ActiveSongChanged.Invoke(null, EventArgs.Empty);
        }
		
		public void Play(StandardSongCollection collection, int index = 0)
		{
            _queue.Clear();
            _numSongsInQueuePlayed = 0;

			foreach(var song in collection)
				_queue.Add(song);
			
			_queue.ActiveSongIndex = index;
			
			PlaySong(_queue.ActiveSong);
		}

        private void PlaySong(ISong song)
        {
			mPlatform.PlaySong(song);
            State = MediaState.Playing;
        }

        internal void OnSongFinishedPlaying(object sender, EventArgs args)
		{
			// TODO: Check args to see if song sucessfully played
			_numSongsInQueuePlayed++;
			
			if (_numSongsInQueuePlayed >= _queue.Count)
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

#if WINDOWS_PHONE
            if (IsRepeating)
            {
                System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    _mediaElement.Position = TimeSpan.Zero;
                    _mediaElement.Play();
                });
            }
#endif
			
			MoveNext();
		}

        public void Resume()
        {
            if (State != MediaState.Paused)
                return;

			mPlatform.Resume();
			State = MediaState.Playing;
        }

        public void Stop()
        {
            if (State == MediaState.Stopped)
                return;

			mPlatform.Stop();
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

            if (IsRepeating && _queue.ActiveSongIndex >= _queue.Count - 1)
            {
                _queue.ActiveSongIndex = 0;
                
                // Setting direction to 0 will force the first song
                // in the queue to be played.
                // if we're on "shuffle", then it'll pick a random one
                // anyway, regardless of the "direction".
                direction = 0;
            }

			var nextSong = _queue.GetNextSong(direction, IsShuffled);

            if (nextSong != null)
                PlaySong(nextSong);

            if (ActiveSongChanged != null)
            {
                ActiveSongChanged.Invoke(null, null);
            }
		}
    }
}

