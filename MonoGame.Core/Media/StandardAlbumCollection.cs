// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System.Collections.Generic;

namespace Microsoft.Xna.Framework.Media
{
	public sealed class StandardAlbumCollection : IAlbumCollection
    {
		private List<IAlbum> albumCollection;

        /// <summary>
        /// Gets the number of Album objects in the AlbumCollection.
        /// </summary>
        public int Count
        {
            get
            {
                return this.albumCollection.Count;
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

		public StandardAlbumCollection(List<IAlbum> albums)
        {
            this.albumCollection = albums;
        }


		public IAlbum GetAlbum (int index)
		{
			return this.albumCollection[index];
		}

        /// <summary>
        /// Gets the Album at the specified index in the AlbumCollection.
        /// </summary>
        /// <param name="index">Index of the Album to get.</param>
		public IAlbum this[int index]
        {
            get
            {
                return this.albumCollection[index];
            }
        }

        /// <summary>
        /// Immediately releases the unmanaged resources used by this object.
        /// </summary>
        public void Dispose()
        {
            foreach (var album in this.albumCollection)
                album.Dispose();
        }
    }
}
