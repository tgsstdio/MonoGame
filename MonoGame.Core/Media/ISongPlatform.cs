// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;

namespace Microsoft.Xna.Framework.Media
{
	public interface ISongPlatform
	{
		void PlatformSetTrackNumber (int value);

		void PlatformDispose (bool disposed);

		IGenre PlatformGetGenre ();

		IArtist PlatformGetArtist ();

		IAlbum PlatformGetAlbum ();

		void PlatformInitialize (string fileName);

		int PlatformGetRating ();

		int PlatformGetTrackNumber ();

		TimeSpan PlatformGetDuration ();

		bool PlatformIsRated ();

		bool PlatformIsProtected ();

		string PlatformGetName ();

		int PlatformGetPlayCount ();
	}
}

