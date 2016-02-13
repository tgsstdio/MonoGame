using System;
using MonoGame.Content;
using MonoGame.Graphics;

namespace NewFences
{
	public class BlockBundle : IDisposable
	{
		BlockIdentifier BlockId { get; set;}
		IMeshBuffer[] Buffers {get;set;}
		IRenderPassGroup Groups {get;set;}

		#region IDisposable implementation

		~BlockBundle()
		{
			Dispose(false);			
		}

		public void Dispose ()
		{
			Dispose (true);
			GC.SuppressFinalize (this);
		}

		protected void ReleaseUnmanagedResources ()
		{

		}

		protected void ReleaseManagedResources ()
		{
			
		}

		private bool mDisposed = false;
		protected void Dispose(bool disposing)
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

