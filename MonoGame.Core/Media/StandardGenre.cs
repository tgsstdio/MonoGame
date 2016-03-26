// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;

namespace Microsoft.Xna.Framework.Media
{
    public sealed class StandardGenre : IGenre
    {
        private string genre;

        /// <summary>
        /// Gets the AlbumCollection for the Genre.
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
                return true;
            }
        }

        /// <summary>
        /// Gets the name of the Genre.
        /// </summary>
        public string Name
        {
            get
            {
                return this.genre;
            }
        }

        /// <summary>
        /// Gets the SongCollection for the Genre.
        /// </summary>
        public ISongCollection Songs
        {
            get
            {
                throw new NotImplementedException();
            }
        }

		public StandardGenre(string genre)
        {
            this.genre = genre;
        }

        /// <summary>
        /// Immediately releases the unmanaged resources used by this object.
        /// </summary>
        public void Dispose()
        {

        }

        /// <summary>
        /// Returns a String representation of the Genre.
        /// </summary>
        public override string ToString()
        {
            return this.genre;
        }

        /// <summary>
        /// Gets the hash code for this instance.
        /// </summary>
        public override int GetHashCode()
        {
            return this.genre.GetHashCode();
        }
    }
}
