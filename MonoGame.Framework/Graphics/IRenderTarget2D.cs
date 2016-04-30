// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using MonoGame.Core;

namespace Microsoft.Xna.Framework.Graphics
{
	public interface IRenderTarget2D : ITexture2D, IRenderTarget
	{
		DepthFormat DepthStencilFormat { get; }
		int MultiSampleCount { get; }
		bool IsContentLost { get; }
	}
}
