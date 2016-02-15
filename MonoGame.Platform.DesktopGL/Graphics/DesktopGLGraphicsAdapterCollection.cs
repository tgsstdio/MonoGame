using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Platform.DesktopGL.Graphics
{
	public class DesktopGLGraphicsAdapterCollection : IGraphicsAdapterCollection
	{
		public IGraphicsAdapter[] Options { get; private set;}
		public DesktopGLGraphicsAdapterCollection ()
		{
			Options = new IGraphicsAdapter[]{ new DesktopGLGraphicsAdapter () };
		}
	}
}

