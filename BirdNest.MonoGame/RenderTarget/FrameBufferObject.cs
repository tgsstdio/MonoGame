using System;
using BirdNest.MonoGame.Graphics;

namespace BirdNest.MonoGame
{
	public class FrameBufferObject : IRenderTarget
	{
		public FrameBufferObject (Func<InstanceIdentifier> identityGenerator)
		{
			Id = identityGenerator ();
		}

		#region IRenderTarget implementation

		public void Switch ()
		{
			throw new NotImplementedException ();
		}

		public InstanceIdentifier Id {
			private set;
			get;
		}

		#endregion

		#region IDisposable implementation

		~FrameBufferObject()
		{
			Dispose (false);
		}

		public void Dispose ()
		{
			Dispose (true);
			GC.SuppressFinalize (this);
		}

		void ReleaseManagedResources()
		{

		}

		void ReleaseUnmanagedResources()
		{

		}

		private bool mDisposed = false;
		protected virtual void Dispose(bool disposing)
		{
			if (mDisposed)
				return;

			ReleaseUnmanagedResources ();

			if (disposing)
			{
				ReleaseManagedResources ();
			}
			mDisposed = true;
		}

		#endregion
	}
}

