// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.
﻿
namespace Microsoft.Xna.Framework.Audio
{
	public interface ISoundEffectInstancePlatform
	{
		void Dispose (bool disposing);

		void Stop (bool b);

		void Resume ();

		void Play ();

		void Pause ();

		void Apply3D (AudioListener listener, AudioEmitter emitter);

		void Initialize (byte[] buffer, int sampleRate, int channels);

		SoundState GetState ();

		void SetVolume (float value);

		void SetPitch (float value);

		void SetPan (float value);

		bool GetIsLooped ();

		void SetIsLooped (bool value);

		void SetupInstance (SoundEffectInstance inst);
	}

}
