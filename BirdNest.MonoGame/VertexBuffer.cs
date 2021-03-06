﻿using System;
using OpenTK.Graphics.OpenGL;

namespace BirdNest.MonoGame
{
	public abstract class VertexBuffer : IDisposable
	{
		protected VertexBuffer ()
		{
			ArrayId = GL.GenVertexArray();			
		}

		protected abstract void InitialiseBuffers ();
		protected abstract void BindBuffersManually(int programID);
		protected abstract void ReleaseManagedResources ();

		public int ArrayId { get; private set; }
		public int ElementBufferId { get; private set; }


		public void Initialise(int elementBuffer)
		{
			GL.BindVertexArray (ArrayId);
			InitialiseBuffers ();
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

		~ VertexBuffer(){
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

