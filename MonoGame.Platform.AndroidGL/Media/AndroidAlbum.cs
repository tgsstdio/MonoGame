// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.IO;

using Android.Graphics;
using Android.Provider;
using Microsoft.Xna.Framework.Media;
using Android.Net;

namespace MonoGame.Platform.AndroidGL.Media
{
    public sealed class AndroidAlbum : IAlbum
    {
        private IArtist artist;
        private IGenre genre;
        private string album;
		private ISongCollection songCollection;

        private Android.Net.Uri thumbnail;

		public IArtist Artist
        {
            get
            {
                return this.artist;
            }
        }

        /// <summary>
        /// Gets the duration of the Album.
        /// </summary>
        public TimeSpan Duration
        {
            get
            {
                return TimeSpan.Zero; // Not implemented
            }
        }

        /// <summary>
        /// Gets the Genre of the Album.
        /// </summary>
        public IGenre Genre
        {
            get
            {
                return this.genre;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the Album has associated album art.
        /// </summary>
        public bool HasArt
        {
            get
            {
                return this.thumbnail != null;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the object is disposed.
        /// </summary>
        public bool IsDisposed
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the name of the Album.
        /// </summary>
        public string Name
        {
            get
            {
                return this.album;
            }
        }

        /// <summary>
        /// Gets a SongCollection that contains the songs on the album.
        /// </summary>
        public ISongCollection Songs
        {
            get
            {
                return this.songCollection;

            }
        }

		private AndroidAlbum(IAndroidAlbumArtContentResolver artResolver, ISongCollection songCollection, string name, IArtist artist, IGenre genre)
        {
			this.mArtResolver = artResolver;
            this.songCollection = songCollection;
            this.album = name;
            this.artist = artist;
            this.genre = genre;
        }

		internal AndroidAlbum(IAndroidAlbumArtContentResolver artResolver, ISongCollection songCollection, string name, IArtist artist, IGenre genre, Android.Net.Uri thumbnail)
			: this(artResolver, songCollection, name, artist, genre)
        {			
            this.thumbnail = thumbnail;
        }


        /// <summary>
        /// Immediately releases the unmanaged resources used by this object.
        /// </summary>
        public void Dispose()
        {

        }
        
		private IAndroidAlbumArtContentResolver mArtResolver;

        [CLSCompliant(false)]
        public Bitmap GetAlbumArt(int width = 0, int height = 0)
        {
			var albumArt = mArtResolver.Resolve (this.thumbnail);
            if (width == 0 || height == 0)
                return albumArt;

            var scaledAlbumArt = Bitmap.CreateScaledBitmap(albumArt, width, height, true);
            albumArt.Dispose();
            return scaledAlbumArt;
        }

        [CLSCompliant(false)]
        public Bitmap GetThumbnail()
        {
            return this.GetAlbumArt(220, 220);
        }

		/// <summary>
        /// Returns a String representation of this Album.
        /// </summary>
        public override string ToString()
        {
            return this.album.ToString();
        }

        /// <summary>
        /// Gets the hash code for this instance.
        /// </summary>
        public override int GetHashCode()
        {
            return this.album.GetHashCode();
        }
    }
}
