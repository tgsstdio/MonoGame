// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.IO;

namespace Microsoft.Xna.Framework.Media
{
	public abstract class BaseSong : IEquatable<ISong>, ISong
    {
        //private string _name;
		//private int _playCount = 0;
       // private TimeSpan _duration = TimeSpan.Zero;
       // bool disposed;

        /// <summary>
        /// Gets the Album on which the Song appears.
        /// </summary>
        public IAlbum Album
        {
            get { return mPlatform.GetAlbum(); }
#if WINDOWS_STOREAPP || WINDOWS_UAP
            internal set { mPlatform.SetAlbum(value); }
#endif
        }

        /// <summary>
        /// Gets the Artist of the Song.
        /// </summary>
        public IArtist Artist
        {
            get { return mPlatform.GetArtist(); }
        }

        /// <summary>
        /// Gets the Genre of the Song.
        /// </summary>
        public IGenre Genre
        {
            get { return mPlatform.GetGenre(); }
        }

#if ANDROID || OPENAL || WEB || IOS
        internal delegate void FinishedPlayingHandler(object sender, EventArgs args);
#if !DESKTOPGL
        event FinishedPlayingHandler DonePlaying;
#endif
#endif

		internal BaseSong(ISongPlatform platform, string fileName, int durationMS)
			: this(platform, fileName)
        {
            _duration = TimeSpan.FromMilliseconds(durationMS);
        }

		private readonly ISongPlatform mPlatform;
		internal BaseSong(ISongPlatform platform, string fileName)
		{			
			mPlatform = platform;
			_name = fileName;

			mPlatform.Initialize(fileName);
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
        
        void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    mPlatform.Dispose(disposing);
                }

                disposed = true;
            }
        }

        public override int GetHashCode ()
		{
			return base.GetHashCode ();
		}

		protected abstract bool SongEquals (ISong left, ISong right);

		public bool Equals(ISong song)
        {
#if DIRECTX
            return song != null && song.FilePath == FilePath;
#else
			return ((object)song != null) && (Name == song.Name);
#endif
		}
		
		
		public override bool Equals(Object obj)
		{
			if(obj == null)
			{
				return false;
			}
			
			return Equals(obj as ISong);  
		}
		
		public static bool operator ==(ISong song1, ISong song2)
		{
			if((object)song1 == null)
			{
				return (object)song2 == null;
			}

			return song1.Equals(song2);
		}
		
		public static bool operator !=(ISong song1, ISong song2)
		{
		  return ! (song1 == song2);
		}

        public TimeSpan Duration
        {
			get { return mPlatform.GetDuration(); }
        }	

        public bool IsProtected
        {
			get { return mPlatform.IsProtected(); }
        }

        public bool IsRated
        {
			get { return mPlatform.IsRated(); }
        }

        public string Name
        {
			get { return mPlatform.GetName(); }
        }

        public int PlayCount
        {
			get { return mPlatform.GetPlayCount(); }
        }

        public int Rating
        {
			get { return mPlatform.GetRating(); }
        }

        public int TrackNumber
        {
			get { return mPlatform.GetTrackNumber(); }
			set { mPlatform.SetTrackNumber(value); }
        }

		public abstract void Stop ();

		public float Volume {
			get;
			set;
		}	
    }
}

