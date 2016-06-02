using System;
using OpenTK.Graphics.OpenGL;

namespace MonoGame.Graphics.AZDO
{
	public abstract class FullVertexBufferObject  : IVertexBufferObject
	{
		protected FullVertexBufferObject ()
		{
			ArrayId = GL.GenVertexArray();			
		}

		protected abstract void InitializeBuffers ();
		protected abstract void BindBuffersManually(int programID);
		protected abstract void ReleaseManagedResources ();

		public int ArrayId { get; private set; }
		public int ElementBufferId { get; private set; }

		public void Initialize(int elementBuffer)
		{
			GL.BindVertexArray (ArrayId);
			InitializeBuffers ();
			ElementBufferId = elementBuffer;
			GL.BindBuffer (BufferTarget.ElementArrayBuffer, ElementBufferId);
			GL.BindVertexArray (0);
		}

		public void Bind()
		{
			GL.BindVertexArray (ArrayId);
			GL.BindBuffer (BufferTarget.ElementArrayBuffer, ElementBufferId);
		}

		public void Unbind()
		{
			GL.BindVertexArray (0);
			GL.BindBuffer (BufferTarget.ElementArrayBuffer, 0);
		}

		public void BindManually(int programID)
		{
			GL.BindVertexArray (ArrayId);
			BindBuffersManually (programID);
			GL.BindBuffer (BufferTarget.ArrayBuffer, 0);
		}

		#region IDisposable implementation

		~ FullVertexBufferObject(){
			Dispose(false);
		}

		public void Dispose ()
		{
			Dispose (true);
			GC.SuppressFinalize(this);
		}

		void ReleaseUnmanagedResources ()
		{
			GL.DeleteVertexArray (ArrayId);
		}

		private bool mDisposed = false;
		protected virtual void Dispose(bool disposing)
		{
			if (mDisposed)
			{
				return;
			}

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

