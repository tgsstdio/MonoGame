using System;
using Microsoft.Xna.Framework;

namespace MonoGame.Platform.DesktopGL
{
	public class DesktopGraphicsDevice : IGraphicsDevice
	{
		#region IGraphicsDevice implementation

		public void CreateDevice (Microsoft.Xna.Framework.Graphics.GraphicsAdapter adapter, Microsoft.Xna.Framework.Graphics.GraphicsProfile graphicsProfile)
		{
			throw new NotImplementedException ();
		}

		public IWeakReferenceCollection WeakReferences {
			get {
				throw new NotImplementedException ();
			}
		}

		public Microsoft.Xna.Framework.Graphics.GraphicsProfile GraphicsProfile {
			get {
				throw new NotImplementedException ();
			}
		}

		public Microsoft.Xna.Framework.Graphics.IGraphicsCapabilities GraphicsCapabilities {
			get {
				throw new NotImplementedException ();
			}
		}

		public void Present ()
		{
			throw new NotImplementedException ();
		}

		public void Dispose ()
		{
			throw new NotImplementedException ();
		}

		public Microsoft.Xna.Framework.Graphics.PresentationParameters PresentationParameters {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		#endregion


	}
}

