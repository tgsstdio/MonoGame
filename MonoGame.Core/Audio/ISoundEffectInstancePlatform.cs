// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.
﻿
namespace Microsoft.Xna.Framework.Audio
{
	public interface ISoundEffectInstancePlatform
	{
		void PlatformDispose (bool disposing);

		void PlatformStop (bool b);

		void PlatformResume ();

		void PlatformPlay ();

		void PlatformPause ();

		void PlatformApply3D (AudioListener listener, AudioEmitter emitter);

		void PlatformInitialize (byte[] buffer, int sampleRate, int channels);

		SoundState PlatformGetState ();

		void PlatformSetVolume (float value);

		void PlatformSetPitch (float value);

		void PlatformSetPan (float value);

		bool PlatformGetIsLooped ();

		void PlatformSetIsLooped (bool value);
	}

}
