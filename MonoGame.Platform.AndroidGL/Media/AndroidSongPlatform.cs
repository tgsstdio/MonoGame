using System;
using Microsoft.Xna.Framework.Media;

namespace MonoGame.Platform.AndroidGL
{
	public class AndroidSongPlatform : ISongPlatform
	{
		#region ISongPlatform implementation

		public void PlatformSetTrackNumber (int value)
		{
			throw new NotImplementedException ();
		}

		public void PlatformDispose (bool disposed)
		{
			throw new NotImplementedException ();
		}

		public IGenre PlatformGetGenre ()
		{
			throw new NotImplementedException ();
		}

		public IArtist PlatformGetArtist ()
		{
			throw new NotImplementedException ();
		}

		public IAlbum PlatformGetAlbum ()
		{
			throw new NotImplementedException ();
		}

		public void PlatformInitialize (string fileName)
		{
			throw new NotImplementedException ();
		}

		public int PlatformGetRating ()
		{
			throw new NotImplementedException ();
		}

		public int PlatformGetTrackNumber ()
		{
			throw new NotImplementedException ();
		}

		public TimeSpan PlatformGetDuration ()
		{
			throw new NotImplementedException ();
		}

		public bool PlatformIsRated ()
		{
			throw new NotImplementedException ();
		}

		public bool PlatformIsProtected ()
		{
			throw new NotImplementedException ();
		}

		public string PlatformGetName ()
		{
			throw new NotImplementedException ();
		}

		public int PlatformGetPlayCount ()
		{
			throw new NotImplementedException ();
		}

		#endregion


	}
}

