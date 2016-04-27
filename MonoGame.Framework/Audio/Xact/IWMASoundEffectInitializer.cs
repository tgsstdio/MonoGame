// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

namespace Microsoft.Xna.Framework.Audio
{
	public interface IWMASoundEffectInitializer
	{
		SoundEffect LoadEffect (byte[] audiodata, bool isWma, bool isM4a);
	}
}

