using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Core
{
	public class NullGraphicsDevice : IGraphicsDevice
	{
		#region IGraphicsDevice implementation

		public void ClearToBlack ()
		{
			throw new NotImplementedException ();
		}

		public Viewport Viewport {
			get;
			set;
		}

		public void OnDeviceReset ()
		{
			throw new NotImplementedException ();
		}

		public void OnDeviceResetting ()
		{
			throw new NotImplementedException ();
		}

		public void Initialize ()
		{
			throw new NotImplementedException ();
		}

		public void CreateDevice (IGraphicsAdapter adapter, GraphicsProfile graphicsProfile)
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

		public IPresentationParameters PresentationParameters {
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


		#endregion
	}
}

