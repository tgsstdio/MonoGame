using System;

namespace Magnesium.OpenGL
{
	public abstract class FullShaderProgram : IShaderProgram
	{
		protected FullShaderProgram ()
		{
			
		}

		public void Use ()
		{
			throw new NotImplementedException ();
		}
		public void Unuse ()
		{
			throw new NotImplementedException ();
		}

		public byte DescriptorSet {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}
		public int VBO {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		#region IDisposable implementation
		~FullShaderProgram()
		{
			Dispose (false);
		}

		public void Dispose ()
		{
			Dispose (true);
			GC.SuppressFinalize (this);
		}

		protected void ReleaseUnmanagedResources()
		{
			//GL.DeleteProgram (mProgram.ProgramID);
		}

//		void ReleaseManagedResources()
//		{
//
//		}

		private bool mDisposed = false;
		protected virtual void Dispose(bool disposing)
		{
			if (mDisposed)
				return;

			ReleaseUnmanagedResources ();

			if (disposing)
			{
			//	ReleaseManagedResources ();
			}
			mDisposed = true;
		}

		#endregion
	}
}

