// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.
//
// Author: Kenneth James Pouncey

using System;

namespace Microsoft.Xna.Framework.Graphics
{
	public interface ISamplerStateCollectionPlatform
	{
		void Dirty ();

		void Clear ();

		void SetSamplerState (int index);
	}

}
