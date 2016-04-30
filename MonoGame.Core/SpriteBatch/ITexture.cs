using System;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Core
{
	public interface ITexture
	{
		Int32 SortingKey { get; }
		SurfaceFormat Format { get; }
		Int32 LevelCount { get; }
		void GraphicsDeviceResetting ();
	}
}

