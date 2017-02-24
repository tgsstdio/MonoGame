// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.IO;
using Microsoft.Xna.Framework.Media;

namespace MonoGame.Content.Audio.OpenAL.NVorbis
{
    public class NVorbisSong : ISong
    {
        private OggStream stream;
        private float _volume = 1f;
        private IMediaPlayer mPlayer;
        private TimeSpan _duration;
        private int _playCount;
        private IContentStreamer mContentStreamer;
        private string mExtension;
        private AssetIdentifier mAssetId;

        public NVorbisSong(IContentStreamer contentStreamer, string extension, AssetIdentifier assetId, IMediaPlayer player)
        {
            mContentStreamer = contentStreamer;
            mExtension = extension;
            mAssetId = assetId;
            mPlayer = player;
        }

        public void Initialize()
        {
            using (var fs = mContentStreamer.LoadContent(mAssetId, new[] { mExtension }))
            {
                stream = new OggStream(fs, OnFinishedPlaying);
                stream.Prepare();

                _duration = stream.GetLength();
            }
        }
        
        internal void SetEventHandler(FinishedPlayingHandler handler) { }

        internal void OnFinishedPlaying()
        {
            mPlayer.OnSongFinishedPlaying(null, null);
        }
		
        internal void Play()
        {
            stream.Play();
            _playCount++;
        }

        internal void Resume()
        {
            stream.Resume();
        }

        internal void Pause()
        {
            stream.Pause();
        }

        public void Stop()
        {
            stream.Stop();
            _playCount = 0;
        }

        public void SongCompleted(object sender, EventArgs args)
        {
            throw new NotImplementedException();
        }

        private bool mIsDisposed = false;
        public void Dispose()
        {
            if (mIsDisposed)
                return;

            if (stream != null)
            {
                stream.Dispose();
            }

            mIsDisposed = true;
        }

        public float Volume
        {
            get { return _volume; }
            set
            {
                _volume = value;
                stream.Volume = _volume;
            }
        }

        public TimeSpan Duration
        {
            get
            {
                return _duration;
            }
        }

        public TimeSpan Position
        {
            get
            {
                return stream.GetPosition();
            }
        }

        public int TrackNumber
        {
            get;
            set;
        }

        public string Name
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public int PlayCount
        {
            get
            {
                return _playCount;
            }
        }
    }
}

