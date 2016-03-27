// MIT License - Copyright (C) The Mono.Xna Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.
using System;

namespace Microsoft.Xna.Framework
{
    public interface IGraphicsDeviceManager : IDisposable
    {
        bool BeginDraw();
        void CreateDevice();
        void EndDraw();
		IGraphicsDevice GraphicsDevice { get; }
		void ResetClientBounds ();

		DisplayOrientation SupportedOrientations {
			get;
			set;
		}

		void ApplyChanges();
	}
}

