using Microsoft.Xna.Framework.Graphics;
using System;

namespace Microsoft.Xna.Framework
{
	public interface IGraphicsDevice : IDisposable
	{
		Viewport Viewport {
			get;
			set;
		}

		void OnDeviceReset ();

		IPresentationParameters PresentationParameters { get; }
		IWeakReferenceCollection WeakReferences { get; }
		GraphicsProfile GraphicsProfile { get; }
		//IGraphicsCapabilities GraphicsCapabilities {get;}
		void CreateDevice (IGraphicsAdapter adapter, GraphicsProfile graphicsProfile);
		void Present();

		void Initialize ();

		void OnDeviceResetting ();
	}
}

