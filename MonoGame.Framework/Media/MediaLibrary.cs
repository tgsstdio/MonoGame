// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;

namespace Microsoft.Xna.Framework.Media
{
	public partial class MediaLibrary : IMediaLibrary, IDisposable
	{
		public IAlbumCollection Albums { get { return mPlatform.GetAlbums();  } }
        //public ArtistCollection Artists { get; private set; }
        //public GenreCollection Genres { get; private set; }
        public bool IsDisposed { get; private set; }
        public MediaSource MediaSource { get { return null; } }
		//public PlaylistCollection Playlists { get; private set; }
		public ISongCollection Songs { get { return mPlatform.GetSongs(); } }

		private IMediaLibraryPlatform mPlatform;
		public MediaLibrary(IMediaLibraryPlatform platform)
		{
			mPlatform = platform;
#if WINDOWS_PHONE
			// TODO : should go into mPlatform.Initialize()
			// Load it automaticall on Windows Phone because it has no cost and people might expect the same behaviour as WP7
            PlatformLoad(null);
#endif
			mPlatform.Initialize ();
		}

        /// <summary>
        /// Load the contents of MediaLibrary. This blocking call might take up to a few minutes depending on the platform and the size of the user's music library.
        /// </summary>
        /// <param name="progressCallback">Callback that reports back the progress of the music library loading in percents (0-100).</param>
        public void Load(Action<int> progressCallback = null)
	    {
			mPlatform.Load(progressCallback);
	    }
		
		public MediaLibrary(MediaSource mediaSource)
		{
            throw new NotSupportedException("Initializing from MediaSource is not supported");
		}
		
		public void Dispose()
		{
			mPlatform.Dispose();
		    this.IsDisposed = true;
		}
	}
}

