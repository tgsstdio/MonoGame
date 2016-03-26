// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

#if WINDOWS_PHONE
extern alias MicrosoftXnaFramework;
using MsArtist = MicrosoftXnaFramework::Microsoft.Xna.Framework.Media.Artist;
#endif
using System;

namespace Microsoft.Xna.Framework.Media
{
	#if WINDOWS_PHONE	
    public sealed class WindowsPhoneArtist : IArtist
    {
        private MsArtist artist;

        /// <summary>
        /// Gets the AlbumCollection for the Artist.
        /// </summary>
        public IAlbumCollection Albums
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Gets a value indicating whether the object is disposed.
        /// </summary>
        public bool IsDisposed
        {
            get
            {
                return this.artist.IsDisposed;
            }
        }

        /// <summary>
        /// Gets the name of the Artist.
        /// </summary>
        public string Name
        {
            get
            {
                return this.artist.Name;
            }
        }

        /// <summary>
        /// Gets the SongCollection for the Artist.
        /// </summary>
        public ISongCollection Songs
        {
            get
            {
                throw new NotImplementedException();
            }
        }

		public static implicit operator WindowsPhoneArtist(MsArtist artist)
        {
            return new Artist(artist);
        }

		private WindowsPhoneArtist(MsArtist artist)
        {
            this.artist = artist;
        }

        /// <summary>
        /// Immediately releases the unmanaged resources used by this object.
        /// </summary>
        public void Dispose()
        {
            this.artist.Dispose();
        }

        /// <summary>
        /// Returns a String representation of the Artist.
        /// </summary>
        public override string ToString()
        {
            return this.artist.ToString();
        }

        /// <summary>
        /// Gets the hash code for this instance.
        /// </summary>
        public override int GetHashCode()
        {
            return this.artist.GetHashCode();
        }
    }
	#endif
}
