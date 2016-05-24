using System;
using System.Collections.Generic;
using MonoGame.Content;
using MonoGame.Content.Blocks;
using OpenTK.Graphics.OpenGL;

namespace MonoGame.Shaders.GLSL.DesktopGL
{
	public class GLShaderRegistry : IShaderRegistry
	{
		private readonly Dictionary<ulong, GLShaderProgram> mPrograms;
		private readonly IAssetManager mAssetManager;
		public GLShaderRegistry (IAssetManager manager)
		{
			mAssetManager = manager;
			mPrograms = new Dictionary<ulong, GLShaderProgram> ();
		}

		~GLShaderRegistry()
		{
			Dispose (false);
		}

		public void Dispose ()
		{
			Dispose (true);
			GC.SuppressFinalize(this);
		}

		private bool mDisposed = false;
		protected void Dispose(bool disposing)
		{
			if (mDisposed)
				return;

			if (disposing)
			{
				ReleaseManagedResources ();
			}
			mDisposed = true;
		}

		void ReleaseManagedResources()
		{
			foreach (var program in mPrograms.Values)
			{
				GL.DeleteProgram (program.ProgramID);
			}
			mPrograms.Clear ();
		}

		#region IShaderRegistry implementation

		public bool TryGetValue (AssetIdentifier identifier, out GLShaderProgram result)
		{
			return mPrograms.TryGetValue (identifier.AssetId, out result);
		}

		public void Add (AssetInfo key, GLShaderProgram program)
		{
			mAssetManager.Add (key);
			mPrograms.Add (key.Identifier.AssetId, program);
		}

		public void Remove (GLShaderProgram program)
		{
			mPrograms.Remove (program.Identifier.AssetId);
			mAssetManager.Remove (program.Identifier);
		}

		#endregion
	}
}

