using System;
using Microsoft.Xna.Framework;

namespace MonoGame.Platform.DesktopGL
{
	public class DesktopGraphicsDevice : IGraphicsDevice
	{
		#region IGraphicsDevice implementation

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

