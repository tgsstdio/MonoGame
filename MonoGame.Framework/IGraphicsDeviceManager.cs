// MIT License - Copyright (C) The Mono.Xna Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.
using System;
using Microsoft.Xna.Framework.Graphics;

namespace Microsoft.Xna.Framework
{
    public interface IGraphicsDeviceManager : IDisposable
    {
        bool BeginDraw();
        void CreateDevice();
        void EndDraw();
		bool IsFullScreen { get; set; }
		int PreferredBackBufferHeight { get; set; }
		int PreferredBackBufferWidth { get; set; }	
		IGraphicsDevice GraphicsDevice { get; }
	}
}

