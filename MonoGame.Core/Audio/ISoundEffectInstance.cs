// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;

namespace Microsoft.Xna.Framework.Audio
{
	public interface ISoundEffectInstance : IDisposable
	{
		bool IsLooped { get; set; }
		float Pan { get; set; }
		float Pitch { get; set; }
		float Volume { get; set; }
		SoundState State { get; }
		bool IsDisposed { get; }
		void Apply3D(AudioListener listener, AudioEmitter emitter);
		void Apply3D(AudioListener[] listeners, AudioEmitter emitter);
		void Pause();
		void Play();
		void Resume();
		void Stop();
		void Stop (bool immediate);

		bool IsXAct { get; set; }
		SoundEffect Effect { get; set; }
		bool IsPooled { get; set; }
	}
}
