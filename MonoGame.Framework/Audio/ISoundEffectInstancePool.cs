// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

namespace Microsoft.Xna.Framework.Audio
{
	public interface ISoundEffectInstancePool
	{
		ISoundEffectInstance GetInstance (bool forXAct);
		bool SoundsAvailable { get; }
		void Remove (ISoundEffectInstance inst);
		void Update ();	
		void UpdateMasterVolume();
	}
}
