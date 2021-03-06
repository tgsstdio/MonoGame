﻿using System;

namespace Microsoft.Xna.Framework.Audio
{
	public interface ISoundPlayer
	{
		bool Play(SoundEffect effect);
		ISoundEffectInstance GetPooledInstance (bool forXAct, SoundEffect effect);
	}
}

