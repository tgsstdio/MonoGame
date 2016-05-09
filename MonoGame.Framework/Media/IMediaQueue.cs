// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

namespace Microsoft.Xna.Framework.Media
{
	public interface IMediaQueue
	{
		ISong GetNextSong (int direction, bool isShuffled);

		int ActiveSongIndex {
			get;
			set;
		}

		ISong At (int index);

		void Add (ISong song);

		void Clear ();

		int Count {
			get;
		}

		ISong ActiveSong {
			get;
		}		
	}
}

