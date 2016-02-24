// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using MonoGame.Graphics;

namespace Microsoft.Xna.Framework.Graphics
{
	public interface IRenderTarget2DPlatform
	{
		void GraphicsDeviceResetting ();

		void Construct (IWeakReferenceCollection owner, int width, int height, bool mipMap, SurfaceFormat preferredFormat, DepthFormat preferredDepthFormat, int preferredMultiSampleCount, RenderTargetUsage usage, bool shared);

		void Dispose (bool disposing);
	}
}
