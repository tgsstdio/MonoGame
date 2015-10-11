using System;
using OpenTK.Graphics.OpenGL;

namespace BirdNest.MonoGame
{
	public abstract class RenderUnit : IDisposable
	{
		protected DrawElementsIndirectCommand[] DrawCommands;	
		protected VertexBuffer TextBuffer;
		protected int Stride;
		protected RenderUnit (VertexBuffer v, DrawElementsIndirectCommand[] dc)
		{
			TextBuffer = v;
			DrawCommands = dc;
			Stride = System.Runtime.InteropServices.Marshal.SizeOf (typeof(DrawElementsIndirectCommand));
		}

		protected abstract void BindShaderStorage();
		protected abstract void UnbindShaderStorage();
		protected abstract void ReleaseManagedResources ();

		public void Render()
		{
			GL.MultiDrawElementsIndirect<DrawElementsIndirectCommand> (
				All.Triangles,
				All.UnsignedInt,
				DrawCommands,
				DrawCommands.Length,
				Stride);
		}

		public void Bind()
		{
			TextBuffer.Bind ();
			BindShaderStorage ();
		}

		public void Unbind()
		{
			UnbindShaderStorage ();
			TextBuffer.Unbind ();
		}

		#region IDisposable implementation
		~RenderUnit()
		{
			Dispose (false);
		}

		public void BindManually(int programID)
		{
			TextBuffer.BindManually (programID);
		}

		public void Dispose ()
		{
			Dispose (true);
			GC.SuppressFinalize (this);
		}

		private bool mDisposed = false;
		protected virtual void Dispose(bool disposing)
		{
			if (mDisposed)
				return;

			if (disposing)
			{
				DrawCommands = null;
				TextBuffer.Dispose ();
				ReleaseManagedResources ();
			}
			mDisposed = true;
		}

		#endregion
	}
}

