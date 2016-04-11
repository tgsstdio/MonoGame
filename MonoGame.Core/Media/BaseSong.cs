// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.IO;

namespace Microsoft.Xna.Framework.Media
{
	public abstract class BaseSong : ISong
	//IEquatable<ISong>,

    {
        protected string _name;
		protected int _playCount = 0;
		protected TimeSpan _duration = TimeSpan.Zero;
        bool disposed;

		public event FinishedPlayingHandler DonePlaying;

		public void SongCompleted (object sender, EventArgs args)
		{
			if (DonePlaying != null)
			{
				DonePlaying (sender, args);
			}
		}

		/// <summary>
		/// Set the event handler for "Finished Playing". Done this way to prevent multiple bindings.
		/// </summary>
		public void SetEventHandler(FinishedPlayingHandler handler)
		{
			if (DonePlaying != null)
				return;
			DonePlaying += handler;
		}

		protected abstract IAlbum PlatformGetAlbum ();
		protected abstract void PlatformSetAlbum (IAlbum value);

        /// <summary>
        /// Gets the Album on which the Song appears.
        /// </summary>
        public IAlbum Album
        {
            get { return PlatformGetAlbum(); }
            protected set { PlatformSetAlbum(value); }
        }

		protected abstract IArtist PlatformGetArtist ();

        /// <summary>
        /// Gets the Artist of the Song.
        /// </summary>
        public IArtist Artist
        {
            get { return PlatformGetArtist(); }
        }

		protected abstract IGenre PlatformGetGenre ();

        /// <summary>
        /// Gets the Genre of the Song.
        /// </summary>
        public IGenre Genre
        {
            get { return PlatformGetGenre(); }
        }

		protected BaseSong(string fileName, int durationMS)
			: this(fileName)
        {
            _duration = TimeSpan.FromMilliseconds(durationMS);
        }

		protected BaseSong(string fileName)
		{			
			_name = fileName;
        }

		protected abstract void PlatformInitialize (string fileName);
		public void Initialize ()
		{			
			PlatformInitialize(_name);
		}

		~BaseSong()
        {
            Dispose(false);
        }

        internal string FilePath
		{
			get { return _name; }
		}

// TODO : Fix this
//		public static Song FromUri(ISongPlatform platform, string name, Uri uri)
//        {
//            if (!uri.IsAbsoluteUri)
//            {
//				var song = new Song(platform, uri.OriginalString);
//                song._name = name;
//                return song;
//            }
//            else
//            {
//                throw new NotImplementedException("Loading songs from an absolute path is not implemented");
//            }
//        }
		
		public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        
		protected abstract void PlatformDispose (bool disposing);
        void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    PlatformDispose(disposing);
                }

                disposed = true;
            }
        }

//        public override int GetHashCode ()
//		{
//			return base.GetHashCode ();
//		}
//
//		protected abstract bool SongEquals (ISong left, ISong right);
//
//		public bool Equals(ISong song)
//        {
//#if DIRECTX
//            return song != null && song.FilePath == FilePath;
//#else
//			return ((object)song != null) && (Name == song.Name);
//#endif
//		}
//		
//		
//		public override bool Equals(Object obj)
//		{
//			if(obj == null)
//			{
//				return false;
//			}
//			
//			return Equals(obj as ISong);  
//		}
		
//		public static bool operator ==(ISong song1, ISong song2)
//		{
//			if((object)song1 == null)
//			{
//				return (object)song2 == null;
//			}
//
//			return song1.Equals(song2);
//		}
//		
//		public static bool operator !=(ISong song1, ISong song2)
//		{
//		  return ! (song1 == song2);
//		}

		protected abstract TimeSpan PlatformGetDuration (); 

        public TimeSpan Duration
        {
			get { return PlatformGetDuration(); }
        }	

		protected abstract bool PlatformIsProtected ();

        public bool IsProtected
        {
			get { return PlatformIsProtected(); }
        }

		protected abstract bool PlatformIsRated ();

        public bool IsRated
        {
			get { return PlatformIsRated(); }
        }

		protected abstract string PlatformGetName ();

        public string Name
        {
			get { return PlatformGetName(); }
        }

		protected abstract int PlatformGetPlayCount ();
        public int PlayCount
        {
			get { return PlatformGetPlayCount(); }
        }

		protected abstract int PlatformGetRating ();
        public int Rating
        {
			get { return PlatformGetRating(); }
        }

		protected abstract int PlatformGetTrackNumber ();
		protected abstract void PlatformSetTrackNumber (int value);

        public int TrackNumber
        {
			get { return PlatformGetTrackNumber(); }
			set { PlatformSetTrackNumber(value); }
        }

		public abstract void Stop ();

		protected abstract float PlatformGetVolume();
		protected abstract void PlatformSetVolume(float value);
		public float Volume {
			get {
				return PlatformGetVolume ();
			}
			set {
				PlatformSetVolume (value);
			}
		}	
    }
}

