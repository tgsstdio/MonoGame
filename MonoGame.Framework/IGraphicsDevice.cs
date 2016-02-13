using Microsoft.Xna.Framework.Graphics;

namespace Microsoft.Xna.Framework
{
	public interface IGraphicsDevice
	{
		IPresentationParameters PresentationParameters { get; }
		IWeakReferenceCollection WeakReferences { get; }
		GraphicsProfile GraphicsProfile { get; }
		IGraphicsCapabilities GraphicsCapabilities {get;}
		void CreateDevice (GraphicsAdapter adapter, GraphicsProfile graphicsProfile);
		void Present();
		void Dispose();
	}
}

