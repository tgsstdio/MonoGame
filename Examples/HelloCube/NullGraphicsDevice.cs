using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HelloCube
{
	public class NullGraphicsDevice : IGraphicsDevice
	{
		#region IGraphicsDevice implementation

		public void CreateDevice (GraphicsAdapter adapter, GraphicsProfile graphicsProfile)
		{
			throw new NotImplementedException ();
		}

		public void Present ()
		{
			throw new NotImplementedException ();
		}

		public void Dispose ()
		{
			throw new NotImplementedException ();
		}

		public PresentationParameters PresentationParameters {
			get {
				throw new NotImplementedException ();
			}
		}

		public IWeakReferenceCollection WeakReferences {
			get {
				throw new NotImplementedException ();
			}
		}

		public GraphicsProfile GraphicsProfile {
			get {
				throw new NotImplementedException ();
			}
		}

		public IGraphicsCapabilities GraphicsCapabilities {
			get {
				throw new NotImplementedException ();
			}
		}

		#endregion
	}
}

