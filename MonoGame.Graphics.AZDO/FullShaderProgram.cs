using System;

namespace MonoGame.Graphics.AZDO
{
	public abstract class FullShaderProgram : IShaderProgram
	{
		private ShaderProgram mProgram;
		protected FullShaderProgram (ShaderProgram program)
		{
			mProgram = program;
		}

		public abstract void SetUniformIndex (byte index);
		public abstract byte GetUniformIndex ();
		public abstract void Bind (IConstantBufferCollection buffers);
		public abstract ushort GetBufferMask ();
		public void Use ()
		{
			//GL.UseProgram (mProgram.ProgramID);
		}

		// FIXME : better method name
		public void Unuse()
		{
			//GL.UseProgram (0);
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

