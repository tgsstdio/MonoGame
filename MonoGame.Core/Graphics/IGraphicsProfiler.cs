using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Core
{
	public interface IGraphicsProfiler
	{
		GraphicsProfile GetHighestSupportedGraphicsProfile ();
	}
}

