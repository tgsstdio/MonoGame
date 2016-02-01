using System;
using OpenTK.Graphics.OpenGL;
using MonoGame.Graphics;

namespace BirdNest.MonoGame
{
	public class ShaderProgramRoot : IDisposable
	{
		private ShaderProgram mProgram;
		public ShaderProgramRoot (ShaderProgram program)
		{
			mProgram = program;
		}

		public void Use()
		{
			GL.UseProgram (mProgram.ProgramID);
		}

		// FIXME : better method name
		public void Unuse()
		{
			GL.UseProgram (0);
		}

		#region IDisposable implementation
		~ShaderProgramRoot()
		{
			Dispose (false);
		}

		public void Dispose ()
		{
			Dispose (true);
			GC.SuppressFinalize (this);
		}

		void ReleaseUnmanagedResources()
		{
			GL.DeleteProgram (mProgram.ProgramID);
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

