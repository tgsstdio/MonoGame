using System;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Core
{
	public interface ITexture
	{
		Int32 SortingKey { get; }
		SurfaceFormat Format { get; }
		UInt32 LevelCount { get; }
		void GraphicsDeviceResetting ();		
	}
}

