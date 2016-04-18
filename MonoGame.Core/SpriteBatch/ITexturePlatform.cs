// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.
using System;
using Magnesium;

namespace MonoGame.Core
{
	public interface ITexturePlatform
	{
		void GraphicsDeviceResetting (
			MgImage image,
			MgImageView view,
			MgSampler sampler,
			MgDeviceMemory deviceMemory);
		Int32 GenerateSortingKey();
	}
}

