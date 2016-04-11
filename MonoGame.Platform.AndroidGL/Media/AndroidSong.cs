// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.IO;
using Android.Media;
using Android.Net;
using Microsoft.Xna.Framework.Media;

namespace MonoGame.Platform.AndroidGL.Media
{
	public sealed class AndroidSong : BaseSong
    {
//        static Android.Media.MediaPlayer _androidPlayer;
//        static Song _playingSong;

        private IAlbum album;
        private IArtist artist;
        private IGenre genre;
		public string _name;
        private TimeSpan duration;
        private TimeSpan position;
        private Android.Net.Uri assetUri;

        [CLSCompliant(false)]
        public Android.Net.Uri AssetUri
        {
            get { return this.assetUri; }
        }

		// TODO : handle android player
//        static Song()
//        {
//            _androidPlayer = new Android.Media.MediaPlayer();
//            _androidPlayer.Completion += AndroidPlayer_Completion;
//        }

		private IAndroidSongPlayer mPlayer;
		public AndroidSong (IAndroidSongPlayer player, string name) : base(name)
		{
			mPlayer = player;
		}

		internal AndroidSong(IAlbum album, IArtist artist, IGenre genre, string name, TimeSpan duration, Android.Net.Uri assetUri)
			: base(name) 
        {
            this.album = album;
            this.artist = artist;
            this.genre = genre;
            this._name = name;
            this.duration = duration;
            this.assetUri = assetUri;
        }

		protected override void PlatformInitialize(string fileName)
        {
            // Nothing to do here
        }

		protected override void PlatformDispose(bool disposing)
        {
            // Appears to be a noOp on Android
        }

        internal void Play()
        {
			mPlayer.Play (this.assetUri, this, _name);
            _playCount++;
        }

        internal void Resume()
        {
            //_androidPlayer.Start();
			mPlayer.Resume();
        }

        internal void Pause()
        {
			mPlayer.Pause();
        }

        public override void Stop()
        {
//            _androidPlayer.Stop();
//            _playingSong = null;

			mPlayer.Stop ();

            _playCount = 0;
            position = TimeSpan.Zero;
        }

		protected override float PlatformGetVolume()
        {
           return 0.0f;
        }

		protected override void PlatformSetVolume(float value)
		{				
			mPlayer.SetVolume(value);            
        }

        public TimeSpan Position
        {
            get
            {
				if (mPlayer.Current == this && mPlayer.IsPlaying)
					position = TimeSpan.FromMilliseconds(mPlayer.CurrentPosition);

                return position;
            }
            set
            {
                //_androidPlayer.SeekTo((int)value.TotalMilliseconds);   
				mPlayer.Seek((int)value.TotalMilliseconds); 
            }
        }

		protected override void PlatformSetAlbum(IAlbum album)
		{
			throw new NotSupportedException ();
		}

		protected override IAlbum PlatformGetAlbum()
        {
            return this.album;
        }

		protected override IArtist PlatformGetArtist()
        {
            return this.artist;
        }

		protected override IGenre PlatformGetGenre()
        {
            return this.genre;
        }

		protected override TimeSpan PlatformGetDuration()
        {
            return this.assetUri != null ? this.duration : _duration;
        }

		protected override bool PlatformIsProtected()
        {
            return false;
        }

		protected override bool PlatformIsRated()
        {
            return false;
        }

		protected override string PlatformGetName()
        {
            return this.assetUri != null ? this._name : Path.GetFileNameWithoutExtension(_name);
        }

		protected override int PlatformGetPlayCount()
        {
            return _playCount;
        }

		protected override int PlatformGetRating()
        {
            return 0;
        }

		protected override int PlatformGetTrackNumber()
        {
            return 0;
        }

		protected override void PlatformSetTrackNumber(int value)
		{
			throw new NotSupportedException ();
		}
    }
}

