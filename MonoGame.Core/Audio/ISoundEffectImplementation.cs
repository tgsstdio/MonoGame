// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.
using System.IO;

namespace Microsoft.Xna.Framework.Audio
{
	public interface ISoundEffectImplementation
	{
		void Dispose (bool disposing);

		void LoadAudioStream (Stream s);
		void SetupInstance (ISoundEffectInstance inst);
		void Initialize (byte[] buffer, int sampleRate, AudioChannels channels);
		void Initialize (byte[] buffer, int offset, int count, int sampleRate, AudioChannels channels, int loopStart, int loopLength);
	}

}
