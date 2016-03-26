// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;

namespace Microsoft.Xna.Framework.Media
{
	public interface IMediaPlayerPlatform
	{
		void Pause ();

		void SetVolume (float volume);

		float GetVolume ();

		bool GetGameHasControl ();

		void SetIsShuffled (bool value);

		bool GetIsShuffled ();

		void SetPlayPosition (TimeSpan value);

		MediaState GetState ();

		TimeSpan GetPlayPosition ();

		void SetIsRepeating (bool value);

		bool GetIsRepeating ();

		void SetIsMuted (bool value);

		bool GetIsMuted ();

		void Resume ();
		void Stop ();
		void PlaySong (ISong song);
		void Initialize ();		
	}
}

