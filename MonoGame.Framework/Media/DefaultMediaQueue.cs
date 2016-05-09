// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.Collections.Generic;

namespace Microsoft.Xna.Framework.Media
{
	public sealed class DefaultMediaQueue : IMediaQueue
	{
        readonly List<ISong> songs = new List<ISong>();
		private int _activeSongIndex = -1;
		private Random random = new Random();

		public ISong ActiveSong
		{
			get
			{
				if (songs.Count == 0 || _activeSongIndex < 0)
					return null;
				
				return songs[_activeSongIndex];
			}
		}
		
		public int ActiveSongIndex
		{
		    get
		    {
		        return _activeSongIndex;
		    }
		    set
		    {
		        _activeSongIndex = value;
		    }
		}

        public int Count
        {
            get
            {
                return songs.Count;
            }
        }

		public ISong At(int index)
        {
        	return songs[index];
        }

		internal IEnumerable<ISong> Songs
        {
            get
            {
                return songs;
            }
        }

		public ISong GetNextSong(int direction, bool shuffle)
		{
			if (shuffle)
				_activeSongIndex = random.Next(songs.Count);
			else			
				_activeSongIndex = (int)MathHelper.Clamp(_activeSongIndex + direction, 0, songs.Count - 1);
			
			return songs[_activeSongIndex];
		}
		
		public void Clear()
		{
			ISong song;
			for(; songs.Count > 0; )
			{
				song = songs[0];
				songs.Remove(song);
			}	
		}

		public void Add(ISong song)
        {
            songs.Add(song);
        }

	}
}

