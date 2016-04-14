// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.
using Magnesium;
using System;

namespace MonoGame.Graphics
{
	public interface ITexturePlatform
	{
		void GraphicsDeviceResetting (MgImage image, MgImageView view);
		Int32 GenerateSortingKey();
	}
}

