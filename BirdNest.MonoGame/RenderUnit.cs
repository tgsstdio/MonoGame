using System;
using OpenTK.Graphics.OpenGL;

namespace BirdNest.MonoGame
{
	public class RenderUnit : IDisposable
	{
		protected IDrawElementsCommandFilter Filter;	
		protected VertexBuffer Buffer;
		protected int Stride;
		public RenderUnit (VertexBuffer v, IDrawElementsCommandFilter f)
		{
			Buffer = v;
			Filter = f;
			Stride = System.Runtime.InteropServices.Marshal.SizeOf (typeof(DrawElementsIndirectCommand));
		}

		protected virtual void BindShaderStorage()
		{

		}

		protected virtual void UnbindShaderStorage()
		{

		}

		protected virtual void ReleaseManagedResources ()
		{

		}

		public virtual void InitialiseUniforms (int programID)
		{

		}

		public void Render()
		{
			var commands = Filter.ToArray ();
			GL.MultiDrawElementsIndirect<DrawElementsIndirectCommand> (
				All.Triangles,
				All.UnsignedInt,
				commands,
				commands.Length,
				Stride);
		}

		public void Bind()
		{
			Buffer.Bind ();
			BindShaderStorage ();
		}

		public void Unbind()
		{
			UnbindShaderStorage ();
			Buffer.Unbind ();
		}

		public void BindManually(int programID)
		{
			Buffer.BindManually (programID);
		}

		#region IDisposable implementation
		~RenderUnit()
		{
			Dispose (false);
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
				Filter = null;
				Buffer = null;
				ReleaseManagedResources ();
			}
			mDisposed = true;
		}

		#endregion
	}
}

