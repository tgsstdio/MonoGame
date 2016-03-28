// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;

namespace Microsoft.Xna.Framework
{
	public interface IGameBackbone : IDisposable
	{
		void DoInitialize();

		void Tick();
		//IGraphicsDevice GraphicsDevice { get; }

		[CLSCompliant(false)]
		IGameWindow Window { get; }

		void Run();
	}
}
