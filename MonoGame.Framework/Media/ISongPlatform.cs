// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;

namespace Microsoft.Xna.Framework.Media
{
	public interface ISongPlatform
	{
		void Dispose (bool disposed);

		Genre GetGenre ();

		Artist GetArtist ();

		Album GetAlbum ();

		void Initialize (string fileName);

		int GetRating ();

		int GetTrackNumber ();

		TimeSpan GetDuration ();

		bool IsRated ();

		bool IsProtected ();

		string GetName ();

		int GetPlayCount ();
	}
}

