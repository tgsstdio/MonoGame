// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.
using System;

namespace Microsoft.Xna.Framework.Graphics
{
	public interface IPresentationParameters
	{
		void Clear ();
		bool IsFullScreen { get; set; }
		SurfaceFormat BackBufferFormat { get; set; }
		int BackBufferHeight { get; set; }
		int BackBufferWidth { get; set;	}
		Rectangle Bounds { get; }
		IntPtr DeviceWindowHandle { get; set; }
		DepthFormat DepthStencilFormat {get;set;}
		int MultiSampleCount { get; set; }
		PresentInterval PresentationInterval { get; set; }
		DisplayOrientation DisplayOrientation { get; set; }
		RenderTargetUsage RenderTargetUsage { get; set; }
		IPresentationParameters Clone ();
	}
}
