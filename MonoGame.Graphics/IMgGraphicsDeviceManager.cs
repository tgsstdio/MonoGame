using Microsoft.Xna.Framework;
using Magnesium;

namespace MonoGame.Graphics
{
	public interface IMgGraphicsDeviceManager : IGraphicsDeviceManager
	{
		IMgGraphicsDevice Device { get; }
	}
}

