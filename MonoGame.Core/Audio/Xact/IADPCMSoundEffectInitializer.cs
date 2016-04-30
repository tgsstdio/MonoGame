// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

namespace Microsoft.Xna.Framework.Audio
{
	public interface IADPCMSoundEffectInitializer
	{
		SoundEffect LoadEffect (byte[] audiodata, int rate, int chans, int align);
	}
}

