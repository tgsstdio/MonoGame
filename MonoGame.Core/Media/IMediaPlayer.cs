// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;

namespace Microsoft.Xna.Framework.Media
{
	public interface IMediaPlayer
	{
		MediaState State { get; }
		 
		void Play(ISong song);

		void Pause ();

		void Resume ();

		bool IsMuted {
			get;
			set;
		}		

		bool IsRepeating {
			get;
			set;
		}

        void OnSongFinishedPlaying(object sender, EventArgs args);
    }
}

